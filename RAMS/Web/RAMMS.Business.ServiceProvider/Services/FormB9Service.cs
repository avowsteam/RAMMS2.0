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
            return GetMaxRev(Year);
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
        //        FORMB9Rpt rpt = await this.GetReportData(id);
        //        System.IO.File.Copy(Oldfilename, cachefile, true);
        //        using (var workbook = new XLWorkbook(cachefile))
        //        {
        //            int noofsheets = (rpt.Details.Count() / 24) + ((rpt.Details.Count() % 24) > 0 ? 1 : 1);
        //            for (int sheet = 2; sheet <= noofsheets; sheet++)
        //            {
        //                using (var tempworkbook = new XLWorkbook(cachefile))
        //                {
        //                    string sheetname = "sheet" + Convert.ToString(sheet);
        //                    IXLWorksheet copysheet = tempworkbook.Worksheet(1);
        //                    copysheet.Worksheet.Name = sheetname;
        //                    copysheet.Cell(5, 7).Value = rpt.Division;
        //                    copysheet.Cell(5, 26).Value = rpt.District;
        //                    copysheet.Cell(5, 47).Value = rpt.RMU;
        //                    copysheet.Cell(6, 7).Value = rpt.RoadCode;
        //                    copysheet.Cell(7, 7).Value = rpt.RoadName;
        //                    copysheet.Cell(6, 26).Value = rpt.CrewLeader;
        //                    copysheet.Cell(5, 72).Value = rpt.InspectedByName;
        //                    copysheet.Cell(6, 72).Value = rpt.InspectedDate.HasValue ? rpt.InspectedDate.Value.ToString("dd-MM-yyyy") : "";
        //                    copysheet.Cell(7, 74).Value = rpt.RoadLength;
        //                    copysheet.Cell(2, 73).Value = sheet;
        //                    copysheet.Cell(2, 80).Value = noofsheets;
        //                    workbook.AddWorksheet(copysheet);
        //                }
        //            }
        //            int index = 1;
        //            int? condition1 = 0;
        //            int? condition2 = 0;
        //            int? condition3 = 0;
        //            string conditiondata1 = "";
        //            string conditiondata2 = "";
        //            string conditiondata3 = "";
        //            for (int sheet = 1; sheet <= noofsheets; sheet++)
        //            {


        //                IXLWorksheet worksheet;
        //                workbook.Worksheets.TryGetWorksheet($"sheet{sheet}", out worksheet);

        //                if (worksheet != null)
        //                {
        //                    worksheet.Cell(5, 7).Value = (rpt.Division == "MIRI" ? "Miri" : rpt.Division);
        //                    worksheet.Cell(5, 26).Value = (rpt.District == "MIRI" ? "Miri" : rpt.District);
        //                    worksheet.Cell(5, 47).Value = (rpt.RMU == "MIRI" ? "Miri" : rpt.RMU);
        //                    worksheet.Cell(6, 7).Value = rpt.RoadCode;
        //                    worksheet.Cell(7, 7).Value = rpt.RoadName;
        //                    worksheet.Cell(6, 26).Value = rpt.CrewLeader;
        //                    worksheet.Cell(5, 72).Value = rpt.InspectedByName;
        //                    worksheet.Cell(6, 72).Value = rpt.InspectedDate.HasValue ? rpt.InspectedDate.Value.ToString("dd-MM-yyyy") : "";
        //                    worksheet.Cell(7, 74).Value = rpt.RoadLength;
        //                    worksheet.Cell(2, 80).Value = noofsheets;
        //                    //worksheet.Cell(9, 8).Value = condition1.ToString() == "0" ? "" : condition1.ToString();
        //                    //worksheet.Cell(9, 24).Value = condition2.ToString() == "0" ? "" : condition1.ToString();
        //                    //worksheet.Cell(9, 45).Value = condition3.ToString() == "0" ? "" : condition1.ToString();
        //                    int i = 14;

        //                    var data = rpt.Details.Skip((sheet - 1) * 24).Take(24);
        //                    foreach (var r in data)
        //                    {


        //                        if (r.Condition == 1)
        //                        {
        //                            condition1 += 1;
        //                        }
        //                        if (r.Condition == 2)
        //                        {
        //                            condition2 += 1;
                                    
        //                        }
        //                        if (r.Condition == 3)
        //                        {
        //                            condition3 += 1;
        //                        }

        //                        worksheet.Cell(i, 2).Value = index;

        //                        worksheet.Cell(i, 4).Value = $"{r.LocationChKm}+{r.LocationChM}";
        //                        worksheet.Cell(i, 8).Value = r.StructCode;
        //                        worksheet.Cell(i, 10).Value = r.Tier;
        //                        worksheet.Cell(i, 17).Value = r.Length;
        //                        worksheet.Cell(i, 24).Value = r.Width;
        //                        worksheet.Cell(i, 31).Value = r.BottomWidth;
        //                        worksheet.Cell(i, 38).Value = r.Height;
        //                        worksheet.Cell(i, 45).Value = r.Condition;
        //                        worksheet.Cell(i, 52).Value = r.Descriptions;

        //                        index++;
        //                        i++;

        //                    }
        //                    worksheet.Cell(39, 8).Value = condition1;
        //                    worksheet.Cell(39, 24).Value = condition2;
        //                    worksheet.Cell(39, 45).Value = condition3;
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
