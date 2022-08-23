﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Common;
using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.Repository.Interfaces;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormB7Service : IFormB7Service
    {
        private readonly IFormB7Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;
        public FormB7Service(IRepositoryUnit repoUnit, IFormB7Repository repo,
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

        public async Task<FormB7HeaderDTO> GetHeaderById(int id)
        {
            RmB7Hdr res = _repo.GetHeaderById(id);
            FormB7HeaderDTO FormB7 = new FormB7HeaderDTO();
            FormB7 = _mapper.Map<FormB7HeaderDTO>(res);
            FormB7.RmB7LabourHistory = _mapper.Map<List<FormB7LabourHistoryDTO>>(res.RmB7LabourHistory);
            FormB7.RmB7MaterialHistory = _mapper.Map<List<FormB7MaterialHistoryDTO>>(res.RmB7MaterialHistory);
            FormB7.RmB7EquipmentsHistory = _mapper.Map<List<FormB7EquipmentsHistoryDTO>>(res.RmB7EquipmentsHistory);
            return FormB7;
        }

        public int? GetMaxRev(int Year)
        {
            return _repo.GetMaxRev(Year);
        }

        public async Task<int> SaveFormB7(FormB7HeaderDTO FormB7)
        {
            try
            {
                var domainModelFormB7 = _mapper.Map<RmB7Hdr>(FormB7);
                domainModelFormB7.B7hPkRefNo = 0;

                return await _repo.SaveFormB7(domainModelFormB7);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
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
                FormB7Rpt _rpt = this.GetReportData(id).Result;
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);

                    using (var book = new XLWorkbook(cachefile))
                    {
                        if (worksheet != null)
                        {
                            worksheet.Cell(1, 1).Value = "APPENDIX B7: L.E.M Unit Price (" + _rpt.Year + ") for RMU Batu Niah and RMU Miri";

                            var Labour = _rpt.Labours;
                            var i = 4;
                            foreach (var lab in Labour)
                            {
                                worksheet.Cell(i + 1, 1).Value = lab.Code;
                                worksheet.Cell(i + 1, 2).Value = lab.Name;
                                worksheet.Cell(i + 1, 3).Value = lab.Unit + " hrs";
                                worksheet.Cell(i + 1, 4).Value = lab.UnitPriceBatuNiah;
                                worksheet.Cell(i + 1, 5).Value = lab.UnitPriceMiri;
                                i++;
                            }

                            

                            //Material

                            var Materials = _rpt.Materials;
                            i = i + 3;
                            foreach (var mat in Materials)
                            {
                                worksheet.Cell(i + 1, 1).Value = mat.Code;
                                worksheet.Cell(i + 1, 2).Value = mat.Name;
                                worksheet.Cell(i + 1, 3).Value = mat.Unit;
                                worksheet.Cell(i + 1, 4).Value = mat.UnitPriceBatuNiah;
                                worksheet.Cell(i + 1, 5).Value = mat.UnitPriceMiri;
                                i++;
                            }

                            //Equipement

                            var Equipements = _rpt.Equipments;
                            i = 4;
                            foreach (var eqp in Equipements)
                            {
                                worksheet.Cell(i + 1, 7).Value = eqp.Code;
                                worksheet.Cell(i + 1, 8).Value = eqp.Name;
                                worksheet.Cell(i + 1, 9).Value = "Nr./" + eqp.Unit + " hrs";
                                worksheet.Cell(i + 1, 10).Value = eqp.UnitPriceBatuNiah;
                                worksheet.Cell(i + 1, 11).Value = eqp.UnitPriceMiri;
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

        public async Task<FormB7Rpt> GetReportData(int headerid)
        {
            return await _repo.GetReportData(headerid);
        }


    }
}