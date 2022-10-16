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
    public class FormT3Service : IFormT3Service
    {
        private readonly IFormT3Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;

        public FormT3Service(IRepositoryUnit repoUnit, IFormT3Repository repo,
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

        public async Task<FormT3HeaderDTO> GetHeaderById(int id, bool view)
        {
            RmT3Hdr res = _repo.GetHeaderById(id, view);
            FormT3HeaderDTO FormT3 = new FormT3HeaderDTO();
            FormT3 = _mapper.Map<FormT3HeaderDTO>(res);
            FormT3.RmT3History = _mapper.Map<List<FormT3HistoryDTO>>(res.RmT3History);
            return FormT3;
        }

        public int Delete(int id)
        {
            ///if (id > 0 && !_repo.isF1Exist(id))
            if (id > 0)
            {
                id = _repo.DeleteHeader(new RmT3Hdr() { T3hActiveYn = false, T3hPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<FormT3HeaderDTO> FindDetails(FormT3HeaderDTO frmT3, int createdBy)
        {
            RmT3Hdr header = _mapper.Map<RmT3Hdr>(frmT3);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmT3 = _mapper.Map<FormT3HeaderDTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmT3.Status = "Initialize";

                //frmR1R2.InspectedBy = _security.UserID;
                //frmR1R2.InspectedName = _security.UserName;
                //frmR1R2.InspectedDt = DateTime.Today;
                frmT3.CrBy = frmT3.ModBy = createdBy;
                frmT3.CrDt = frmT3.ModDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();

                lstData.Add("RMU", frmT3.RmuCode.ToString());
                lstData.Add("YYYY", frmT3.RevisionYear.ToString());
                lstData.Add("RevisionNo", frmT3.RevisionNo.ToString());
                frmT3.PkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormT3, lstData);

                header = _mapper.Map<RmT3Hdr>(frmT3);
                header = await _repo.Save(header, false);
                frmT3 = _mapper.Map<FormT3HeaderDTO>(header);
            }
            return frmT3;
        }

        public int? GetMaxRev(int Year, string RmuCode)
        {
            return _repo.GetMaxRev(Year, RmuCode);
        }

        public async Task<int> SaveFormT3(List<FormT3HistoryDTO> FormT3)
        {
            try
            {
                var domainModelFormT3 = _mapper.Map<List<RmT3History>>(FormT3);
                foreach (var list in domainModelFormT3)
                {
                    list.T3hhPkRefNoHistory = 0;
                }

                return await _repo.SaveFormT3(domainModelFormT3);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<FormT3HeaderDTO> SaveT3(FormT3HeaderDTO frmT3hdr, List<FormT3HistoryDTO> frmT3, bool updateSubmit)
        {
            RmT3Hdr frmT3hdr_1 = this._mapper.Map<RmT3Hdr>((object)frmT3hdr);
            frmT3hdr_1 = UpdateStatus(frmT3hdr_1);

            RmT3Hdr source = await this._repo.Save(frmT3hdr_1, updateSubmit);

            var domainModelFormT3 = _mapper.Map<List<RmT3History>>(frmT3);
            foreach (var list in domainModelFormT3)
            {
                list.T3hhPkRefNoHistory = list.T3hhPkRefNoHistory;
                list.T3hhT3hPkRefNo = frmT3hdr.PkRefNo;
            }
            await _repo.SaveFormT3(domainModelFormT3);


            frmT3hdr = this._mapper.Map<FormT3HeaderDTO>((object)source);
            return frmT3hdr;
        }

        public RmT3Hdr UpdateStatus(RmT3Hdr form)
        {
            if (form.T3hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmT3Hdr.Where(x => x.T3hPkRefNo == form.T3hPkRefNo).Select(x => new { Status = x.T3hStatus, Log = x.T3hAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.T3hAuditlog = existsObj.Log;
                    //form.T3hStatus = existsObj.Status;
                }

            }


            if (form.T3hSubmitSts && (string.IsNullOrEmpty(form.T3hStatus) || form.T3hStatus == Common.StatusList.FormQA1Saved || form.T3hStatus == Common.StatusList.FormQA1Rejected))
            {
                //form.T3hStatus = Common.StatusList.FormQA1Submitted;
                int? revNo = _repo.GetB14RevisionNo(form.T3hRmuCode, form.T3hRevisionYear);
                string Comments = form.T3hRevisionYear + "+" + revNo;
                form.T3hAuditlog = Utility.ProcessLog(form.T3hAuditlog, "Submitted", "Submitted", string.Empty, Comments, null, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Executed By:" + _security.UserName + " - Form T3 (" + form.T3hPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormT3/Edit/" + form.T3hPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            //else if (string.IsNullOrEmpty(form.T3hStatus) || form.T3hStatus == "Initialize")
            //    form.T3hStatus = Common.StatusList.FormR1R2Saved;

            return form;
        }

        public byte[] FormDownload(string formname, int id, string basepath, string filepath)
        {
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
                List<FormT3Rpt> _rpt = this.GetReportData(id);
                List<RmT3History> res = _repo.GetHistoryData(id);
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
                            worksheet.Cell(2, 1).Value = "ANNUAL WORK PROGRAMME AND BUDGET " + rpt.RevisionYear + " (PLANNED BUDGET)";
                            worksheet.Cell(4, 14).Value = rpt.RevisionNo;
                            worksheet.Cell(4, 17).Value = rpt.RevisionDate;

                            for (int i = 0; i < res.Count; i++)
                            {
                                worksheet.Cell((i + 7), 5).Value = res[i].T3hhJan;
                                worksheet.Cell((i + 7), 6).Value = res[i].T3hhFeb;
                                worksheet.Cell((i + 7), 7).Value = res[i].T3hhMar;
                                worksheet.Cell((i + 7), 8).Value = res[i].T3hhApr;
                                worksheet.Cell((i + 7), 9).Value = res[i].T3hhMay;
                                worksheet.Cell((i + 7), 10).Value = res[i].T3hhJun;
                                worksheet.Cell((i + 7), 11).Value = res[i].T3hhJul;
                                worksheet.Cell((i + 7), 12).Value = res[i].T3hhAug;
                                worksheet.Cell((i + 7), 13).Value = res[i].T3hhSep;
                                worksheet.Cell((i + 7), 14).Value = res[i].T3hhOct;
                                worksheet.Cell((i + 7), 15).Value = res[i].T3hhNov;
                                worksheet.Cell((i + 7), 16).Value = res[i].T3hhDec;
                                worksheet.Cell((i + 7), 17).Value = res[i].T3hhSubTotal;
                                worksheet.Cell((i + 7), 18).Value = res[i].T3hhUnitOfService;
                                worksheet.Cell((i + 7), 19).Value = res[i].T3hhRemarks;
                            }
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

        public List<FormT3Rpt> GetReportData(int headerid)
        {
            return _repo.GetReportData(headerid);
        }

        public async Task<List<FormT3HistoryDTO>> GetHistoryData(int id)
        {
            List<RmT3History> res = _repo.GetHistoryData(id);
            List<FormT3HistoryDTO> FormB7 = new List<FormT3HistoryDTO>();
            FormB7 = _mapper.Map<List<FormT3HistoryDTO>>(res);
            return FormB7;
        }

        public async Task<List<FormB14HistoryDTO>> GetPlannedBudgetData(string RmuCode, int year)
        {
            List<RmB14History> res = _repo.GetPlannedBudgetData(RmuCode, year);
            List<FormB14HistoryDTO> FormB13 = new List<FormB14HistoryDTO>();
            FormB14HistoryDTO clsB13 = new FormB14HistoryDTO();
            for (int i = 0; i < res.Count(); i++)
            {
                clsB13 = new FormB14HistoryDTO();
                clsB13.B14hPkRefNo = res[i].B14hhB14hPkRefNo;
                clsB13.PkRefNoHistory= res[i].B14hhPkRefNoHistory;
                clsB13.Feature = res[i].B14hhFeature;
                clsB13.Jan = (res[i].B14hhJan == null ? 0 : res[i].B14hhJan);
                clsB13.Feb = clsB13.Jan + (res[i].B14hhFeb == null ? 0 : res[i].B14hhFeb);
                clsB13.Mar = clsB13.Feb + (res[i].B14hhMar == null ? 0 : res[i].B14hhMar);
                clsB13.Apr = clsB13.Mar + (res[i].B14hhApr == null ? 0 : res[i].B14hhApr);
                clsB13.May = clsB13.Apr + (res[i].B14hhMay == null ? 0 : res[i].B14hhMay);
                clsB13.Jun = clsB13.May + (res[i].B14hhJun == null ? 0 : res[i].B14hhJun);
                clsB13.Jul = clsB13.Jun + (res[i].B14hhJul == null ? 0 : res[i].B14hhJul);
                clsB13.Aug = clsB13.Jul + (res[i].B14hhAug == null ? 0 : res[i].B14hhAug);
                clsB13.Sep = clsB13.Aug + (res[i].B14hhSep == null ? 0 : res[i].B14hhSep);
                clsB13.Oct = clsB13.Sep + (res[i].B14hhOct == null ? 0 : res[i].B14hhOct);
                clsB13.Nov = clsB13.Oct + (res[i].B14hhNov == null ? 0 : res[i].B14hhNov);
                clsB13.Dec = clsB13.Nov + (res[i].B14hhDec == null ? 0 : res[i].B14hhDec);
                clsB13.SubTotal = clsB13.Jan + clsB13.Feb + clsB13.Mar + clsB13.Apr + clsB13.May + clsB13.Jun + clsB13.Jul + clsB13.Aug + clsB13.Sep + clsB13.Oct + clsB13.Nov + clsB13.Dec;
                FormB13.Add(clsB13);
            }
            //FormB13 = _mapper.Map<List<FormB14HistoryDTO>>(res);
            return FormB13;
        }

        public async Task<List<FormB10HistoryResponseDTO>> GetUnitData(int year)
        {
            List<RmB10DailyProductionHistory> res = _repo.GetUnitData(year);
            List<FormB10HistoryResponseDTO> FormB10 = new List<FormB10HistoryResponseDTO>();
            FormB10 = _mapper.Map<List<FormB10HistoryResponseDTO>>(res);
            return FormB10;
        }

        public Task<int> SaveFormT3(FormT3HeaderDTO FormT3)
        {
            throw new NotImplementedException();
        }
    }
}
