﻿using System;
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

    public class FormB9Service : IFormB9Service
    {
        private readonly IFormB9Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormB9Service(IRepositoryUnit repoUnit, IFormB9Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormB9ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB9SearchGridDTO> filterOptions)
        {
            PagingResult<FormB9ResponseDTO> result = new PagingResult<FormB9ResponseDTO>();
            List<FormB9ResponseDTO> formAlist = new List<FormB9ResponseDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }

        public async Task<PagingResult<FormB9HistoryResponseDTO>> GetFormB9HistoryGridList(FilteredPagingDefinition<FormB9HistoryResponseDTO> filterOptions)
        {
            PagingResult<FormB9HistoryResponseDTO> result = new PagingResult<FormB9HistoryResponseDTO>();
            List<FormB9HistoryResponseDTO> formAlist = new List<FormB9HistoryResponseDTO>();
            result.PageResult = await _repo.GetFormB9HistoryGridList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }

        public async Task<FormB9ResponseDTO> GetHeaderById(int id)
        {
            RmB9DesiredService res = _repo.GetHeaderById(id);
            FormB9ResponseDTO B9 = new FormB9ResponseDTO();
            B9 = _mapper.Map<FormB9ResponseDTO>(res);
            B9.FormB9History = _mapper.Map<List<FormB9HistoryResponseDTO>>(res.RmB9DesiredServiceHistory);
            return B9;
        }

        public int? GetMaxRev(int Year)
        {
            return _repo.GetMaxRev(Year);
        }

        public async Task<int> SaveFormB9(FormB9ResponseDTO FormB9, List<FormB9HistoryResponseDTO> FormB9History)
        {
            try
            {
                var domainModelFormB9 = _mapper.Map<RmB9DesiredService>(FormB9);
                domainModelFormB9.B9dsPkRefNo = 0;
                var domainModelFormB9History = _mapper.Map<List<RmB9DesiredServiceHistory>>(FormB9History);

                return await _repo.SaveFormB9(domainModelFormB9, domainModelFormB9History);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }




        //public async Task<FORMB9Rpt> GetReportData(int headerid)
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
                FormB9ResponseDTO rptcol = await this.GetHeaderById(id);
                var rpt = rptcol.FormB9History;
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
                                worksheet.Cell(i, 5).Value = r.Cond1;
                                worksheet.Cell(i, 6).Value = r.Cond2;
                                worksheet.Cell(i, 7).Value = r.Cond3;
                                worksheet.Cell(i, 8).Value = r.UnitOfService;
                                worksheet.Cell(i, 9).Value = r.Remarks;
                                
 
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