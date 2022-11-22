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

    public class FormP1Service : IFormP1Service
    {
        private readonly IFormP1Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormP1Service(IRepositoryUnit repoUnit, IFormP1Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormP1HeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormP1SearchGridDTO> filterOptions)
        {
            PagingResult<FormP1HeaderResponseDTO> result = new PagingResult<FormP1HeaderResponseDTO>();
            List<FormP1HeaderResponseDTO> formAlist = new List<FormP1HeaderResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }


        public async Task<FormP1HeaderResponseDTO> GetHeaderById(int id)
        {
            RmPaymentCertificateHeader res = _repo.GetHeaderById(id);
            FormP1HeaderResponseDTO P1 = new FormP1HeaderResponseDTO();
            P1 = _mapper.Map<FormP1HeaderResponseDTO>(res);
            P1.FormP1Details = _mapper.Map<List<FormP1ResponseDTO>>(res.RmPaymentCertificate);

            return P1;
        }




        public async Task<int> SaveFormP1(FormP1HeaderResponseDTO FormP1)
        {
            try
            {
                var domainModelFormP1 = _mapper.Map<RmPaymentCertificateHeader>(FormP1);
                domainModelFormP1.PchPkRefNo = 0;
                var res = _repo.SaveFormP1(domainModelFormP1);

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormP1.PchSubmissionYear.ToString());
                lstData.Add("MM", domainModelFormP1.PchSubmissionMonth < 10 ? "0" + domainModelFormP1.PchSubmissionMonth.ToString() : domainModelFormP1.PchSubmissionMonth.ToString());
                lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(res.Result));
                domainModelFormP1.PchRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormP1, lstData);
                domainModelFormP1.PchPaymentCertificateNo = res.Result;
                var result = _repo.SaveFormP1(domainModelFormP1, true);

                return res.Result;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> UpdateFormP1(FormP1HeaderResponseDTO FormP1Header, List<FormP1ResponseDTO> FormP1Details)
        {
            try
            {
                int PkRefNo = FormP1Header.PkRefNo;
                var domainModelFormP1 = _mapper.Map<RmPaymentCertificateHeader>(FormP1Header);
                domainModelFormP1.PchPkRefNo = PkRefNo;
                domainModelFormP1.PchActiveYn = true;
                domainModelFormP1 = UpdateStatus(domainModelFormP1);
                _repoUnit.FormP1Repository.Update(domainModelFormP1);
                await _repoUnit.CommitAsync();

                var domainModelFormP1Details = _mapper.Map<List<RmPaymentCertificate>>(FormP1Details);
                return await _repo.UpdateFormP1(domainModelFormP1, domainModelFormP1Details);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public RmPaymentCertificateHeader UpdateStatus(RmPaymentCertificateHeader form)
        {
            if (form.PchPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormF3Repository._context.RmPaymentCertificateHeader.Where(x => x.PchPkRefNo == form.PchPkRefNo).Select(x => new { Status = x.PchStatus, Log = x.PchAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.PchAuditLog = existsObj.Log;
                    form.PchStatus = existsObj.Status;

                }

            }
            if (form.PchSubmitSts && (form.PchStatus == "Saved" || form.PchStatus == "Initialize"))
            {
                form.PchStatus = Common.StatusList.Submitted;
                form.PchAuditLog = Utility.ProcessLog(form.PchAuditLog, "Submitted By", "Submitted", form.PchUsernameSo, string.Empty, form.PchSignDateSo, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "proposed By:" + form.PchUsernameSo + " - Form P1 (" + form.PchPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormP1/add?id=" + form.PchPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }


        public int? DeleteFormP1(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormP1(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        ////public async Task<FORMP1Rpt> GetReportData(int headerid)
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
                FormP1HeaderResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormP1Details;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    for (int sheet = 1; sheet <= 1; sheet++)
                    {
                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {
                            int i = 27;

                            foreach (var r in rpt)
                            {

                                worksheet.Cell(i, 10).Value = r.Amount;
                                worksheet.Cell(i, 13).Value = r.Addition;
                                worksheet.Cell(i, 16).Value = r.Deduction;
                                worksheet.Cell(i, 19).Value = r.PreviousPayment;
                                //worksheet.Cell(i, 22).Value = r.TotalToDate;
                                //worksheet.Cell(i, 25).Value = r.AmountIncludedInPc;
                                i++;
                            }


                            worksheet.Cell(4, 24).Value = rptcol.PaymentCertificateNo;
                            worksheet.Cell(9, 20).Value = rptcol.Bank;
                            worksheet.Cell(10, 7).Value = rptcol.Address;
                            worksheet.Cell(10, 20).Value = rptcol.BankAccNo;
                            worksheet.Cell(11, 20).Value = rptcol.Assignee;
                            worksheet.Cell(14, 20).Value = rptcol.SubmissionDate; //ContractsEndsOn date




                            worksheet.Cell(17, 20).Value = rptcol.ContractRoadLength;
                            worksheet.Cell(18, 20).Value = rptcol.NetValueDeduction;
                            worksheet.Cell(19, 20).Value = rptcol.NetValueAddition;
                            worksheet.Cell(20, 20).Value = rptcol.NetValueInstructedWork;
                            worksheet.Cell(21, 20).Value = rptcol.NetValueLadInstructedWork;

                            worksheet.Cell(23, 13).Value = rptcol.SubmissionMonth;
                            worksheet.Cell(23, 19).Value = rptcol.SubmissionYear;

                            worksheet.Cell(40, 3).Value = "I certify that under the terms of Contract No.PWD / HO / B130 / 2019  , the sum of Ringgit Malaysia(" + rptcol.DueAmount + ") is due to the Service Provider.";

                            worksheet.Cell(47, 18).Value = rptcol.UsernameSo;
                            worksheet.Cell(48, 18).Value = rptcol.DesignationSo;
                            worksheet.Cell(49, 18).Value = rptcol.SignDateSo;

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
