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

                var obj = _repoUnit.FormucuaRepository.FindAsync(x => x.RmmhPkRefNo == domainModelFormUCUA.RmmhPkRefNo && x.RmmhRefId == domainModelFormUCUA.RmmhRefId && x.RmmhDateReceived == domainModelFormUCUA.RmmhDateReceived && x.RmmhActiveYn == true).Result;
                //var obj = _repoUnit.FormucuaRepository.FindAsync(x => x.RmmhRefId == domainModelFormUCUA.RmmhRefId && x.RmmhActiveYn == true).Result;
                var MaxPkrefNo = _repoUnit.FormucuaRepository._context.RmUcua.Select(x => x.RmmhPkRefNo).Max();
                MaxPkrefNo = MaxPkrefNo + 1;
                if (obj != null)
                {
                    var res = _mapper.Map<FormUCUAResponseDTO>(obj);
                    res.FormExist = true;
                    return res;
                }

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("RefNo", MaxPkrefNo.ToString());
                lstData.Add("YYYYMMDD", Utility.ToString(Convert.ToDateTime(FormUCUA.DateReceived).ToString("yyyyMMdd")));
                domainModelFormUCUA.RmmhRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormUCUA, lstData);
                domainModelFormUCUA.RmmhStatus = "Initialize";
                domainModelFormUCUA.RmmhUnsafeAct = FormUCUA.hdnUnsafeAct;
                domainModelFormUCUA.RmmhUnsafeCondition = FormUCUA.hdnUnsafeCondition;

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
                domainModelformUcua.RmmhUnsafeAct = FormUCUA.hdnUnsafeAct;
                domainModelformUcua.RmmhUnsafeCondition = FormUCUA.hdnUnsafeCondition;
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
                form.RmmhAuditLog = Utility.ProcessLog(form.RmmhAuditLog, "Submitted By", "Submitted", string.Empty, string.Empty, form.RmmhDateReceived, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Recorded By:" + form.RmmhReportingName + " - Form Ucua (" + form.RmmhPkRefNo + ")",
                 //   RmNotMessage = "Recorded By:" + " - Form Ucua (" + form.RmmhPkRefNo + ")",
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
                Oldfilename = filepath + filename + ".xlsx";
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
                FormUCUARpt rpt = await this.GetReportData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    int noofsheets = 1;


                    //for (int sheet = 1; sheet <= noofsheets; sheet++)
                    //{


                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    // workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);
                    using (var book = new XLWorkbook(cachefile))
                    {

                        if (worksheet != null)
                        {
                            // worksheet.Cell(1, 13).Value = rpt.RefId;
                            worksheet.Cell(2, 4).Value = rpt.ReportingName;
                            //worksheet.Cell(4, 19).Value = rpt.Location;
                            //worksheet.Cell(4, 22).Value = rpt.WorkScope;
                            //worksheet.Cell(4, 26).Value = rpt.UnsafeAct;
                            //worksheet.Cell(4, 35).Value = rpt.UnsafeActDescription;
                            //worksheet.Cell(5, 6).Value = rpt.UnsafeCondition;
                            //worksheet.Cell(5, 17).Value = rpt.UnsafeConditionDescription;
                            //worksheet.Cell(5, 29).Value = rpt.ImprovementRecommendation;
                            //worksheet.Cell(6, 6).Value = rpt.DateReceived.HasValue ? rpt.DateReceived.Value.ToString("dd-MM-yyyy") : ""; 

                            //worksheet.Cell(7, 4).Value = rpt.DateCommitteeReview.HasValue ? rpt.DateCommitteeReview.Value.ToString("dd-MM-yyyy") : "";
                            //worksheet.Cell(25, 4).Value = rpt.CommentsOfficeUse;
                            //worksheet.Cell(49, 4).Value = rpt.HseSection;
                            //worksheet.Cell(7, 38).Value = rpt.SafteyCommitteeChairman;
                            //worksheet.Cell(25, 38).Value = rpt.ImsRep;
                            //worksheet.Cell(49, 38).Value = rpt.DateActionTaken.HasValue ? rpt.DateActionTaken.Value.ToString("dd-MM-yyyy") : ""; 

                            //worksheet.Cell(51, 32).Value = rpt.ActionTakenBy;

                            //worksheet.Cell(63, 1).Value = rpt.ActionDescription;

                            //worksheet.Cell(63, 33).Value = rpt.EffectivenessActionTakenBy;
                            //worksheet.Cell(64, 33).Value = rpt.EffectivenessActionDescription;
                            //worksheet.Cell(65, 33).Value = rpt.DateEffectivenessActionTaken.HasValue ? rpt.DateEffectivenessActionTaken.Value.ToString("dd-MM-yyyy") : "";

                            //worksheet.Cell(67, 33).Value = rpt.Status;
                            //worksheet.Cell(68, 33).Value = rpt.ActiveYn;
                            //worksheet.Cell(69, 33).Value = rpt.SubmitYn;

                        }
                    }
                        // }


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
        public async Task<FormUCUARpt> GetReportData(int headerid)
        {
            return await _repo.GetReportData(headerid);
        }
        //public async Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions)
        //{
        //    PagingResult<FormUCUAHeaderRequestDTO> result = new PagingResult<FormUCUAHeaderRequestDTO>();
        //    List<FormUCUAHeaderRequestDTO> formAlist = new List<FormUCUAHeaderRequestDTO>();
        //    result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
        //    result.TotalRecords = result.PageResult.Count();
        //    result.PageNo = filterOptions.StartPageNo;
        //    result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
        //    return result;
        //}
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
        public int? DeleteFormUCUA(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormUCUA(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }
    }

}
