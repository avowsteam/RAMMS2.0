using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Common;
using RAMMS.Common.RefNumber;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormUCUAService: IFormUCUAService
    {
        private readonly IFormUCUARepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormUCUAService(IRepositoryUnit repoUnit, IFormUCUARepository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }

        public async Task<FormUCUAResponseDTO> GetHeaderById(int id)
        {
            var header = await _repoUnit.FormucuaRepository.FindAsync(s => s.RmmhPkRefNo == id);
            if (header == null)
            {
                return null;
            }
            return _mapper.Map<FormUCUAResponseDTO>(header);
        }
        
        public async Task<FormUCUAResponseDTO> SaveFormUCUA(FormUCUAResponseDTO FormUCUA)
        {
            try
            {
                var domainModelFormUCUA = _mapper.Map<RmUcua>(FormUCUA);
                domainModelFormUCUA.RmmhPkRefNo = 0;


                var obj = _repoUnit.FormucuaRepository.FindAsync(x => x.RmmhRefId == domainModelFormUCUA.RmmhRefId && x.RmmhActiveYn == true).Result;
                if (obj != null)
                {
                    var res = _mapper.Map<FormUCUAResponseDTO>(obj);
                    res.FormExist = true;
                    return res;
                }

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("RoadCode", domainModelFormUCUA.RmmhRefId);
                lstData.Add("YYYYMMDD", Utility.ToString(Convert.ToDateTime(FormUCUA.DateReceived).ToString("yyyyMMdd")));
                domainModelFormUCUA.RmmhRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormUCUA, lstData);
                domainModelFormUCUA.RmmhStatus = "Initialize";

                var entity = _repoUnit.FormucuaRepository.CreateReturnEntity(domainModelFormUCUA);
                FormUCUA.PkRefNo = _mapper.Map<FormUCUAResponseDTO>(entity).PkRefNo;
                FormUCUA.RefId = domainModelFormUCUA.RmmhRefId;
                FormUCUA.Status = domainModelFormUCUA.RmmhStatus;

                return FormUCUA;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> Update(FormUCUAResponseDTO FormUCUA)
        {
            int rowsAffected;
            try
            {
                int PkRefNo = FormUCUA.PkRefNo;
                int? Fw1PkRefNo = FormUCUA.PkRefNo;

                var domainModelformUcua = _mapper.Map<RmUcua>(FormUCUA);
                domainModelformUcua.RmmhPkRefNo = PkRefNo;

                domainModelformUcua.RmmhActiveYn = true;
                domainModelformUcua = UpdateStatus(domainModelformUcua);
                _repoUnit.FormucuaRepository.Update(domainModelformUcua);
                rowsAffected = await _repoUnit.CommitAsync();
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }
        public RmUcua UpdateStatus(RmUcua form)
        {
            if (form.RmmhPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormucuaRepository._context.RmUcua.Where(x => x.RmmhPkRefNo == form.RmmhPkRefNo).Select(x => new { Status = x.RmmhStatus, Log = x.RmmhAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.RmmhAuditLog = existsObj.Log;
                    form.RmmhStatus = existsObj.Status;

                }

            }
            if (form.RmmhSubmitYn && (form.RmmhStatus == "Saved" || form.RmmhStatus == "Initialize"))
            {
                form.RmmhStatus = Common.StatusList.FormUcuaSubmitted;
                form.RmmhAuditLog = Utility.ProcessLog(form.RmmhAuditLog, "Submitted By", "Submitted", form.RmmhReportingName, string.Empty, form.RmmhDateReceived, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Recorded By:" + " - Form Ucua (" + form.RmmhPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/InstructedWorks/EditFormT?id=" + form.RmmhPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }
        public async Task<byte[]> FormDownload(string formname, int id, string filepath)
        {
            string Oldfilename = "";
            string filename = "";
            string cachefile = "";
            if (!filepath.Contains(".xlsx"))
            {
                Oldfilename = filepath + formname + ".xlsx";
                filename = formname + DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString();
                cachefile = filepath + filename + ".xlsx";
            }
            else
            {
                Oldfilename = filepath;
                filename = filepath.Replace(".xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString() + ".xlsx");
                cachefile = filename;
            }

            try
            {
                FORMTRpt rpt = await this.GetReportData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    int noofsheets = 1;


                    for (int sheet = 1; sheet <= noofsheets; sheet++)
                    {


                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {
                            // worksheet.Cell(1, 13).Value = rpt.RefId;
                            worksheet.Cell(3, 35).Value = rpt.InspectedDate.HasValue ? rpt.InspectedDate.Value.ToString("dd-MM-yyyy") : "";
                            worksheet.Cell(4, 8).Value = rpt.RMU;
                            worksheet.Cell(4, 19).Value = rpt.Details.Day;
                            worksheet.Cell(4, 22).Value = rpt.Details.TotalDay;
                            worksheet.Cell(4, 26).Value = rpt.Details.HourlycountPerDay;
                            worksheet.Cell(4, 35).Value = rpt.RefNo;
                            worksheet.Cell(5, 6).Value = rpt.RoadCode;
                            worksheet.Cell(5, 17).Value = rpt.Details.DirectionFrom;
                            worksheet.Cell(5, 29).Value = rpt.Details.DirectionTo;
                            worksheet.Cell(6, 6).Value = rpt.RoadName;

                            worksheet.Cell(7, 4).Value = rpt.Details.FromTime;
                            worksheet.Cell(25, 4).Value = rpt.Details.FromTime;
                            worksheet.Cell(49, 4).Value = rpt.Details.FromTime;
                            worksheet.Cell(7, 38).Value = rpt.TotalPC;
                            worksheet.Cell(25, 38).Value = rpt.TotalHV;
                            worksheet.Cell(49, 38).Value = rpt.TotalMC;

                            int colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "PC").FirstOrDefault();
                                worksheet.Cell(8, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }
                            worksheet.Cell(9, 32).Value = rpt.Details.DescriptionPC;

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "2R" && x.Loading == "2RE").FirstOrDefault();
                                worksheet.Cell(26, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "2R" && x.Loading == "2RN").FirstOrDefault();
                                worksheet.Cell(27, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "2R" && x.Loading == "2RO").FirstOrDefault();
                                worksheet.Cell(28, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3R" && x.Loading == "3RE").FirstOrDefault();
                                worksheet.Cell(29, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3R" && x.Loading == "3RN").FirstOrDefault();
                                worksheet.Cell(30, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3R" && x.Loading == "3RO").FirstOrDefault();
                                worksheet.Cell(31, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3A" && x.Loading == "3AE").FirstOrDefault();
                                worksheet.Cell(32, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3A" && x.Loading == "3AN").FirstOrDefault();
                                worksheet.Cell(33, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "3A" && x.Loading == "3AO").FirstOrDefault();
                                worksheet.Cell(34, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "4A" && x.Loading == "4AE").FirstOrDefault();
                                worksheet.Cell(35, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "4A" && x.Loading == "4AN").FirstOrDefault();
                                worksheet.Cell(36, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "4A" && x.Loading == "4AO").FirstOrDefault();
                                worksheet.Cell(37, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "5A" && x.Loading == "5AE").FirstOrDefault();
                                worksheet.Cell(38, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "5A" && x.Loading == "5AN").FirstOrDefault();
                                worksheet.Cell(39, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "5A" && x.Loading == "5AO").FirstOrDefault();
                                worksheet.Cell(40, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "6A" && x.Loading == "6AE").FirstOrDefault();
                                worksheet.Cell(41, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "6A" && x.Loading == "6AN").FirstOrDefault();
                                worksheet.Cell(42, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "6A" && x.Loading == "6AO").FirstOrDefault();
                                worksheet.Cell(43, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "7A" && x.Loading == "7AE").FirstOrDefault();
                                worksheet.Cell(44, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "7A" && x.Loading == "7AN").FirstOrDefault();
                                worksheet.Cell(45, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "HV" && x.Axle == "7A" && x.Loading == "7AO").FirstOrDefault();
                                worksheet.Cell(46, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }
                            worksheet.Cell(27, 32).Value = rpt.Details.DescriptionHV;

                            colindex = 8;
                            for (int i = 1; i <= 12; i++)
                            {
                                var obj = rpt.Details.Vechilce.Where(x => x.Time == (i * 5) && x.VechicleType == "MC").FirstOrDefault();
                                worksheet.Cell(50, colindex).Value = obj == null ? "" : obj.Count.ToString();
                                colindex = colindex + 2;
                            }
                            worksheet.Cell(51, 32).Value = rpt.Details.DescriptionMC;

                            worksheet.Cell(63, 1).Value = rpt.Details.Description;

                            worksheet.Cell(63, 33).Value = rpt.RecName;
                            worksheet.Cell(64, 33).Value = rpt.RecDesg;
                            worksheet.Cell(65, 33).Value = rpt.RecDate.HasValue ? rpt.RecDate.Value.ToString("dd-MM-yyyy") : "";

                            worksheet.Cell(67, 33).Value = rpt.HdName;
                            worksheet.Cell(68, 33).Value = rpt.HdDesg;
                            worksheet.Cell(69, 33).Value = rpt.HdDate.HasValue ? rpt.HdDate.Value.ToString("dd-MM-yyyy") : "";

                        }
                    }


                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        System.IO.File.Delete(cachefile);
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        System.IO.File.Delete(cachefile);
                        return content;
                    }
                }

            }
        }

        public async Task<FORMTRpt> GetReportData(int headerid)
        {
            return await _repo.GetReportData(headerid);
        }
        public async Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions)
        {
            PagingResult<FormUCUAHeaderRequestDTO> result = new PagingResult<FormUCUAHeaderRequestDTO>();
            List<FormUCUAHeaderRequestDTO> formAlist = new List<FormUCUAHeaderRequestDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }
        public async Task<int> DeActivateFormT(int formNo)
        {
            int rowsAffected;
            try
            {
                var domainModelFormT = await _repoUnit.FormTRepository.GetByIdAsync(formNo);
                domainModelFormT.FmtActiveYn = false;
                _repoUnit.FormTRepository.Update(domainModelFormT);
                rowsAffected = await _repoUnit.CommitAsync();
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }

    }

}
