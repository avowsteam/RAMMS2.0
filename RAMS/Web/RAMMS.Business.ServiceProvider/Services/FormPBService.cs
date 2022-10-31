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
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormPB.PbiwSubmissionYear.ToString());
                lstData.Add("MM", domainModelFormPB.PbiwSubmissionMonth.ToString());
                domainModelFormPB.PbiwRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormPB, lstData);
                var res = _repo.SaveFormPB(domainModelFormPB);
                 
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
        //        FormPBDetailResponseDTO rptcol = await this.GetHeaderById(id);
        //        var rpt = rptcol.FormPBHistory;
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
        //                    var rev = rptcol.FormPBRevisionHistory;
        //                    foreach (var r in rev)
        //                    {
        //                        worksheet.Cell(j, 1).Value = r.Date;
        //                        worksheet.Cell(j, 2).Value = r.Description;
        //                        worksheet.Cell(j, 6).Value = r.RevNo;
        //                        j++;
        //                    }

        //                    worksheet.Cell(3, 1).Value = "APPENDIX PB - " + rptcol.Rmu;
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
