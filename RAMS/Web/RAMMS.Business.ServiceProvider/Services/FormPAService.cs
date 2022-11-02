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

    public class FormPAService : IFormPAService
    {
        private readonly IFormPARepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormPAService(IRepositoryUnit repoUnit, IFormPARepository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormPAHeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormPASearchGridDTO> filterOptions)
        {
            PagingResult<FormPAHeaderResponseDTO> result = new PagingResult<FormPAHeaderResponseDTO>();
            List<FormPAHeaderResponseDTO> formAlist = new List<FormPAHeaderResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }


        public async Task<FormPAHeaderResponseDTO> GetHeaderById(int id)
        {
            RmPaymentCertificateMamw res = _repo.GetHeaderById(id);
            FormPAHeaderResponseDTO PA = new FormPAHeaderResponseDTO();
            PA = _mapper.Map<FormPAHeaderResponseDTO>(res);
          //  PA.FormPADetails= _mapper.Map<List<FormPAResponseDTO>>(res.RmPaymentCertificate);
           
            return PA;
        }

       


        public async Task<int> SaveFormPA(FormPAHeaderResponseDTO FormPA)
        {
            try
            {
                var domainModelFormPA = _mapper.Map<RmPaymentCertificateMamw>(FormPA);
                domainModelFormPA.PcmamwPkRefNo = 0;
               
                var res = _repo.SaveFormPA(domainModelFormPA);
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormPA.PcmamwSubmissionYear.ToString());
                lstData.Add("MM", domainModelFormPA.PcmamwSubmissionMonth.ToString());
                lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(res.Result));
                domainModelFormPA.PcmamwRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormPA, lstData);
                var result = _repo.SaveFormPA(domainModelFormPA,true);

                return result.Result;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }
 
        public async Task<int> UpdateFormPA(FormPAHeaderResponseDTO FormPAHeader, List<FormPACRRResponseDTO> FormPACrr, List<FormPACRRAResponseDTO> FormPACrra, List<FormPACRRDResponseDTO> FormPACrrd)
        {
            try
            {
                int PkRefNo = FormPAHeader.PkRefNo;
                var domainModelFormPA = _mapper.Map<RmPaymentCertificateMamw>(FormPAHeader);
                domainModelFormPA.PcmamwPkRefNo = PkRefNo;
                domainModelFormPA.PcmamwActiveYn = true;
                domainModelFormPA.RmPaymentCertificateCrr = new List<RmPaymentCertificateCrr>();
                domainModelFormPA.RmPaymentCertificateCrrd = new List<RmPaymentCertificateCrrd>();
                domainModelFormPA.RmPaymentCertificateCrra = new List<RmPaymentCertificateCrra>();
                domainModelFormPA = UpdateStatus(domainModelFormPA);
                _repoUnit.FormPARepository.Update(domainModelFormPA);
                await _repoUnit.CommitAsync();

                var domainModelFormPACrr = _mapper.Map<List<RmPaymentCertificateCrr>>(FormPACrr);
                var domainModelFormPACrra = _mapper.Map<List<RmPaymentCertificateCrra>>(FormPACrra);
                var domainModelFormPACrrd = _mapper.Map<List<RmPaymentCertificateCrrd>>(FormPACrrd);
               
                return await _repo.UpdateFormPA(domainModelFormPA, domainModelFormPACrr, domainModelFormPACrra, domainModelFormPACrrd);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public RmPaymentCertificateMamw UpdateStatus(RmPaymentCertificateMamw form)
        {
            if (form.PcmamwPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormF3Repository._context.RmPaymentCertificateMamw.Where(x => x.PcmamwPkRefNo == form.PcmamwPkRefNo).Select(x => new { Status = x.PcmamwStatus, Log = x.PcmamwAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.PcmamwAuditLog = existsObj.Log;
                    form.PcmamwStatus = existsObj.Status;

                }

            }
            if (form.PcmamwSubmitSts && (form.PcmamwStatus == "Saved" || form.PcmamwStatus == "Initialize"))
            {
                form.PcmamwStatus = Common.StatusList.Submitted;
                form.PcmamwAuditLog = Utility.ProcessLog(form.PcmamwAuditLog, "Submitted By", "Submitted", form.PcmamwUsernameSo, string.Empty, form.PcmamwSignDateSo, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "proposed By:" + form.PcmamwUsernameSo + " - Form PA (" + form.PcmamwPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormPA/add?id=" + form.PcmamwPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }


        public int? DeleteFormPA(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormPA(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        ////public async Task<FORMPARpt> GetReportData(int headerid)
        ////{
        ////    return await _repo.GetReportData(headerid);
        ////}

        //public async Task<byte[]> FormDownload(string formname, int id, string filepath)
        //{
        //    string Oldfilename = "";
        //    string filename = "";
        //    string cachefile = "";
        //    if (!filepath.Contains(".xlsx"))
        //    {
        //        Oldfilename = filepath + formname + ".xlsx";
        //        filename = formname + DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString();
        //        cachefile = filepath + filename + ".xlsx";
        //    }
        //    else
        //    {
        //        Oldfilename = filepath;
        //        filename = filepath.Replace(".xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString() + ".xlsx");
        //        cachefile = filename;
        //    }

        //    try
        //    {
        //        FormPAResponseDTO rptcol = await this.GetHeaderById(id);
        //        var rpt = rptcol.FormPAHistory;
        //        System.IO.File.Copy(Oldfilename, cachefile, true);
        //        using (var workbook = new XLWorkbook(cachefile))
        //        {
        //            for (int sheet = 1; sheet <= 1; sheet++)
        //            {
        //                IXLWorksheet worksheet;
        //                workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

        //                if (worksheet != null)
        //                {
        //                    int i = 10;

        //                    foreach (var r in rpt)
        //                    {

        //                        worksheet.Cell(i, 5).Value = r.InvCond1;
        //                        worksheet.Cell(i, 6).Value = r.InvCond2;
        //                        worksheet.Cell(i, 7).Value = r.InvCond3;
        //                        worksheet.Cell(i, 9).Value = r.SlCond1;
        //                        worksheet.Cell(i, 10).Value = r.SlCond2;
        //                        worksheet.Cell(i, 11).Value = r.SlCond3;
        //                        worksheet.Cell(i, 17).Value = r.CdcLabour;
        //                        worksheet.Cell(i, 18).Value = r.CdcEquipment;
        //                        worksheet.Cell(i, 19).Value = r.CdcMaterial;
        //                        worksheet.Cell(i, 21).Value = r.AverageDailyProduction;
        //                        worksheet.Cell(i, 22).Value = r.UnitOfService;
        //                        worksheet.Cell(i, 25).Value = r.SlAvgDesired;

        //                        i++;

        //                    }

        //                    int j = 54;
        //                    var rev = rptcol.FormPARevisionHistory;
        //                    foreach (var r in rev)
        //                    {
        //                        worksheet.Cell(j, 1).Value = r.Date;
        //                        worksheet.Cell(j, 2).Value = r.Description;
        //                        worksheet.Cell(j, 6).Value = r.RevNo;
        //                        j++;
        //                    }

        //                    worksheet.Cell(3, 1).Value = "APPENDIX PA - " + rptcol.Rmu;
        //                    worksheet.Cell(5, 27).Value = rptcol.RevisionNo;
        //                    worksheet.Cell(5, 29).Value = rptcol.RevisionDate;

        //                    worksheet.Cell(55, 15).Value = rptcol.UserNameProsd;
        //                    worksheet.Cell(55, 19).Value = rptcol.UserNameFclitd;
        //                    worksheet.Cell(55, 24).Value = rptcol.UserNameAgrd;
        //                    worksheet.Cell(55, 28).Value = rptcol.UserNameEdosd;
        //                }
        //            }


        //            using (var stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var content = stream.ToArray();
        //                System.IO.File.Delete(cachefile);
        //                return content;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.IO.File.Copy(Oldfilename, cachefile, true);
        //        using (var workbook = new XLWorkbook(cachefile))
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var content = stream.ToArray();
        //                System.IO.File.Delete(cachefile);
        //                return content;
        //            }
        //        }

        //    }
        //}

    }
}
