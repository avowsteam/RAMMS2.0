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

    public class FormB13Service : IFormB13Service
    {
        private readonly IFormB13Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormB13Service(IRepositoryUnit repoUnit, IFormB13Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormB13ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB13SearchGridDTO> filterOptions)
        {
            PagingResult<FormB13ResponseDTO> result = new PagingResult<FormB13ResponseDTO>();
            List<FormB13ResponseDTO> formAlist = new List<FormB13ResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }


        public async Task<FormB13ResponseDTO> GetHeaderById(int id)
        {
            RmB13ProposedPlannedBudget res = _repo.GetHeaderById(id);
            FormB13ResponseDTO B13 = new FormB13ResponseDTO();
            B13 = _mapper.Map<FormB13ResponseDTO>(res);
            B13.FormB13History = _mapper.Map<List<FormB13HistoryResponseDTO>>(res.RmB13ProposedPlannedBudgetHistory);
            B13.FormB13RevisionHistory = _mapper.Map<List<FormB13HistoryRevisionResponseDTO>>(res.RmB13RevisionHistory);
            return B13;
        }

        public int? GetMaxRev(int Year, string RMU)
        {
            return _repo.GetMaxRev(Year, RMU);
        }


        public async Task<FormB13ResponseDTO> SaveFormB13(FormB13ResponseDTO FormB13)
        {
            try
            {
                var domainModelFormB13 = _mapper.Map<RmB13ProposedPlannedBudget>(FormB13);
                domainModelFormB13.B13pPkRefNo = 0;
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormB13.B13pRevisionYear.ToString());
                lstData.Add("RevisionNo", domainModelFormB13.B13pRevisionNo.ToString());
                domainModelFormB13.B13pPkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormB13, lstData);
                var res = _repo.SaveFormB13(domainModelFormB13);

                FormB13.PkRefNo = res.Result.B13pPkRefNo;
                return FormB13;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> UpdateFormB13(FormB13ResponseDTO FormB13, List<FormB13HistoryResponseDTO> FormB13History)
        {
            try
            {
                int PkRefNo = FormB13.PkRefNo;
                var domainModelFormB13 = _mapper.Map<RmB13ProposedPlannedBudget>(FormB13);
                domainModelFormB13.B13pPkRefNo = PkRefNo;
                domainModelFormB13.B13pActiveYn = true;
                domainModelFormB13 = UpdateStatus(domainModelFormB13);
                _repoUnit.FormB13Repository.Update(domainModelFormB13);
                await _repoUnit.CommitAsync();

                var domainModelFormB13History = _mapper.Map<List<RmB13ProposedPlannedBudgetHistory>>(FormB13History);
                return await _repo.UpdateFormB13(domainModelFormB13, domainModelFormB13History);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public RmB13ProposedPlannedBudget UpdateStatus(RmB13ProposedPlannedBudget form)
        {
            if (form.B13pPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormF3Repository._context.RmB13ProposedPlannedBudget.Where(x => x.B13pPkRefNo == form.B13pPkRefNo).Select(x => new { Status = x.B13pStatus, Log = x.B13pAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.B13pAuditLog = existsObj.Log;
                    form.B13pStatus = existsObj.Status;

                }

            }
            if (form.B13pSubmitSts && (form.B13pStatus == "Saved" || form.B13pStatus == "Initialize"))
            {
                form.B13pStatus = Common.StatusList.Submitted;
                form.B13pAuditLog = Utility.ProcessLog(form.B13pAuditLog, "Submitted By", "Submitted", form.B13pUserNameProsd, string.Empty, form.B13pRevisionDate, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "proposed By:" + form.B13pUserNameProsd + " - Form B13 (" + form.B13pPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormB13/add?id=" + form.B13pPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }


        public int? DeleteFormB13(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormB13(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        ////public async Task<FORMB13Rpt> GetReportData(int headerid)
        ////{
        ////    return await _repo.GetReportData(headerid);
        ////}

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
                FormB13ResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormB13History;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    for (int sheet = 1; sheet <= 1; sheet++)
                    {
                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {
                            int i = 10;

                            foreach (var r in rpt)
                            {

                                worksheet.Cell(i, 5).Value = r.InvCond1;
                                worksheet.Cell(i, 6).Value = r.InvCond2;
                                worksheet.Cell(i, 7).Value = r.InvCond3;
                                worksheet.Cell(i, 9).Value = r.SlCond1;
                                worksheet.Cell(i, 10).Value = r.SlCond2;
                                worksheet.Cell(i, 11).Value = r.SlCond3;
                                worksheet.Cell(i, 17).Value = r.CdcLabour;
                                worksheet.Cell(i, 18).Value = r.CdcEquipment;
                                worksheet.Cell(i, 19).Value = r.CdcMaterial;
                                worksheet.Cell(i, 21).Value = r.AverageDailyProduction;
                                worksheet.Cell(i, 22).Value = r.UnitOfService;

                                i++;

                            }

                            int j = 54;
                            var rev = rptcol.FormB13RevisionHistory;
                            foreach (var r in rev)
                            {
                                worksheet.Cell(j, 1).Value = r.Date;
                                worksheet.Cell(j, 2).Value = r.Description;
                                worksheet.Cell(j, 6).Value = r.RevNo;
                                j++;
                            }

                            worksheet.Cell(55, 15).Value = rptcol.UserNameProsd;
                            worksheet.Cell(55, 19).Value = rptcol.UserNameFclitd;
                            worksheet.Cell(55, 24).Value = rptcol.UserNameAgrd;
                            worksheet.Cell(55, 28).Value = rptcol.UserNameEdosd;
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

    }
}
