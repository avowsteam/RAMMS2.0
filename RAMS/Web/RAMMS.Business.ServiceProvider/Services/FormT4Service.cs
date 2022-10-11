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

    public class FormT4Service : IFormT4Service
    {
        private readonly IFormT4Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormT4Service(IRepositoryUnit repoUnit, IFormT4Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormT4HeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormT4SearchGridDTO> filterOptions)
        {
            PagingResult<FormT4HeaderResponseDTO> result = new PagingResult<FormT4HeaderResponseDTO>();
            List<FormT4ResponseDTO> formAlist = new List<FormT4ResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }


        public async Task<FormT4HeaderResponseDTO> GetHeaderById(int id)
        {
            RmT4DesiredBdgtHeader res = _repo.GetHeaderById(id);
            FormT4HeaderResponseDTO T4 = new FormT4HeaderResponseDTO();
            T4 = _mapper.Map<FormT4HeaderResponseDTO>(res);
            T4.FormT4 = _mapper.Map<List<FormT4ResponseDTO>>(res.RmT4DesiredBdgt);
           
            return T4;
            
        }

        public int? GetMaxRev(int Year, string RMU)
        {
            return _repo.GetMaxRev(Year, RMU);
        }


        public async Task<FormT4HeaderResponseDTO> SaveFormT4(FormT4HeaderResponseDTO FormT4)
        {
            try
            {
                var domainModelFormT4 = _mapper.Map<RmT4DesiredBdgtHeader>(FormT4);
                domainModelFormT4.T4dbhPkRefNo = 0;
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYY", domainModelFormT4.T4dbhRevisionYear.ToString());
                lstData.Add("RMU", domainModelFormT4.T4dbhRmu.ToString());
                lstData.Add("RevisionNo", domainModelFormT4.T4dbhRevisionNo.ToString());
                domainModelFormT4.T4dbhPkRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormT4, lstData);
                var res = _repo.SaveFormT4(domainModelFormT4);

                FormT4.PkRefNo = res.Result.T4dbhPkRefNo;
                return FormT4;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        
        public async Task<int> UpdateFormT4(FormT4HeaderResponseDTO FormT4, List<FormT4ResponseDTO> FormT4History)
        {
            try
            {
                int PkRefNo = FormT4.PkRefNo;
                var domainModelFormT4 = _mapper.Map<RmT4DesiredBdgtHeader>(FormT4);
                domainModelFormT4.T4dbhPkRefNo = PkRefNo;
                domainModelFormT4.T4dbhActiveYn = true;
                domainModelFormT4 = UpdateStatus(domainModelFormT4);
                _repoUnit.FormT4Repository.Update(domainModelFormT4);
                await _repoUnit.CommitAsync();

                var domainModelFormT4History = _mapper.Map<List<RmT4DesiredBdgt>>(FormT4History);
                return await _repo.UpdateFormT4(domainModelFormT4, domainModelFormT4History);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public RmT4DesiredBdgtHeader UpdateStatus(RmT4DesiredBdgtHeader form)
        {
            if (form.T4dbhPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormF3Repository._context.RmT4DesiredBdgtHeader.Where(x => x.T4dbhPkRefNo == form.T4dbhPkRefNo).Select(x => new { Status = x.T4dbhStatus, Log = x.T4dbhAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.T4dbhAuditLog = existsObj.Log;
                    form.T4dbhStatus = existsObj.Status;

                }

            }
            if (form.T4dbhSubmitSts && (form.T4dbhStatus == "Saved" || form.T4dbhStatus == "Initialize"))
            {
                form.T4dbhStatus = Common.StatusList.Submitted;
                form.T4dbhAuditLog = Utility.ProcessLog(form.T4dbhAuditLog, "Submitted By", "Submitted", _security.UserName, string.Empty, form.T4dbhRevisionDate, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "proposed By:" + _security.UserName + " - Form T4 (" + form.T4dbhPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormT4/add?id=" + form.T4dbhPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }


        public int? DeleteFormT4(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormT4(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        ////public async Task<FORMT4Rpt> GetReportData(int headerid)
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
                FormT4HeaderResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormT4;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    for (int sheet = 1; sheet <= 1; sheet++)
                    {
                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {
                            int i = 9;

                            foreach (var r in rpt)
                            {

                                worksheet.Cell(i, 4).Value = r.InvCond1;
                                worksheet.Cell(i, 5).Value = r.InvCond2;
                                worksheet.Cell(i, 6).Value = r.InvCond3;
                                worksheet.Cell(i, 8).Value = r.SlCond1;
                                worksheet.Cell(i, 9).Value = r.SlCond2;
                                worksheet.Cell(i, 10).Value = r.SlCond3;
                                worksheet.Cell(i, 16).Value = r.AverageDailyProduction;
                                worksheet.Cell(i, 17).Value = r.UnitOfService;
                                worksheet.Cell(i, 19).Value = r.CdcLabour;
                                worksheet.Cell(i, 20).Value = r.CdcEquipment;
                                worksheet.Cell(i, 21).Value = r.CdcMaterial;
                               
                                i++;

                            }
 
                         
                            worksheet.Cell(4, 22).Value = rptcol.RevisionNo;
                            worksheet.Cell(4, 24).Value = rptcol.RevisionDate;

                          
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
