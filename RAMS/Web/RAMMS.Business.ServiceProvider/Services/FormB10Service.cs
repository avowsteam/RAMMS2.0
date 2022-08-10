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

    public class FormB10Service : IFormB10Service
    {
        private readonly IFormB10Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormB10Service(IRepositoryUnit repoUnit, IFormB10Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormB10ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB10SearchGridDTO> filterOptions)
        {
            PagingResult<FormB10ResponseDTO> result = new PagingResult<FormB10ResponseDTO>();
            List<FormB10ResponseDTO> formAlist = new List<FormB10ResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }

     
        public async Task<FormB10ResponseDTO> GetHeaderById(int id)
        {
            RmB10DailyProduction res = _repo.GetHeaderById(id);
            FormB10ResponseDTO B10 = new FormB10ResponseDTO();
            B10 = _mapper.Map<FormB10ResponseDTO>(res);
            B10.FormB10History = _mapper.Map<List<FormB10HistoryResponseDTO>>(res.RmB10DailyProductionHistory);
            return B10;
        }

        public int? GetMaxRev(int Year)
        {
            return _repo.GetMaxRev(Year);
        }

        public async Task<int> SaveFormB10(FormB10ResponseDTO FormB10, List<FormB10HistoryResponseDTO> FormB10History)
        {
            try
            {
                var domainModelFormB10 = _mapper.Map<RmB10DailyProduction>(FormB10);
                domainModelFormB10.B10dpPkRefNo = 0;
                var domainModelFormB10History = _mapper.Map<List<RmB10DailyProductionHistory>>(FormB10History);

                return await _repo.SaveFormB10(domainModelFormB10, domainModelFormB10History);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }




        //public async Task<FORMB10Rpt> GetReportData(int headerid)
        //{
        //    return await _repo.GetReportData(headerid);
        //}

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
                FormB10ResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormB10History;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    for (int sheet = 1; sheet <= 1; sheet++)
                    {
                        IXLWorksheet worksheet;
                        workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

                        if (worksheet != null)
                        {
                            int i = 6;
                            
                            foreach (var r in rpt)
                            {

                                worksheet.Cell(i, 1).Value = r.Feature;
                                worksheet.Cell(i, 2).Value = r.Code;
                                worksheet.Cell(i, 4).Value = r.Name;
                                worksheet.Cell(i, 5).Value = r.AdpValue;
                                worksheet.Cell(i, 6).Value = r.AdpUnit;

                                i++;

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

    }
}
