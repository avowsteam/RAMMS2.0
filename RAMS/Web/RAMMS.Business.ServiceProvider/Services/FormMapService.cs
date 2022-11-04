using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
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
    public class FormMapService : IFormMapService
    {
        private readonly IFormMapRepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;

        public FormMapService(IRepositoryUnit repoUnit, IFormMapRepository repo,
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

        public async Task<FormMapHeaderDTO> GetHeaderById(int id, bool view)
        {
            RmMapHeader res = _repo.GetHeaderById(id, view);
            FormMapHeaderDTO FormMap = new FormMapHeaderDTO();
            FormMap = _mapper.Map<FormMapHeaderDTO>(res);
            FormMap.RmMapDetails = _mapper.Map<List<FormMapDetailsDTO>>(res.RmMapDetails);
            return FormMap;
        }

        public int Delete(int id)
        {
            ///if (id > 0 && !_repo.isF1Exist(id))
            if (id > 0)
            {
                id = _repo.DeleteHeader(new RmMapHeader() { RmmhActiveYn = false, RmmhPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<FormMapHeaderDTO> FindDetails(FormMapHeaderDTO frmMap, int createdBy)
        {
            RmMapHeader header = _mapper.Map<RmMapHeader>(frmMap);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmMap = _mapper.Map<FormMapHeaderDTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmMap.Status = "Initialize";

                //frmR1R2.InspectedBy = _security.UserID;
                //frmR1R2.InspectedName = _security.UserName;
                //frmR1R2.InspectedDt = DateTime.Today;
                frmMap.CrBy = frmMap.ModBy = createdBy;
                frmMap.CrDt = frmMap.ModDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                if (frmMap.RmuCode.ToUpper() == "MRI")
                    lstData.Add("RMU", "Miri");
                else if (frmMap.RmuCode.ToUpper() == "BTN")
                    lstData.Add("RMU", "Batu Niah");
                else
                    lstData.Add("RMU", frmMap.RmuCode.ToString());
                lstData.Add("YYYY", frmMap.Year.ToString());
                frmMap.RefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormMap, lstData);

                header = _mapper.Map<RmMapHeader>(frmMap);
                header = await _repo.Save(header, false);
                frmMap = _mapper.Map<FormMapHeaderDTO>(header);
            }
            return frmMap;
        }

        public async Task<List<FormDHeaderResponseDTO>> GetForDDetails(string RMU, int Year, int Month)
        {
            List<RmFormDHdr> res = _repo.GetForDDetails(RMU, Year, Month);
            List<FormDHeaderResponseDTO> FormD = new List<FormDHeaderResponseDTO>();
            FormD = _mapper.Map<List<FormDHeaderResponseDTO>>(res);
            return FormD;
        }

        public async Task<List<FormMapDetailsDTO>> GetForMapDetails(int ID)
        {
            List<RmMapDetails> res = _repo.GetForMapDetails(ID);
            List<FormMapDetailsDTO> FormD = new List<FormMapDetailsDTO>();
            FormD = _mapper.Map<List<FormMapDetailsDTO>>(res);
            return FormD;
        }

        public async Task<FormMapHeaderDTO> SaveMap(FormMapHeaderDTO frmmaphdr, List<FormMapDetailsDTO> frmmap, bool updateSubmit)
        {
            RmMapHeader frmb14hdr_1 = this._mapper.Map<RmMapHeader>((object)frmmaphdr);
            frmb14hdr_1 = UpdateStatus(frmb14hdr_1);

            RmMapHeader source = await this._repo.Save(frmb14hdr_1, updateSubmit);

            var domainModelFormB14 = _mapper.Map<List<RmMapDetails>>(frmmap);
            foreach (var list in domainModelFormB14)
            {
                list.RmmdPkRefNoDetails = list.RmmdPkRefNoDetails;
                list.RmmdRmmhPkRefNo = frmmaphdr.PkRefNo;
            }
            await _repo.SaveFormB14(domainModelFormB14);


            frmmaphdr = this._mapper.Map<FormMapHeaderDTO>((object)source);
            return frmmaphdr;
        }

        public RmMapHeader UpdateStatus(RmMapHeader form)
        {
            if (form.RmmhPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmMapHeader.Where(x => x.RmmhPkRefNo == form.RmmhPkRefNo).Select(x => new { Status = x.RmmhStatus, Log = x.RmmhAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.RmmhAuditlog = existsObj.Log;
                    form.RmmhStatus = existsObj.Status;
                }

            }


            if (form.RmmhSubmitSts == true && (string.IsNullOrEmpty(form.RmmhStatus) || form.RmmhStatus == Common.StatusList.FormQA1Saved || form.RmmhStatus == Common.StatusList.FormQA1Rejected))
            {
                //form.Fg1hInspectedBy = _security.UserID;
                //form.Fg1hInspectedName = _security.UserName;
                //form.Fg1hInspectedDt = DateTime.Today;
                form.RmmhStatus = Common.StatusList.FormQA1Submitted;
                form.RmmhAuditlog = Utility.ProcessLog(form.RmmhAuditlog, "Submitted", "Submitted", form.RmmhPreparedName, string.Empty, form.RmmhPreparedDate, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Executed By:" + form.RmmhPreparedName + " - Form M (" + form.RmmhPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormMap/Edit/" + form.RmmhPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            else if (string.IsNullOrEmpty(form.RmmhStatus) || form.RmmhStatus == "Initialize")
                form.RmmhStatus = Common.StatusList.FormR1R2Saved;

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
                List<FormMapRpt> _rpt = this.GetReportData(id);
                List<RmMapDetails> res = _repo.GetHistoryData(id);
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

                            DateTime dt = new DateTime(Convert.ToInt32(rpt.Year), Convert.ToInt32(rpt.Month), 1);
                            string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dt.Month);
                            int dayCount = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dt.Year, dt.Month);

                            worksheet.Cell(2, 1).Value = "Monthly Activities Progress For The Month of (" + MonthName + ") (" + rpt.Year + ")";


                            List<int> lstActivityCode = new List<int>();
                            lstActivityCode = res.Select(x => Convert.ToInt32(x.RmmdActivityId)).Distinct().ToList();
                            lstActivityCode.Sort();

                            for (int i = 0; i < lstActivityCode.Count; i++)
                            {
                                List<RmMapDetails> lstDetail = res.Where(x => x.RmmdActivityId == lstActivityCode[i]).OrderBy(x => x.RmmdOrder).ToList();
                                for (int j = 1; j <= dayCount; j++)
                                {
                                    if (i == 0)
                                    {
                                        DateTime dtSeq = new DateTime(Convert.ToInt32(rpt.Year), Convert.ToInt32(rpt.Month), j);
                                        int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dtSeq, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                                        worksheet.Cell(4, (j + 6)).Value = "WK" + weekNumber;
                                        worksheet.Cell(4, (j + 6)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(4, (j + 6)).Style.Border.OutsideBorderColor = XLColor.Black;


                                        worksheet.Cell(5, (j + 6)).Value = dtSeq.DayOfWeek.ToString().Substring(0, 3);
                                        worksheet.Cell(5, (j + 6)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(5, (j + 6)).Style.Border.OutsideBorderColor = XLColor.Black;

                                        worksheet.Cell(6, (j + 6)).Value = j;
                                        worksheet.Cell(6, (j + 6)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(6, (j + 6)).Style.Border.OutsideBorderColor = XLColor.Black;
                                        worksheet.Cell(6, (6 + dayCount + 1)).Value = "Total";
                                        worksheet.Cell(6, (6 + dayCount + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(6, (6 + dayCount + 1)).Style.Border.OutsideBorderColor = XLColor.Black;

                                        worksheet.Cell(7, (j + 6)).Value = lstDetail[j - 1].RmmdActivityLocationCode;
                                        worksheet.Cell(7, (j + 6)).Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
                                        worksheet.Cell(7, (j + 6)).Style.Border.BottomBorderColor = XLColor.Gray;
                                        worksheet.Cell(7, (j + 6)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(7, (j + 6)).Style.Border.RightBorderColor = XLColor.Black;

                                        worksheet.Cell((7 + 1), (j + 6)).Value = lstDetail[j - 1].RmmdQuantityKm;
                                        worksheet.Cell((7 + 1), 2).Value = lstDetail[j - 1].RmmdProductUnit;
                                        worksheet.Cell((7 + 1), (j + 6)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((7 + 1), (j + 6)).Style.Border.BottomBorderColor = XLColor.Black;
                                        worksheet.Cell((7 + 1), (j + 6)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((7 + 1), (j + 6)).Style.Border.RightBorderColor = XLColor.Black;

                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Value = lstDetail.Select(x => x.RmmdQuantityKm).Sum().ToString();
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.RightBorderColor = XLColor.Black;
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.TopBorderColor = XLColor.Black;
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((7 + 1), (6 + dayCount + 1)).Style.Border.BottomBorderColor = XLColor.Black;

                                        worksheet.Cell(((7 + 1) - 1), (6 + dayCount + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(((7 + 1) - 1), (6 + dayCount + 1)).Style.Border.RightBorderColor = XLColor.Black;
                                        
                                    }
                                    else
                                    {
                                        worksheet.Cell((i + 7 + i), (j + 6)).Value = lstDetail[j - 1].RmmdActivityLocationCode;

                                        worksheet.Cell((i + 7 + i), (j + 6)).Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
                                        worksheet.Cell((i + 7 + i), (j + 6)).Style.Border.BottomBorderColor = XLColor.Gray;
                                        worksheet.Cell((i + 7 + i), (j + 6)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 7 + i), (j + 6)).Style.Border.RightBorderColor = XLColor.Black;

                                        worksheet.Cell((i + 8 + i), (j + 6)).Value = lstDetail[j - 1].RmmdQuantityKm;
                                        worksheet.Cell((i + 8 + i), 2).Value = lstDetail[j - 1].RmmdProductUnit;
                                        worksheet.Cell((i + 8 + i), (j + 6)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 8 + i), (j + 6)).Style.Border.BottomBorderColor = XLColor.Black;
                                        worksheet.Cell((i + 8 + i), (j + 6)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 8 + i), (j + 6)).Style.Border.RightBorderColor = XLColor.Black;

                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Value = lstDetail.Select(x => x.RmmdQuantityKm).Sum().ToString();

                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.RightBorderColor = XLColor.Black;
                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.BottomBorderColor = XLColor.Black;
                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell((i + 8 + i), (6 + dayCount + 1)).Style.Border.TopBorderColor = XLColor.Black;

                                        worksheet.Cell(((i + 8 + i) - 1), (6 + dayCount + 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                        worksheet.Cell(((i + 8 + i) - 1), (6 + dayCount + 1)).Style.Border.RightBorderColor = XLColor.Black;
                                    }

                                }

                                //worksheet.Cell((i + 9), 5).Value = res[i].B14hhJan;
                                //worksheet.Cell((i + 9), 6).Value = res[i].B14hhFeb;
                                //worksheet.Cell((i + 9), 7).Value = res[i].B14hhMar;
                                //worksheet.Cell((i + 9), 8).Value = res[i].B14hhApr;
                                //worksheet.Cell((i + 9), 9).Value = res[i].B14hhMay;
                                //worksheet.Cell((i + 9), 10).Value = res[i].B14hhJun;
                                //worksheet.Cell((i + 9), 11).Value = res[i].B14hhJul;
                                //worksheet.Cell((i + 9), 12).Value = res[i].B14hhAug;
                                //worksheet.Cell((i + 9), 13).Value = res[i].B14hhSep;
                                //worksheet.Cell((i + 9), 14).Value = res[i].B14hhOct;
                                //worksheet.Cell((i + 9), 15).Value = res[i].B14hhNov;
                                //worksheet.Cell((i + 9), 16).Value = res[i].B14hhDec;
                                //worksheet.Cell((i + 9), 17).Value = res[i].B14hhSubTotal;
                                //worksheet.Cell((i + 9), 18).Value = res[i].B14hhUnitOfService;
                            }

                            if (rpt.PreparedName != null)
                            {
                                worksheet.Cell(87, 4).Value = rpt.PreparedName;
                                worksheet.Cell(89, 4).Value = rpt.PreparedDesig;
                                worksheet.Cell(91, 4).Value = rpt.PreparedDate;
                            }
                            if (rpt.CheckedName != null)
                            {
                                worksheet.Cell(87, 14).Value = rpt.CheckedName;
                                worksheet.Cell(89, 14).Value = rpt.CheckedDesig;
                                worksheet.Cell(91, 14).Value = rpt.CheckedDate;
                            }
                            if (rpt.VerifiedName != null)
                            {
                                worksheet.Cell(87, 23).Value = rpt.VerifiedName;
                                worksheet.Cell(89, 23).Value = rpt.VerifiedDesig;
                                worksheet.Cell(91, 23).Value = rpt.VerifiedDate;
                            }
                            //if (rpt.UserNameEndosd != null)
                            //{
                            //    worksheet.Cell(51, 14).Value = rpt.UserNameEndosd;
                            //    worksheet.Cell(52, 14).Value = rpt.UserDesignationEndosd;
                            //}
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

        public List<FormMapRpt> GetReportData(int headerid)
        {
            return _repo.GetReportData(headerid);
        }

    }
}
