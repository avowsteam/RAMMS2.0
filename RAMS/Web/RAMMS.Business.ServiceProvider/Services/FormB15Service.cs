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
    public class FormB15Service : IFormB15Service
    {
        private readonly IFormB15Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;
        public FormB15Service(IRepositoryUnit repoUnit, IFormB15Repository repo,
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

        public async Task<FormB15HeaderDTO> GetHeaderById(int id, bool view)
        {
            RmB15Hdr res = _repo.GetHeaderById(id, view);
            FormB15HeaderDTO FormB15 = new FormB15HeaderDTO();
            FormB15 = _mapper.Map<FormB15HeaderDTO>(res);
            FormB15.RmB15History = _mapper.Map<List<FormB15HistoryDTO>>(res.RmB15History);
            return FormB15;
        }

        public int Delete(int id)
        {
            if (id > 0 && !_repo.isF1Exist(id))
            {
                id = _repo.DeleteHeader(new RmB15Hdr() { B15hActiveYn = false, B15hPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<FormB15HeaderDTO> FindDetails(FormB15HeaderDTO frmB15, int createdBy)
        {
            RmB15Hdr header = _mapper.Map<RmB15Hdr>(frmB15);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmB15 = _mapper.Map<FormB15HeaderDTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmB15.Status = "Initialize";

                //frmR1R2.InspectedBy = _security.UserID;
                //frmR1R2.InspectedName = _security.UserName;
                //frmR1R2.InspectedDt = DateTime.Today;
                frmB15.CrBy = frmB15.ModBy = createdBy;
                frmB15.CrDt = frmB15.ModDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", frmB15.RevisionYear.ToString());
                lstData.Add("RevisionNo", frmB15.RevisionNo.ToString());
                frmB15.PkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormB15, lstData);

                header = _mapper.Map<RmB15Hdr>(frmB15);
                header = await _repo.Save(header, false);
                frmB15 = _mapper.Map<FormB15HeaderDTO>(header);
            }
            return frmB15;
        }

        public int? GetMaxRev(int Year, string RmuCode)
        {
            return _repo.GetMaxRev(Year, RmuCode);
        }

        public async Task<int> SaveFormB15(List<FormB15HistoryDTO> FormB15)
        {
            try
            {
                var domainModelFormB15 = _mapper.Map<List<RmB15History>>(FormB15);
                foreach (var list in domainModelFormB15)
                {
                    list.B15hhPkRefNoHistory = 0;
                }

                return await _repo.SaveFormB15(domainModelFormB15);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<FormB15HeaderDTO> SaveB15(FormB15HeaderDTO frmb15hdr, List<FormB15HistoryDTO> frmb15, bool updateSubmit)
        {
            RmB15Hdr frmb15hdr_1 = this._mapper.Map<RmB15Hdr>((object)frmb15hdr);
            frmb15hdr_1 = UpdateStatus(frmb15hdr_1);

            RmB15Hdr source = await this._repo.Save(frmb15hdr_1, updateSubmit);

            var domainModelFormB15 = _mapper.Map<List<RmB15History>>(frmb15);
            foreach (var list in domainModelFormB15)
            {
                list.B15hhPkRefNoHistory = list.B15hhPkRefNoHistory;
                list.B15hhB15hPkRefNo = frmb15hdr.PkRefNo;
            }
            await _repo.SaveFormB15(domainModelFormB15);


            frmb15hdr = this._mapper.Map<FormB15HeaderDTO>((object)source);
            return frmb15hdr;
        }

        public RmB15Hdr UpdateStatus(RmB15Hdr form)
        {
            if (form.B15hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmB15Hdr.Where(x => x.B15hPkRefNo == form.B15hPkRefNo).Select(x => new { Status = x.B15hStatus, Log = x.B15hAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.B15hAuditlog = existsObj.Log;
                    form.B15hStatus = existsObj.Status;
                }

            }
            

            if (form.B15hSubmitSts && (string.IsNullOrEmpty(form.B15hStatus) || form.B15hStatus == Common.StatusList.FormQA1Saved || form.B15hStatus == Common.StatusList.FormQA1Rejected))
            {
                //form.Fg1hInspectedBy = _security.UserID;
                //form.Fg1hInspectedName = _security.UserName;
                //form.Fg1hInspectedDt = DateTime.Today;
                form.B15hStatus = Common.StatusList.FormQA1Submitted;
                form.B15hAuditlog = Utility.ProcessLog(form.B15hAuditlog, "Submitted", "Submitted", form.B15hUserNameProsd, string.Empty, form.B15hDtProsd, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Executed By:" + form.B15hUserNameProsd + " - Form M (" + form.B15hPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormB15/Edit/" + form.B15hPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            else if (string.IsNullOrEmpty(form.B15hStatus) || form.B15hStatus == "Initialize")
                form.B15hStatus = Common.StatusList.FormR1R2Saved;

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
                List<FormB15Rpt> _rpt = this.GetReportData(id);
                List<RmB15History> res = _repo.GetHistoryData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);

                    using (var book = new XLWorkbook(cachefile))
                    {
                        if (worksheet != null)
                        {
                            var rptCount = _rpt.Count;
                            var rpt = _rpt[rptCount - 1];
                            worksheet.Cell(6, 14).Value = rpt.RevisionNo;
                            worksheet.Cell(6, 17).Value = rpt.RevisionDate;

                            for (int i = 0; i < res.Count; i++)
                            {
                                worksheet.Cell((i + 11), 5).Value = res[i].B15hhJan;
                                worksheet.Cell((i + 11), 6).Value = res[i].B15hhFeb;
                                worksheet.Cell((i + 11), 7).Value = res[i].B15hhMar;
                                worksheet.Cell((i + 11), 8).Value = res[i].B15hhApr;
                                worksheet.Cell((i + 11), 9).Value = res[i].B15hhMay;
                                worksheet.Cell((i + 11), 10).Value = res[i].B15hhJun;
                                worksheet.Cell((i + 11), 11).Value = res[i].B15hhJul;
                                worksheet.Cell((i + 11), 12).Value = res[i].B15hhAug;
                                worksheet.Cell((i + 11), 13).Value = res[i].B15hhSep;
                                worksheet.Cell((i + 11), 14).Value = res[i].B15hhOct;
                                worksheet.Cell((i + 11), 15).Value = res[i].B15hhNov;
                                worksheet.Cell((i + 11), 16).Value = res[i].B15hhDec;
                                worksheet.Cell((i + 11), 17).Value = res[i].B15hhSubTotal;
                                worksheet.Cell((i + 11), 18).Value = res[i].B15hhRemarks;
                            }

                            worksheet.Cell(54, 4).Value = rpt.UserNameProsd;
                            worksheet.Cell(54, 5).Value = rpt.UserNameFclitd;
                            worksheet.Cell(54, 9).Value = rpt.UserNameAgrd;
                            worksheet.Cell(54, 14).Value = rpt.UserNameEndosd;
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

        public List<FormB15Rpt> GetReportData(int headerid)
        {
            return _repo.GetReportData(headerid);
        }

        public async Task<List<FormB15HistoryDTO>> GetHistoryData(int id)
        {
            List<RmB15History> res = _repo.GetHistoryData(id);
            List<FormB15HistoryDTO> FormB7 = new List<FormB15HistoryDTO>();
            FormB7 = _mapper.Map<List<FormB15HistoryDTO>>(res);
            return FormB7;
        }

        public async Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetData(string RmuCode, int year)
        {
            List<RmB13ProposedPlannedBudgetHistory> res = _repo.GetPlannedBudgetData(RmuCode, year);
            List<FormB13HistoryResponseDTO> FormB13 = new List<FormB13HistoryResponseDTO>();
            FormB13 = _mapper.Map<List<FormB13HistoryResponseDTO>>(res);
            return FormB13;
        }
    }
}
