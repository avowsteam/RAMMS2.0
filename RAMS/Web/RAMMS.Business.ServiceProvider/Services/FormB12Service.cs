using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.Repository.Interfaces;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormB12Service : IFormB12Service
    {
        private readonly IFormB12Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;
        public FormB12Service(IRepositoryUnit repoUnit, IFormB12Repository repo,
            IAssetsService assetsService, IMapper mapper, IProcessService proService,
            ISecurity security)
        {
            _repo = repo;
            _mapper = mapper;
            _assetsService = assetsService;
            _repoUnit = repoUnit;
            processService = proService;
            _security = security;
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            return await _repo.GetHeaderGrid(searchData);
        }

        public async Task<FormB12DTO> FindDetails(FormB12DTO frmB12, int createdBy)
        {
            RmB12Hdr header = _mapper.Map<RmB12Hdr>(frmB12);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmB12 = _mapper.Map<FormB12DTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmB12.Status = "Initialize";

                frmB12.CrBy = createdBy;
                frmB12.CrDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();

                lstData.Add("YYYY", frmB12.RevisionYear.ToString());
                lstData.Add("RevisionNo", frmB12.RevisionNo.ToString());
                frmB12.PkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormB12, lstData);

                header = _mapper.Map<RmB12Hdr>(frmB12);
                header = await _repo.Save(header, false);
                frmB12 = _mapper.Map<FormB12DTO>(header);
            }
            return frmB12;
        }


        public async Task<FormB12DTO> GetHeaderById(int id, bool view)
        {
            RmB12Hdr res = _repo.GetHeaderById(id,view);
            FormB12DTO FormB12 = new FormB12DTO();
            FormB12 = _mapper.Map<FormB12DTO>(res);
            FormB12.FormB12History = _mapper.Map<List<FormB12HistoryDTO>>(res.RmB12DesiredServiceLevelHistory);
           
            return FormB12;
        }

        public int? GetMaxRev(int Year)
        {
            return _repo.GetMaxRev(Year);
        }

        public async Task<int> SaveFormB12(FormB12DTO FormB12)
        {
            try
            {
                var domainModelFormB12 = _mapper.Map<List<RmB12DesiredServiceLevelHistory>>(FormB12);
                foreach (var list in domainModelFormB12)
                {
                    list.B12dslhPkRefNo= 0;
                }

                return await _repo.SaveFormB12(domainModelFormB12);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<FormB12DTO> SaveB12(FormB12DTO frmb12hdr, bool updateSubmit)
        {
            RmB12Hdr frmb14hdr_1 = this._mapper.Map<RmB12Hdr>((object)frmb12hdr);
            frmb14hdr_1 = UpdateStatus(frmb14hdr_1);

            RmB12Hdr source = await this._repo.Save(frmb14hdr_1, updateSubmit);

            var domainModelFormB12 = _mapper.Map<List<RmB12DesiredServiceLevelHistory>>(frmb12hdr.FormB12History);
            foreach (var list in domainModelFormB12)
            {
                list.B12dslhPkRefNo = list.B12dslhPkRefNo;
                list.B12dslhB12hPkRefNo = frmb12hdr.PkRefNo;
            }
            await _repo.SaveFormB12(domainModelFormB12);


            frmb12hdr = this._mapper.Map<FormB12DTO>((object)source);
            return frmb12hdr;
        }

        public RmB12Hdr UpdateStatus(RmB12Hdr form)
        {
            if (form.B12hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmB12Hdr.Where(x => x.B12hPkRefNo == form.B12hPkRefNo).Select(x => new { Status = x.B12hStatus, Log = x.B12hAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.B12hAuditlog = existsObj.Log;
                    form.B12hStatus = existsObj.Status;
                }

            }


            if (form.B12hSubmitSts && (string.IsNullOrEmpty(form.B12hStatus) || form.B12hStatus == Common.StatusList.FormQA1Saved ))
            {

                form.B12hStatus = Common.StatusList.FormQA1Submitted;
                form.B12hAuditlog = Utility.ProcessLog(form.B12hAuditlog, "Submitted", "Submitted", form.B12hCrByName, string.Empty, form.B12hCrDt, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Submitted By:" + form.B12hCrByName + " - Form B12 (" + form.B12hPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormB12/Edit/" + form.B12hPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            else if (string.IsNullOrEmpty(form.B12hStatus) || form.B12hStatus == "Initialize")
                form.B12hStatus = Common.StatusList.FormR1R2Saved;

            return form;
        }


        public byte[] FormDownload(string formname, int id, string basepath, string filepath)
        {
            //string structureCode = _repoUnit.DDLookUpRepository.GetConcatenateDdlTypeValue(new DTO.RequestBO.DDLookUpDTO { Type = "Structure Code", TypeCode = "Y" });
            string Oldfilename = "";
            string filename = "";
            string cachefile = "";
            basepath = $"{basepath}/Uploads";
            if (!filepath.Contains(".xlsx"))
            {
                Oldfilename = filepath + formname + ".xlsx";// formdetails.FgdFilePath+"\\" + formdetails.FgdFileName+ ".xlsx";
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
                FormB12Rpt _rpt = this.GetReportData(id).Result;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);

                    using (var book = new XLWorkbook(cachefile))
                    {
                        if (worksheet != null)
                        {
                            worksheet.Cell(1, 1).Value = "APPENDIX B12: L.E.M Unit Price (" + _rpt.Year + ") for RMU Batu Niah and RMU Miri";

                            var Labour = _rpt.B12History;
                            var i = 3;
                            foreach (var lab in Labour)
                            {
                                worksheet.Cell(i + 1, 1).Value = lab.Code;
                                worksheet.Cell(i + 1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                worksheet.Cell(i + 1, 1).Style.Font.SetFontSize(10);


                                worksheet.Cell(i + 1, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 1, 2).Value = lab.Name;
                                worksheet.Cell(i + 1, 2).Style.Font.SetFontSize(10);

                                worksheet.Cell(i + 1, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                worksheet.Cell(i + 1, 3).Value = lab.Unit + " hrs";
                                worksheet.Cell(i + 1, 3).Style.Font.SetFontSize(10);

                                worksheet.Cell(i + 1, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                worksheet.Cell(i + 1, 4).Value = lab.UnitPriceBatuNiah;
                                worksheet.Cell(i + 1, 4).Style.Font.SetFontSize(10);

                                worksheet.Cell(i + 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(i + 1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                worksheet.Cell(i + 1, 5).Value = lab.UnitPriceMiri;
                                worksheet.Cell(i + 1, 5).Style.Font.SetFontSize(10);
                                i++;
                            }

                            i+= 2;

                            ClosedXML.Excel.IXLRange range = worksheet.Range(i, 1, i+1, 1);
                            range.Merge(true);
                            worksheet.Cell(i, 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 1).Style.Font.SetBold(true);
                            worksheet.Cell(i, 1).Style.Font.SetFontSize(11);
                            worksheet.Cell(i, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 1).Value = "CODE";

                            ClosedXML.Excel.IXLRange range1 = worksheet.Range(i, 2, i + 1, 2);
                            range1.Merge(true);
                            worksheet.Cell(i, 2).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 2).Style.Font.SetBold(true);
                            worksheet.Cell(i, 2).Style.Font.SetFontSize(11);
                            worksheet.Cell(i, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 2).Value = "MATERIALS";

                            ClosedXML.Excel.IXLRange range2 = worksheet.Range(i, 3, i + 1, 3);
                            range2.Merge(true);
                            worksheet.Cell(i, 3).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 3).Style.Font.SetBold(true);
                            worksheet.Cell(i, 3).Style.Font.SetFontSize(11);
                            worksheet.Cell(i, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 3).Value = "UNIT";


                            ClosedXML.Excel.IXLRange range3 = worksheet.Range(i, 4, i, 5);
                            range3.Merge(true);
                            worksheet.Cell(i, 4).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 4).Style.Font.SetBold(true);
                            worksheet.Cell(i, 4).Style.Font.SetFontSize(9);
                            worksheet.Cell(i, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 4).Value = "MIRI DIVISION";
                            i += 1;
                            worksheet.Cell(i, 4).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 4).Style.Font.SetBold(true);
                            worksheet.Cell(i, 4).Style.Font.SetFontSize(11);
                            worksheet.Cell(i, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 4).Value = "Batu Niah";

                            worksheet.Cell(i, 5).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            worksheet.Cell(i, 5).Style.Font.SetBold(true);
                            worksheet.Cell(i, 5).Style.Font.SetFontSize(9);
                            worksheet.Cell(i, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(i, 5).Value = "Miri";


                           

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

        public async Task<FormB12Rpt> GetReportData(int headerid)
        {
            return await _repo.GetReportData(headerid);
        }

        public int Delete(int id)
        {
            ///if (id > 0 && !_repo.isF1Exist(id))
            if (id > 0)
            {
                id = _repo.DeleteHeader(new RmB12Hdr() { B12hActiveYn = false, B12hPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<List<FormB12HistoryDTO>> GetHistoryData(int id)
        {
            List<RmB12DesiredServiceLevelHistory> res = _repo.GetHistoryData(id);
            List<FormB12HistoryDTO> FormB12 = new List<FormB12HistoryDTO>();
            FormB12 = _mapper.Map<List<FormB12HistoryDTO>>(res);
            return FormB12;
        }

        public async Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetDataMiri( int year)
        {
            List<RmB13ProposedPlannedBudgetHistory> res = _repo.GetPlannedBudgetDataMiri(year);
            List<FormB13HistoryResponseDTO> FormB13 = new List<FormB13HistoryResponseDTO>();
            FormB13 = _mapper.Map<List<FormB13HistoryResponseDTO>>(res);
            return FormB13;
        }

        public async Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetDataBTN(int year)
        {
            List<RmB13ProposedPlannedBudgetHistory> res = _repo.GetPlannedBudgetDataBTN(year);
            List<FormB13HistoryResponseDTO> FormB13 = new List<FormB13HistoryResponseDTO>();
            FormB13 = _mapper.Map<List<FormB13HistoryResponseDTO>>(res);
            return FormB13;
        }

        public async Task<List<FormB10HistoryResponseDTO>> GetUnitData(int year)
        {
            List<RmB10DailyProductionHistory> res = _repo.GetUnitData(year);
            List<FormB10HistoryResponseDTO> FormB10 = new List<FormB10HistoryResponseDTO>();
            FormB10 = _mapper.Map<List<FormB10HistoryResponseDTO>>(res);
            return FormB10;
        }


    }
}
