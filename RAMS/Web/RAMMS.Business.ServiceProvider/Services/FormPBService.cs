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

    public class FormPBService : IFormPBService
    {
        private readonly IFormPBRepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormPBService(IRepositoryUnit repoUnit, IFormPBRepository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormPBHeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormPBSearchGridDTO> filterOptions)
        {
            PagingResult<FormPBHeaderResponseDTO> result = new PagingResult<FormPBHeaderResponseDTO>();
            List<FormPBHeaderResponseDTO> formAlist = new List<FormPBHeaderResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }


        public async Task<FormPBHeaderResponseDTO> GetHeaderById(int id)
        {
            RmPbIw res = _repo.GetHeaderById(id);
            FormPBHeaderResponseDTO PB = new FormPBHeaderResponseDTO();
            PB = _mapper.Map<FormPBHeaderResponseDTO>(res);
            PB.FormPBDetails= _mapper.Map<List<FormPBDetailResponseDTO>>(res.RmPbIwDetails);
           
            return PB;
        }

       


        public async Task<int> SaveFormPB(FormPBHeaderResponseDTO FormPB)
        {
            try
            {
                var domainModelFormPB = _mapper.Map<RmPbIw>(FormPB);
                domainModelFormPB.PbiwPkRefNo = 0;
                var res = _repo.SaveFormPB(domainModelFormPB);

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormPB.PbiwSubmissionYear.ToString());
                lstData.Add("MM", domainModelFormPB.PbiwSubmissionMonth < 10 ? "0" + domainModelFormPB.PbiwSubmissionMonth.ToString() : domainModelFormPB.PbiwSubmissionMonth.ToString());
                lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(res.Result));
                domainModelFormPB.PbiwRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormPB, lstData);
                var result = _repo.SaveFormPB(domainModelFormPB,true);

                return res.Result;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> UpdateFormPB(FormPBHeaderResponseDTO FormPBHeader, List<FormPBDetailResponseDTO> FormPBDetails)
        {
            try
            {
                int PkRefNo = FormPBHeader.PkRefNo;
                var domainModelFormPB = _mapper.Map<RmPbIw>(FormPBHeader);
                domainModelFormPB.PbiwPkRefNo = PkRefNo;
                domainModelFormPB.PbiwActiveYn = true;
                domainModelFormPB = UpdateStatus(domainModelFormPB);
               _repoUnit.FormPBRepository.Update(domainModelFormPB);
                await _repoUnit.CommitAsync();

                var domainModelFormPBDetails = _mapper.Map<List<RmPbIwDetails>>(FormPBDetails);
                return await _repo.UpdateFormPB(domainModelFormPB, domainModelFormPBDetails);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public RmPbIw UpdateStatus(RmPbIw form)
        {
            if (form.PbiwPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormF3Repository._context.RmPbIw.Where(x => x.PbiwPkRefNo == form.PbiwPkRefNo).Select(x => new { Status = x.PbiwStatus, Log = x.PbiwAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.PbiwAuditLog = existsObj.Log;
                    form.PbiwStatus = existsObj.Status;

                }

            }
            if (form.PbiwSubmitSts && (form.PbiwStatus == "Saved" || form.PbiwStatus == "Initialize"))
            {
                form.PbiwStatus = Common.StatusList.Submitted;
                form.PbiwAuditLog = Utility.ProcessLog(form.PbiwAuditLog, "Submitted By", "Submitted", form.PbiwUsernameSo, string.Empty, form.PbiwSignDateSo, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "proposed By:" + form.PbiwUsernameSo + " - Form PB (" + form.PbiwPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormPB/add?id=" + form.PbiwPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }


        public int? DeleteFormPB(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormPB(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        ////public async Task<FORMPBRpt> GetReportData(int headerid)
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
                FormPBHeaderResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormPBDetails;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    for (int sheet = 1; sheet <= 1; sheet++)
                    {
                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {

                            worksheet.Cell(3, 7).Value = rptcol.SubmissionMonth;
                            worksheet.Cell(3, 19).Value = rptcol.SubmissionYear;

                            int i = 8;

                            foreach (var r in rpt)
                            {

                                worksheet.Cell(i, 1).Value = r.IwRef;
                                worksheet.Cell(i, 6).Value = r.ProjectTitle;
                                worksheet.Cell(i, 12).Value = r.CompletionDate;
                                worksheet.Cell(i, 15).Value = r.CompletionRefNo;
                                worksheet.Cell(i, 19).Value = r.AmountBeforeLad;
                                worksheet.Cell(i, 24).Value = r.AmountBeforeLad;
                                worksheet.Cell(i, 29).Value = r.FinalPayment;
                                i++;
                            }

                            worksheet.Cell(21, 4).Value = rptcol.UsernameSp;
                            worksheet.Cell(22, 4).Value = rptcol.DesignationSp;
                            worksheet.Cell(23, 4).Value = rptcol.SignDateSp;

                            worksheet.Cell(21, 14).Value = rptcol.UsernameEc;
                            worksheet.Cell(22, 14).Value = rptcol.DesignationEc;
                            worksheet.Cell(23, 14).Value = rptcol.SignDateEc;

                            worksheet.Cell(21, 24).Value = rptcol.UsernameSo;
                            worksheet.Cell(22, 24).Value = rptcol.DesignationSo;
                            worksheet.Cell(23, 24).Value = rptcol.SignDateSo;
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
