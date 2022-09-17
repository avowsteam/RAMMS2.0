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
    public class FormB14Service : IFormB14Service
    {
        private readonly IFormB14Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;
        public FormB14Service(IRepositoryUnit repoUnit, IFormB14Repository repo,
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

        public async Task<FormB14HeaderDTO> GetHeaderById(int id, bool view)
        {
            RmB14Hdr res = _repo.GetHeaderById(id, view);
            FormB14HeaderDTO FormB14 = new FormB14HeaderDTO();
            FormB14 = _mapper.Map<FormB14HeaderDTO>(res);
            FormB14.RmB14History = _mapper.Map<List<FormB14HistoryDTO>>(res.RmB14History);
            return FormB14;
        }

        public int Delete(int id)
        {
            if (id > 0 && !_repo.isF1Exist(id))
            {
                id = _repo.DeleteHeader(new RmB14Hdr() { B14hActiveYn = false, B14hPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<FormB14HeaderDTO> FindDetails(FormB14HeaderDTO frmB14, int createdBy)
        {
            RmB14Hdr header = _mapper.Map<RmB14Hdr>(frmB14);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmB14 = _mapper.Map<FormB14HeaderDTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmB14.Status = "Initialize";

                //frmR1R2.InspectedBy = _security.UserID;
                //frmR1R2.InspectedName = _security.UserName;
                //frmR1R2.InspectedDt = DateTime.Today;
                frmB14.CrBy = frmB14.ModBy = createdBy;
                frmB14.CrDt = frmB14.ModDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", frmB14.RevisionYear.ToString());
                lstData.Add("RevisionNo", frmB14.RevisionNo.ToString());
                frmB14.PkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormB14, lstData);

                header = _mapper.Map<RmB14Hdr>(frmB14);
                header = await _repo.Save(header, false);
                frmB14 = _mapper.Map<FormB14HeaderDTO>(header);
            }
            return frmB14;
        }

        public int? GetMaxRev(int Year, string RmuCode)
        {
            return _repo.GetMaxRev(Year, RmuCode);
        }

        public async Task<int> SaveFormB14(List<FormB14HistoryDTO> FormB14)
        {
            try
            {
                var domainModelFormB14 = _mapper.Map<List<RmB14History>>(FormB14);
                foreach (var list in domainModelFormB14)
                {
                    list.B14hhPkRefNoHistory = 0;
                }

                return await _repo.SaveFormB14(domainModelFormB14);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<FormB14HeaderDTO> SaveB14(FormB14HeaderDTO frmb14hdr, List<FormB14HistoryDTO> frmb14, bool updateSubmit)
        {
            RmB14Hdr frmb14hdr_1 = this._mapper.Map<RmB14Hdr>((object)frmb14hdr);
            frmb14hdr_1 = UpdateStatus(frmb14hdr_1);

            RmB14Hdr source = await this._repo.Save(frmb14hdr_1, updateSubmit);

            var domainModelFormB14 = _mapper.Map<List<RmB14History>>(frmb14);
            foreach (var list in domainModelFormB14)
            {
                list.B14hhPkRefNoHistory = list.B14hhPkRefNoHistory;
                list.B14hhB14hPkRefNo = frmb14hdr.PkRefNo;
            }
            await _repo.SaveFormB14(domainModelFormB14);


            frmb14hdr = this._mapper.Map<FormB14HeaderDTO>((object)source);
            return frmb14hdr;
        }

        public RmB14Hdr UpdateStatus(RmB14Hdr form)
        {
            if (form.B14hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmB14Hdr.Where(x => x.B14hPkRefNo == form.B14hPkRefNo).Select(x => new { Status = x.B14hStatus, Log = x.B14hAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.B14hAuditlog = existsObj.Log;
                    form.B14hStatus = existsObj.Status;
                }

            }


            if (form.B14hSubmitSts && (string.IsNullOrEmpty(form.B14hStatus) || form.B14hStatus == Common.StatusList.FormQA1Saved || form.B14hStatus == Common.StatusList.FormQA1Rejected))
            {
                //form.Fg1hInspectedBy = _security.UserID;
                //form.Fg1hInspectedName = _security.UserName;
                //form.Fg1hInspectedDt = DateTime.Today;
                form.B14hStatus = Common.StatusList.FormQA1Submitted;
                form.B14hAuditlog = Utility.ProcessLog(form.B14hAuditlog, "Submitted", "Submitted", form.B14hUserNameProsd, string.Empty, form.B14hDtProsd, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Executed By:" + form.B14hUserNameProsd + " - Form M (" + form.B14hPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormB14/Edit/" + form.B14hPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            else if (string.IsNullOrEmpty(form.B14hStatus) || form.B14hStatus == "Initialize")
                form.B14hStatus = Common.StatusList.FormR1R2Saved;

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
                List<FormB14Rpt> _rpt = this.GetReportData(id);
                List<RmB14History> res = _repo.GetHistoryData(id);
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
                            worksheet.Cell(5, 14).Value = rpt.RevisionNo;
                            worksheet.Cell(5, 17).Value = rpt.RevisionDate;

                            for (int i = 0; i < res.Count; i++)
                            {
                                worksheet.Cell((i+9), 5).Value = res[i].B14hhJan;
                                worksheet.Cell((i+9), 6).Value = res[i].B14hhFeb;
                                worksheet.Cell((i+9), 7).Value = res[i].B14hhMar;
                                worksheet.Cell((i+9), 8).Value = res[i].B14hhApr;
                                worksheet.Cell((i+9), 9).Value = res[i].B14hhMay;
                                worksheet.Cell((i+9), 10).Value = res[i].B14hhJun;
                                worksheet.Cell((i+9), 11).Value = res[i].B14hhJul;
                                worksheet.Cell((i+9), 12).Value = res[i].B14hhAug;
                                worksheet.Cell((i+9), 13).Value = res[i].B14hhSep;
                                worksheet.Cell((i+9), 14).Value = res[i].B14hhOct;
                                worksheet.Cell((i+9), 15).Value = res[i].B14hhNov;
                                worksheet.Cell((i+9), 16).Value = res[i].B14hhDec;
                                worksheet.Cell((i + 9), 17).Value = res[i].B14hhSubTotal;
                                worksheet.Cell((i + 9), 18).Value = res[i].B14hhUnitOfService;
                            }

                            worksheet.Cell(51, 4).Value = rpt.UserNameProsd;
                            worksheet.Cell(51, 5).Value = rpt.UserNameFclitd;
                            worksheet.Cell(51, 9).Value = rpt.UserNameAgrd;
                            worksheet.Cell(51, 14).Value = rpt.UserNameEndosd;
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

        public List<FormB14Rpt> GetReportData(int headerid)
        {
            return _repo.GetReportData(headerid);
        }

        public async Task<List<FormB14HistoryDTO>> GetHistoryData(int id)
        {
            List<RmB14History> res = _repo.GetHistoryData(id);
            List<FormB14HistoryDTO> FormB7 = new List<FormB14HistoryDTO>();
            FormB7 = _mapper.Map<List<FormB14HistoryDTO>>(res);
            return FormB7;
        }

        public async Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetData(string RmuCode, int year)
        {
            List<RmB13ProposedPlannedBudgetHistory> res = _repo.GetPlannedBudgetData(RmuCode, year);
            List<FormB13HistoryResponseDTO> FormB13 = new List<FormB13HistoryResponseDTO>();
            FormB13 = _mapper.Map<List<FormB13HistoryResponseDTO>>(res);
            return FormB13;
        }

        public async Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData)
        {
            return await _repo.GetAWPBHeaderGrid(searchData);
        }

        public Task<int> SaveFormB14(FormB14HeaderDTO FormB14)
        {
            throw new NotImplementedException();
        }
    }
}
