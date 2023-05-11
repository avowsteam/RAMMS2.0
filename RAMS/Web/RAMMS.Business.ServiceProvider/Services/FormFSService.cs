using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using System.Linq;
using RAMMS.DTO.Report;
using ClosedXML.Excel;
using System.IO;
using System.Collections.Generic;
using RAMMS.Common;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormFSService : IFormFSService
    {
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        public FormFSService(IRepositoryUnit repoUnit, IMapper mapper, ISecurity security, IProcessService proService)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit)); _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security ?? throw new ArgumentNullException(nameof(security));
            processService = proService;
        }
        public long LastHeaderInsertedNo()
        {
            var model = _repoUnit.FormFSHeaderRepository.GetAll().OrderByDescending(s => s.FshPkRefNo).FirstOrDefault();
            if (model != null) { return model.FshPkRefNo; } else { return 0; }
        }
        public async Task<FormFSHeaderRequestDTO> GetHeaderById(int id)
        {
            var model = await _repoUnit.FormFSHeaderRepository.FindAsync(s => s.FshPkRefNo == id);
            if (model == null) { return null; }
            var fs = _mapper.Map<Domain.Models.RmFormFsInsHdr, FormFSHeaderRequestDTO>(model);
            var road = _repoUnit.RoadmasterRepository.FindAll(s => s.RdmPkRefNo == fs.RoadId).FirstOrDefault();
            fs.SecCode = road.RdmSecCode.GetValueOrDefault();
            fs.SecName = road.RdmSecName;
            fs.RmuCode = road.RdmRmuCode;
            fs.RmuName = road.RdmRmuName;
            fs.RoadName = road.RdmRdName;
            return fs;
        }
        public async Task<int> SaveHeader(FormFSHeaderRequestDTO model)
        {
            try
            {
                bool IsAdd = false;
                var form = _mapper.Map<Domain.Models.RmFormFsInsHdr>(model);
                form.FshStatus = StatusList.FormFSInit;
                var road = _repoUnit.RoadmasterRepository.FindAll(s => s.RdmRdCode == form.FshRoadCode).FirstOrDefault();
                if (road != null)
                {
                    form.FshRoadId = road.RdmPkRefNo;
                    form.FshRoadLength = road.RdmLengthPaved;
                }
                form.FshActiveYn = true;
                List<Domain.Models.RmFormFsInsDtl> details = null;
                if (form.FshPkRefNo != 0)
                {
                    form.FshModBy = _security.UserID;
                    form.FshModDt = DateTime.Now;

                    if (form.FshSubmitSts == true)
                    {
                        var count = _repoUnit.FormFSDetailRepository.FindAll(s => s.FsdFshPkRefNo == form.FshPkRefNo && (s.FsdRemarks == "" || s.FsdRemarks == null || s.FsdNeeded == "" || s.FsdNeeded == null) && s.FsdActiveYn == true).Count();
                        if (count > 0)
                        {
                            return -2;
                        }
                    }
                    _repoUnit.FormFSHeaderRepository.Update(form);
                }
                else
                {
                    details = _repoUnit.FormFSDetailRepository.GetDetailsforInsert(form.FshPkRefNo, _security.UserID, form);
                    if (details == null || details.Count == 0)
                        return -1;

                    form.FshCrBy = _security.UserID;
                    form.FshModBy = _security.UserID;
                    form.FshCrDt = DateTime.Now;
                    form.FshModDt = DateTime.Now;
                    _repoUnit.FormFSHeaderRepository.Create(form);
                    IsAdd = true;
                }
                await _repoUnit.CommitAsync();

                if (IsAdd)
                {
                    _repoUnit.FormFSDetailRepository.BulkInsert(details, form.FshPkRefNo);
                }
                if (form != null && form.FshSubmitSts)
                {
                    int iResult = processService.Save(new DTO.RequestBO.ProcessDTO()
                    {
                        ApproveDate = DateTime.Now,
                        Form = "FormFS",
                        IsApprove = true,
                        RefId = form.FshPkRefNo,
                        Remarks = "",
                        Stage = form.FshStatus
                    }).Result;
                }
                return form.FshPkRefNo;
            }
            catch (Exception ex) { await _repoUnit.RollbackAsync(); throw ex; }
        }
        public async Task<int> FindDetail(FormFSHeaderRequestDTO model)
        {
            try
            {
                bool IsAdd = false;
                var form = _mapper.Map<Domain.Models.RmFormFsInsHdr>(model);
                form.FshStatus = StatusList.FormFSInit;
                var exists = _repoUnit.FormFSHeaderRepository.Find(s => s.FshActiveYn == true && s.FshYearOfInsp == model.YearOfInsp && s.FshRoadCode == model.RoadCode);
                if (exists != null)
                    return exists.FshPkRefNo;

                var road = _repoUnit.RoadmasterRepository.FindAll(s => s.RdmRdCode == form.FshRoadCode).FirstOrDefault();
                if (road != null)
                {
                    form.FshRoadId = road.RdmPkRefNo;
                    form.FshRoadLength = road.RdmLengthPaved;
                }
                form.FshActiveYn = true;
                List<Domain.Models.RmFormFsInsDtl> details = null;
                if (form.FshPkRefNo != 0)
                {
                    form.FshModBy = _security.UserID;
                    form.FshModDt = DateTime.Now;
                    _repoUnit.FormFSHeaderRepository.Update(form);
                }
                else
                {
                    details = _repoUnit.FormFSDetailRepository.GetDetailsforInsert(form.FshPkRefNo, _security.UserID, form);

                    if (details == null || details.Count == 0)
                        return -1;

                    form.FshCrBy = _security.UserID;
                    form.FshModBy = _security.UserID;
                    form.FshCrDt = DateTime.Now;
                    form.FshModDt = DateTime.Now;
                    _repoUnit.FormFSHeaderRepository.Create(form);
                    IsAdd = true;
                }
                await _repoUnit.CommitAsync();

                if (IsAdd)
                {
                    _repoUnit.FormFSDetailRepository.BulkInsert(details, form.FshPkRefNo);
                }
                if (form != null && form.FshSubmitSts)
                {
                    int iResult = processService.Save(new DTO.RequestBO.ProcessDTO()
                    {
                        ApproveDate = DateTime.Now,
                        Form = "FormFS",
                        IsApprove = true,
                        RefId = form.FshPkRefNo,
                        Remarks = "",
                        Stage = form.FshStatus
                    }).Result;
                }
                return form.FshPkRefNo;
            }
            catch (Exception ex) { await _repoUnit.RollbackAsync(); throw ex; }
        }
        public async Task<bool> RemoveHeader(int id)
        {
            var model = _repoUnit.FormFSHeaderRepository.Find(s => s.FshPkRefNo == id);
            if (model != null)
            {
                model.FshActiveYn = false;
                return await _repoUnit.CommitAsync() != 0;
            }
            else { return false; }
        }
        public async Task<PagingResult<FormFSHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormFSHeaderRequestDTO> filterOptions)
        {
            PagingResult<FormFSHeaderRequestDTO> result = new PagingResult<FormFSHeaderRequestDTO>();
            result.PageResult = await _repoUnit.FormFSHeaderRepository.GetFilteredRecordList(filterOptions);
            result.TotalRecords = await _repoUnit.FormFSHeaderRepository.GetFilteredRecordCount(filterOptions); return result;
        }
        public long LastDetailInsertedNo()
        {
            var model = _repoUnit.FormFSDetailRepository.GetAll().OrderByDescending(s => s.FsdPkRefNo).FirstOrDefault();
            if (model != null) { return model.FsdPkRefNo; } else { return 0; }
        }
        public async Task<FormFSDetailRequestDTO> GetDetailById(int id)
        {
            var model = await _repoUnit.FormFSDetailRepository.FindAsync(s => s.FsdPkRefNo == id); if (model == null) { return null; }
            return _mapper.Map<Domain.Models.RmFormFsInsDtl, FormFSDetailRequestDTO>(model);
        }
        public async Task<int> SaveDetail(FormFSDetailRequestDTO model)
        {
            try
            {
                Domain.Models.RmFormFsInsDtl dtl = _repoUnit.FormFSDetailRepository.FindAll(s => s.FsdPkRefNo == model.PkRefNo).FirstOrDefault();
                //dtl.FsdCondition1 = model.Condition1;
                //dtl.FsdCondition2 = model.Condition2;
                //dtl.FsdCondition3 = model.Condition3;
                dtl.FsdNeeded = model.Needed;
                dtl.FsdRemarks = model.Remarks;
                return await _repoUnit.CommitAsync();

            }
            catch (Exception ex) { await _repoUnit.RollbackAsync(); throw ex; }
        }


        public async Task<bool> RemoveDetail(int id)
        {
            var model = _repoUnit.FormFSDetailRepository.Find(s => s.FsdPkRefNo == id);
            if (model != null) { return await _repoUnit.CommitAsync() != 0; } else { return false; }
        }
        public async Task<PagingResult<FormFSDetailRequestDTO>> GetDetailList(FilteredPagingDefinition<FormFSDetailRequestDTO> filterOptions)
        {
            PagingResult<FormFSDetailRequestDTO> result = new PagingResult<FormFSDetailRequestDTO>();
            result.PageResult = await _repoUnit.FormFSDetailRepository.GetFilteredRecordList(filterOptions);
            result.TotalRecords = await _repoUnit.FormFSDetailRepository.GetFilteredRecordCount(filterOptions); return result;
        }

        public FormFSRpt GetReportData(int headerid)
        {
            return _repoUnit.FormFSHeaderRepository.GetReportData(headerid);
        }
        private double? GetAverageWidth(string FsdFeature, double? FsdWidth, Dictionary<string, List<Dictionary<string, string>>> AvgWidth, string FsdGrpCode, string FsdGrpType)
        {
            string avgWidthNew = "";
            if (AvgWidth != null)
            {               
                if (AvgWidth.ContainsKey("CLM"))
                {
                    var cw = AvgWidth["CLM"];
                    foreach (var c in cw)
                    {
                        if (c.ContainsValue("Paint"))
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Thermoplastic"))
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                    }
                }
                if (AvgWidth.ContainsKey("CW") && FsdGrpCode == "CW")
                {
                    var cw = AvgWidth["CW"];
                    foreach (var c in cw)
                    {
                        if (c.ContainsValue("Asphalt") && FsdGrpType == "Asphalt")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Surface Dressed") && FsdGrpType == "Surface Dressed")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Gravel") && FsdGrpType == "Gravel")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Earth") && FsdGrpType == "Earth")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Concrete") && FsdGrpType == "Concrete")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                        else if (c.ContainsValue("Sand") && FsdGrpType == "Sand")
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgWidthNew = c["AvgWidth"];
                                break;
                            }
                        }
                    }
                }
            }
            if ((FsdFeature == "CENTER LINE MARKING" || FsdFeature == "CARRIAGE WAY") && !string.IsNullOrEmpty(avgWidthNew))
            {
                return Convert.ToDouble(avgWidthNew);
            }
            return FsdWidth;
        }

        public byte[] FormDownload(string formname, int id, string basepath, string filepath)
        {
            string Oldfilename = "";
            string filename = "";
            string cachefile = "";
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
                FormFSRpt rpt = this.GetReportData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {

                    IXLWorksheet worksheet = workbook.Worksheet(1);

                    if (worksheet != null)
                    {
                        worksheet.Cell(3, 19).Value = rpt.SummarizedBy;
                        worksheet.Cell(7, 19).Value = rpt.CheckedBy;
                        worksheet.Cell(6, 5).Value = rpt.RMU;
                        worksheet.Cell(6, 11).Value = rpt.District;
                        worksheet.Cell(7, 5).Value = rpt.RoadCode;
                        worksheet.Cell(7, 11).Value = rpt.Division;
                        worksheet.Cell(8, 5).Value = rpt.RoadName;
                        worksheet.Cell(5, 8).Value = rpt.CrewLeader;
                        if (rpt.DateOfInspection.HasValue)
                        {
                            worksheet.Cell(7, 15).Value = rpt.DateOfInspection.Value.Day;
                            worksheet.Cell(7, 17).Value = rpt.DateOfInspection.Value.Month;
                            worksheet.Cell(7, 18).Value = rpt.DateOfInspection.Value.Year;
                        }
                        int i = 12;
                        double? AverageWidth = GetAverageWidth(rpt.CWAsphaltic.FsdFeature, rpt.CWAsphaltic.AverageWidth, rpt.AvgWidth, rpt.CWAsphaltic.FsdGrpCode, rpt.CWAsphaltic.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWAsphaltic.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWAsphaltic.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWAsphaltic.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWAsphaltic.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWAsphaltic.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWAsphaltic.Remarks;

                        i = 13;
                        AverageWidth = GetAverageWidth(rpt.CWSurfaceDressed.FsdFeature, rpt.CWSurfaceDressed.AverageWidth, rpt.AvgWidth, rpt.CWSurfaceDressed.FsdGrpCode, rpt.CWSurfaceDressed.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWSurfaceDressed.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWSurfaceDressed.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWSurfaceDressed.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWSurfaceDressed.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWSurfaceDressed.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWSurfaceDressed.Remarks;
                        i = 14;
                        AverageWidth = GetAverageWidth(rpt.CWConcrete.FsdFeature, rpt.CWConcrete.AverageWidth, rpt.AvgWidth, rpt.CWConcrete.FsdGrpCode, rpt.CWConcrete.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWConcrete.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWConcrete.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWConcrete.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWConcrete.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWConcrete.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWConcrete.Remarks;
                        i = 15;
                        AverageWidth = GetAverageWidth(rpt.CWGravel.FsdFeature, rpt.CWGravel.AverageWidth, rpt.AvgWidth, rpt.CWGravel.FsdGrpCode, rpt.CWGravel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWGravel.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWGravel.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWGravel.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWGravel.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWGravel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWGravel.Remarks;

                        //worksheet.Cell(i, 11).Value = "A";
                        //worksheet.Cell(i, 12).Value = "5.3";
                        //worksheet.Cell(i, 14).Value = "0.50";
                        //worksheet.Cell(i, 17).Value = "0.10";
                        //worksheet.Cell(i, 18).Value = "0.20";
                        //worksheet.Cell(i, 19).Value = "0.20";
                        //worksheet.Cell(i, 20).Value = "N";
                        //worksheet.Cell(i, 22).Value = "R";

                        i = 16;
                        AverageWidth = GetAverageWidth(rpt.CWEarth.FsdFeature, rpt.CWEarth.AverageWidth, rpt.AvgWidth, rpt.CWEarth.FsdGrpCode, rpt.CWEarth.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWEarth.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWEarth.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWEarth.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWEarth.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWEarth.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWEarth.Remarks;
                        i = 17;
                        AverageWidth = GetAverageWidth(rpt.CWSand.FsdFeature, rpt.CWSand.AverageWidth, rpt.AvgWidth, rpt.CWSand.FsdGrpCode, rpt.CWSand.FsdGrpType);
                        worksheet.Cell(i, 11).Value = getClassCategoryByWidth(AverageWidth);
                        worksheet.Cell(i, 12).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CWSand.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CWSand.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CWSand.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CWSand.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CWSand.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CWSand.Remarks;
                        i = 18;
                        AverageWidth = GetAverageWidth(rpt.CLMPaint.FsdFeature, rpt.CLMPaint.AverageWidth, rpt.AvgWidth, rpt.CLMPaint.FsdGrpCode, rpt.CLMPaint.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CLMPaint.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CLMPaint.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CLMPaint.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CLMPaint.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CLMPaint.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CLMPaint.Remarks;
                        i = 19;
                        AverageWidth = GetAverageWidth(rpt.CLMThermoplastic.FsdFeature, rpt.CLMThermoplastic.AverageWidth, rpt.AvgWidth, rpt.CLMThermoplastic.FsdGrpCode, rpt.CLMThermoplastic.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CLMThermoplastic.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.CLMThermoplastic.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.CLMThermoplastic.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.CLMThermoplastic.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.CLMThermoplastic.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CLMThermoplastic.Remarks;
                        i = 20;
                        AverageWidth = GetAverageWidth(rpt.LELMPaint.FsdFeature, rpt.LELMPaint.AverageWidth, rpt.AvgWidth, rpt.LELMPaint.FsdGrpCode, rpt.LELMPaint.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LELMPaint.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LELMPaint.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LELMPaint.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LELMPaint.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LELMPaint.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LELMPaint.Remarks;
                        i = 21;
                        AverageWidth = GetAverageWidth(rpt.LELMThermoplastic.FsdFeature, rpt.LELMThermoplastic.AverageWidth, rpt.AvgWidth, rpt.LELMThermoplastic.FsdGrpCode, rpt.LELMThermoplastic.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LELMThermoplastic.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LELMThermoplastic.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LELMThermoplastic.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LELMThermoplastic.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LELMThermoplastic.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LELMThermoplastic.Remarks;
                        i = 22;
                        AverageWidth = GetAverageWidth(rpt.LDitchGravel.FsdFeature, rpt.LDitchGravel.AverageWidth, rpt.AvgWidth, rpt.LDitchGravel.FsdGrpCode, rpt.LDitchGravel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LDitchGravel.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LDitchGravel.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LDitchGravel.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LDitchGravel.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LDitchGravel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LDitchGravel.Remarks;
                        i = 23;
                        AverageWidth = GetAverageWidth(rpt.LDrainEarth.FsdFeature, rpt.LDrainEarth.AverageWidth, rpt.AvgWidth, rpt.LDrainEarth.FsdGrpCode, rpt.LDrainEarth.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LDrainEarth.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LDrainEarth.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LDrainEarth.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LDrainEarth.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LDrainEarth.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LDrainEarth.Remarks;
                        i = 24;
                        AverageWidth = GetAverageWidth(rpt.LDrainBlockstone.FsdFeature, rpt.LDrainBlockstone.AverageWidth, rpt.AvgWidth, rpt.LDrainBlockstone.FsdGrpCode, rpt.LDrainBlockstone.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LDrainBlockstone.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LDrainBlockstone.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LDrainBlockstone.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LDrainBlockstone.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LDrainBlockstone.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LDrainBlockstone.Remarks;
                        i = 25;
                        AverageWidth = GetAverageWidth(rpt.LDrainConcreate.FsdFeature, rpt.LDrainConcreate.AverageWidth, rpt.AvgWidth, rpt.LDrainConcreate.FsdGrpCode, rpt.LDrainConcreate.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LDrainConcreate.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LDrainConcreate.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LDrainConcreate.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LDrainConcreate.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LDrainConcreate.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LDrainConcreate.Remarks;
                        i = 26;
                        AverageWidth = GetAverageWidth(rpt.LShoulderAsphalt.FsdFeature, rpt.LShoulderAsphalt.AverageWidth, rpt.AvgWidth, rpt.LShoulderAsphalt.FsdGrpCode, rpt.LShoulderAsphalt.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LShoulderAsphalt.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LShoulderAsphalt.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LShoulderAsphalt.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LShoulderAsphalt.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LShoulderAsphalt.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LShoulderAsphalt.Remarks;
                        i = 27;
                        AverageWidth = GetAverageWidth(rpt.LShoulderConcrete.FsdFeature, rpt.LShoulderConcrete.AverageWidth, rpt.AvgWidth, rpt.LShoulderConcrete.FsdGrpCode, rpt.LShoulderConcrete.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LShoulderConcrete.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LShoulderConcrete.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LShoulderConcrete.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LShoulderConcrete.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LShoulderConcrete.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LShoulderConcrete.Remarks;
                        i = 28;
                        AverageWidth = GetAverageWidth(rpt.LShoulderEarth.FsdFeature, rpt.LShoulderEarth.AverageWidth, rpt.AvgWidth, rpt.LShoulderEarth.FsdGrpCode, rpt.LShoulderEarth.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LShoulderEarth.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LShoulderEarth.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LShoulderEarth.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LShoulderEarth.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LShoulderEarth.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LShoulderEarth.Remarks;
                        i = 29;
                        AverageWidth = GetAverageWidth(rpt.LShoulderGravel.FsdFeature, rpt.LShoulderGravel.AverageWidth, rpt.AvgWidth, rpt.LShoulderGravel.FsdGrpCode, rpt.LShoulderGravel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LShoulderGravel.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LShoulderGravel.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LShoulderGravel.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LShoulderGravel.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LShoulderGravel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LShoulderGravel.Remarks;
                        i = 30;
                        AverageWidth = GetAverageWidth(rpt.LShoulderFootpathkerb.FsdFeature, rpt.LShoulderFootpathkerb.AverageWidth, rpt.AvgWidth, rpt.LShoulderFootpathkerb.FsdGrpCode, rpt.LShoulderFootpathkerb.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.LShoulderFootpathkerb.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.LShoulderFootpathkerb.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.LShoulderFootpathkerb.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.LShoulderFootpathkerb.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.LShoulderFootpathkerb.Needed;
                        worksheet.Cell(i, 22).Value = rpt.LShoulderFootpathkerb.Remarks;


                        i = 31;
                        AverageWidth = GetAverageWidth(rpt.RELMPaint.FsdFeature, rpt.RELMPaint.AverageWidth, rpt.AvgWidth, rpt.RELMPaint.FsdGrpCode, rpt.RELMPaint.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RELMPaint.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RELMPaint.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RELMPaint.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RELMPaint.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RELMPaint.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RELMPaint.Remarks;
                        i = 32;
                        AverageWidth = GetAverageWidth(rpt.RELMThermoplastic.FsdFeature, rpt.RELMThermoplastic.AverageWidth, rpt.AvgWidth, rpt.RELMThermoplastic.FsdGrpCode, rpt.RELMThermoplastic.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RELMThermoplastic.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RELMThermoplastic.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RELMThermoplastic.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RELMThermoplastic.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RELMThermoplastic.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RELMThermoplastic.Remarks;
                        i = 33;
                        AverageWidth = GetAverageWidth(rpt.RDitchGravel.FsdFeature, rpt.RDitchGravel.AverageWidth, rpt.AvgWidth, rpt.RDitchGravel.FsdGrpCode, rpt.RDitchGravel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RDitchGravel.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RDitchGravel.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RDitchGravel.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RDitchGravel.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RDitchGravel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RDitchGravel.Remarks;
                        i = 34;
                        AverageWidth = GetAverageWidth(rpt.RDrainEarth.FsdFeature, rpt.RDrainEarth.AverageWidth, rpt.AvgWidth, rpt.RDrainEarth.FsdGrpCode, rpt.RDrainEarth.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RDrainEarth.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RDrainEarth.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RDrainEarth.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RDrainEarth.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RDrainEarth.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RDrainEarth.Remarks;
                        i = 35;
                        AverageWidth = GetAverageWidth(rpt.RDrainBlockstone.FsdFeature, rpt.RDrainBlockstone.AverageWidth, rpt.AvgWidth, rpt.RDrainBlockstone.FsdGrpCode, rpt.RDrainBlockstone.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RDrainBlockstone.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RDrainBlockstone.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RDrainBlockstone.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RDrainBlockstone.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RDrainBlockstone.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RDrainBlockstone.Remarks;
                        i = 36;
                        AverageWidth = GetAverageWidth(rpt.RDrainConcreate.FsdFeature, rpt.RDrainConcreate.AverageWidth, rpt.AvgWidth, rpt.RDrainConcreate.FsdGrpCode, rpt.RDrainConcreate.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RDrainConcreate.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RDrainConcreate.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RDrainConcreate.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RDrainConcreate.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RDrainConcreate.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RDrainConcreate.Remarks;
                        i = 37;
                        AverageWidth = GetAverageWidth(rpt.RShoulderAsphalt.FsdFeature, rpt.RShoulderAsphalt.AverageWidth, rpt.AvgWidth, rpt.RShoulderAsphalt.FsdGrpCode, rpt.RShoulderAsphalt.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RShoulderAsphalt.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RShoulderAsphalt.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RShoulderAsphalt.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RShoulderAsphalt.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RShoulderAsphalt.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RShoulderAsphalt.Remarks;
                        i = 38;
                        AverageWidth = GetAverageWidth(rpt.RShoulderConcrete.FsdFeature, rpt.RShoulderConcrete.AverageWidth, rpt.AvgWidth, rpt.RShoulderConcrete.FsdGrpCode, rpt.RShoulderConcrete.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RShoulderConcrete.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RShoulderConcrete.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RShoulderConcrete.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RShoulderConcrete.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RShoulderConcrete.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RShoulderConcrete.Remarks;
                        i = 39;
                        AverageWidth = GetAverageWidth(rpt.RShoulderEarth.FsdFeature, rpt.RShoulderEarth.AverageWidth, rpt.AvgWidth, rpt.RShoulderEarth.FsdGrpCode, rpt.RShoulderEarth.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RShoulderEarth.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RShoulderEarth.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RShoulderEarth.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RShoulderEarth.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RShoulderEarth.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RShoulderEarth.Remarks;
                        i = 40;
                        AverageWidth = GetAverageWidth(rpt.RShoulderGravel.FsdFeature, rpt.RShoulderGravel.AverageWidth, rpt.AvgWidth, rpt.RShoulderGravel.FsdGrpCode, rpt.RShoulderGravel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RShoulderGravel.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RShoulderGravel.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RShoulderGravel.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RShoulderGravel.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RShoulderGravel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RShoulderGravel.Remarks;
                        i = 41;
                        AverageWidth = GetAverageWidth(rpt.RShoulderFootpathkerb.FsdFeature, rpt.RShoulderFootpathkerb.AverageWidth, rpt.AvgWidth, rpt.RShoulderFootpathkerb.FsdGrpCode, rpt.RShoulderFootpathkerb.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RShoulderFootpathkerb.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RShoulderFootpathkerb.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RShoulderFootpathkerb.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RShoulderFootpathkerb.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RShoulderFootpathkerb.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RShoulderFootpathkerb.Remarks;
                        i = 42;
                        AverageWidth = GetAverageWidth(rpt.RSLeft.FsdFeature, rpt.RSLeft.AverageWidth, rpt.AvgWidth, rpt.RSLeft.FsdGrpCode, rpt.RSLeft.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RSLeft.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RSLeft.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RSLeft.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RSLeft.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RSLeft.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RSLeft.Remarks;
                        i = 43;
                        AverageWidth = GetAverageWidth(rpt.RSCenter.FsdFeature, rpt.RSCenter.AverageWidth, rpt.AvgWidth, rpt.RSCenter.FsdGrpCode, rpt.RSCenter.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RSCenter.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RSCenter.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RSCenter.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RSCenter.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RSCenter.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RSCenter.Remarks;
                        i = 44;
                        AverageWidth = GetAverageWidth(rpt.RSRight.FsdFeature, rpt.RSRight.AverageWidth, rpt.AvgWidth, rpt.RSRight.FsdGrpCode, rpt.RSRight.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RSRight.TotalLength / 10;
                        worksheet.Cell(i, 17).Value = rpt.RSRight.Condition1 / 10;
                        worksheet.Cell(i, 18).Value = rpt.RSRight.Condition2 / 10;
                        worksheet.Cell(i, 19).Value = rpt.RSRight.Condition3 / 10;
                        worksheet.Cell(i, 20).Value = rpt.RSRight.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RSRight.Remarks;
                        i = 45;
                        AverageWidth = GetAverageWidth(rpt.SignsDelineator.FsdFeature, rpt.SignsDelineator.AverageWidth, rpt.AvgWidth, rpt.SignsDelineator.FsdGrpCode, rpt.SignsDelineator.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.SignsDelineator.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.SignsDelineator.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.SignsDelineator.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.SignsDelineator.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.SignsDelineator.Needed;
                        worksheet.Cell(i, 22).Value = rpt.SignsDelineator.Remarks;
                        i = 46;
                        AverageWidth = GetAverageWidth(rpt.SignsWarning.FsdFeature, rpt.SignsWarning.AverageWidth, rpt.AvgWidth, rpt.SignsWarning.FsdGrpCode, rpt.SignsWarning.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.SignsWarning.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.SignsWarning.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.SignsWarning.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.SignsWarning.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.SignsWarning.Needed;
                        worksheet.Cell(i, 22).Value = rpt.SignsWarning.Remarks;
                        i = 47;
                        AverageWidth = GetAverageWidth(rpt.SignsGantrySign.FsdFeature, rpt.SignsGantrySign.AverageWidth, rpt.AvgWidth, rpt.SignsGantrySign.FsdGrpCode, rpt.SignsGantrySign.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.SignsGantrySign.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.SignsGantrySign.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.SignsGantrySign.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.SignsGantrySign.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.SignsGantrySign.Needed;
                        worksheet.Cell(i, 22).Value = rpt.SignsGantrySign.Remarks;
                        i = 48;
                        AverageWidth = GetAverageWidth(rpt.SignsGuideSign.FsdFeature, rpt.SignsGuideSign.AverageWidth, rpt.AvgWidth, rpt.SignsGuideSign.FsdGrpCode, rpt.SignsGuideSign.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.SignsGuideSign.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.SignsGuideSign.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.SignsGuideSign.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.SignsGuideSign.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.SignsGuideSign.Needed;
                        worksheet.Cell(i, 22).Value = rpt.SignsGuideSign.Remarks;
                        i = 49;
                        AverageWidth = GetAverageWidth(rpt.CVConcreatePipe.FsdFeature, rpt.CVConcreatePipe.AverageWidth, rpt.AvgWidth, rpt.CVConcreatePipe.FsdGrpCode, rpt.CVConcreatePipe.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CVConcreatePipe.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.CVConcreatePipe.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.CVConcreatePipe.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.CVConcreatePipe.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.CVConcreatePipe.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CVConcreatePipe.Remarks;
                        i = 50;
                        AverageWidth = GetAverageWidth(rpt.CVConcreteBox.FsdFeature, rpt.CVConcreteBox.AverageWidth, rpt.AvgWidth, rpt.CVConcreteBox.FsdGrpCode, rpt.CVConcreteBox.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CVConcreteBox.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.CVConcreteBox.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.CVConcreteBox.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.CVConcreteBox.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.CVConcreteBox.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CVConcreteBox.Remarks;

                        i = 51;
                        AverageWidth = GetAverageWidth(rpt.CVMetal.FsdFeature, rpt.CVMetal.AverageWidth, rpt.AvgWidth, rpt.CVMetal.FsdGrpCode, rpt.CVMetal.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CVMetal.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.CVMetal.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.CVMetal.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.CVMetal.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.CVMetal.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CVMetal.Remarks;
                        i = 52;
                        AverageWidth = GetAverageWidth(rpt.CVHDPE.FsdFeature, rpt.CVHDPE.AverageWidth, rpt.AvgWidth, rpt.CVHDPE.FsdGrpCode, rpt.CVHDPE.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CVHDPE.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.CVHDPE.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.CVHDPE.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.CVHDPE.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.CVHDPE.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CVHDPE.Remarks;
                        i = 53;
                        AverageWidth = GetAverageWidth(rpt.CVOthers.FsdFeature, rpt.CVOthers.AverageWidth, rpt.AvgWidth, rpt.CVOthers.FsdGrpCode, rpt.CVOthers.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.CVOthers.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.CVOthers.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.CVOthers.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.CVOthers.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.CVOthers.Needed;
                        worksheet.Cell(i, 22).Value = rpt.CVOthers.Remarks;
                        i = 54;
                        AverageWidth = GetAverageWidth(rpt.BRConcConc.FsdFeature, rpt.BRConcConc.AverageWidth, rpt.AvgWidth, rpt.BRConcConc.FsdGrpCode, rpt.BRConcConc.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRConcConc.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRConcConc.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRConcConc.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRConcConc.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRConcConc.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRConcConc.Remarks;
                        i = 55;
                        AverageWidth = GetAverageWidth(rpt.BRConcSteel.FsdFeature, rpt.BRConcSteel.AverageWidth, rpt.AvgWidth, rpt.BRConcSteel.FsdGrpCode, rpt.BRConcSteel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRConcSteel.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRConcSteel.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRConcSteel.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRConcSteel.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRConcSteel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRConcSteel.Remarks;
                        i = 56;
                        AverageWidth = GetAverageWidth(rpt.BRSteelTimber.FsdFeature, rpt.BRSteelTimber.AverageWidth, rpt.AvgWidth, rpt.BRSteelTimber.FsdGrpCode, rpt.BRSteelTimber.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRSteelTimber.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRSteelTimber.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRSteelTimber.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRSteelTimber.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRSteelTimber.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRSteelTimber.Remarks;
                        i = 57;
                        AverageWidth = GetAverageWidth(rpt.BRSteelSteel.FsdFeature, rpt.BRSteelSteel.AverageWidth, rpt.AvgWidth, rpt.BRSteelSteel.FsdGrpCode, rpt.BRSteelSteel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRSteelSteel.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRSteelSteel.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRSteelSteel.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRSteelSteel.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRSteelSteel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRSteelSteel.Remarks;
                        i = 58;
                        AverageWidth = GetAverageWidth(rpt.BRTimberTimber.FsdFeature, rpt.BRTimberTimber.AverageWidth, rpt.AvgWidth, rpt.BRTimberTimber.FsdGrpCode, rpt.BRTimberTimber.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRTimberTimber.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRTimberTimber.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRTimberTimber.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRTimberTimber.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRTimberTimber.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRTimberTimber.Remarks;

                        i = 59;
                        AverageWidth = GetAverageWidth(rpt.BRTimberSteel.FsdFeature, rpt.BRTimberSteel.AverageWidth, rpt.AvgWidth, rpt.BRTimberSteel.FsdGrpCode, rpt.BRTimberSteel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRTimberSteel.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRTimberSteel.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRTimberSteel.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRTimberSteel.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRTimberSteel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRTimberSteel.Remarks;
                        i = 60;
                        AverageWidth = GetAverageWidth(rpt.BRMansonry.FsdFeature, rpt.BRMansonry.AverageWidth, rpt.AvgWidth, rpt.BRMansonry.FsdGrpCode, rpt.BRMansonry.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRMansonry.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRMansonry.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRMansonry.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRMansonry.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRMansonry.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRMansonry.Remarks;
                        i = 61;
                        AverageWidth = GetAverageWidth(rpt.BRElevatedViaduct.FsdFeature, rpt.BRElevatedViaduct.AverageWidth, rpt.AvgWidth, rpt.BRElevatedViaduct.FsdGrpCode, rpt.BRElevatedViaduct.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRElevatedViaduct.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRElevatedViaduct.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRElevatedViaduct.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRElevatedViaduct.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRElevatedViaduct.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRElevatedViaduct.Remarks;
                        i = 62;
                        AverageWidth = GetAverageWidth(rpt.BRLongBridge.FsdFeature, rpt.BRLongBridge.AverageWidth, rpt.AvgWidth, rpt.BRLongBridge.FsdGrpCode, rpt.BRLongBridge.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.BRLongBridge.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.BRLongBridge.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.BRLongBridge.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.BRLongBridge.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.BRLongBridge.Needed;
                        worksheet.Cell(i, 22).Value = rpt.BRLongBridge.Remarks;
                        i = 63;
                        AverageWidth = GetAverageWidth(rpt.GRSteel.FsdFeature, rpt.GRSteel.AverageWidth, rpt.AvgWidth, rpt.GRSteel.FsdGrpCode, rpt.GRSteel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.GRSteel.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.GRSteel.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.GRSteel.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.GRSteel.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.GRSteel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.GRSteel.Remarks;
                        i = 64;
                        AverageWidth = GetAverageWidth(rpt.GRWire.FsdFeature, rpt.GRWire.AverageWidth, rpt.AvgWidth, rpt.GRWire.FsdGrpCode, rpt.GRWire.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.GRWire.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.GRWire.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.GRWire.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.GRWire.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.GRWire.Needed;
                        worksheet.Cell(i, 22).Value = rpt.GRWire.Remarks;
                        i = 65;
                        AverageWidth = GetAverageWidth(rpt.GRPedestrialRailing.FsdFeature, rpt.GRPedestrialRailing.AverageWidth, rpt.AvgWidth, rpt.GRPedestrialRailing.FsdGrpCode, rpt.GRPedestrialRailing.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.GRPedestrialRailing.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.GRPedestrialRailing.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.GRPedestrialRailing.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.GRPedestrialRailing.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.GRPedestrialRailing.Needed;
                        worksheet.Cell(i, 22).Value = rpt.GRPedestrialRailing.Remarks;
                        i = 66;
                        AverageWidth = GetAverageWidth(rpt.GRParapetWall.FsdFeature, rpt.GRParapetWall.AverageWidth, rpt.AvgWidth, rpt.GRParapetWall.FsdGrpCode, rpt.GRParapetWall.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.GRParapetWall.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.GRParapetWall.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.GRParapetWall.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.GRParapetWall.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.GRParapetWall.Needed;
                        worksheet.Cell(i, 22).Value = rpt.GRParapetWall.Remarks;
                        i = 67;
                        AverageWidth = GetAverageWidth(rpt.GROthers.FsdFeature, rpt.GROthers.AverageWidth, rpt.AvgWidth, rpt.GROthers.FsdGrpCode, rpt.GROthers.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.GROthers.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.GROthers.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.GROthers.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.GROthers.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.GROthers.Needed;
                        worksheet.Cell(i, 22).Value = rpt.GROthers.Remarks;
                        i = 68;
                        AverageWidth = GetAverageWidth(rpt.RWReinforceConc.FsdFeature, rpt.RWReinforceConc.AverageWidth, rpt.AvgWidth, rpt.RWReinforceConc.FsdGrpCode, rpt.RWReinforceConc.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWReinforceConc.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWReinforceConc.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWReinforceConc.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWReinforceConc.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWReinforceConc.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWReinforceConc.Remarks;
                        i = 69;
                        AverageWidth = GetAverageWidth(rpt.RWSteelMetal.FsdFeature, rpt.RWSteelMetal.AverageWidth, rpt.AvgWidth, rpt.RWSteelMetal.FsdGrpCode, rpt.RWSteelMetal.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWSteelMetal.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWSteelMetal.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWSteelMetal.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWSteelMetal.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWSteelMetal.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWSteelMetal.Remarks;
                        i = 70;
                        AverageWidth = GetAverageWidth(rpt.RWMasonryGabion.FsdFeature, rpt.RWMasonryGabion.AverageWidth, rpt.AvgWidth, rpt.RWMasonryGabion.FsdGrpCode, rpt.RWMasonryGabion.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWMasonryGabion.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWMasonryGabion.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWMasonryGabion.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWMasonryGabion.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWMasonryGabion.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWMasonryGabion.Remarks;
                        i = 71;
                        AverageWidth = GetAverageWidth(rpt.RWPrecastPanel.FsdFeature, rpt.RWPrecastPanel.AverageWidth, rpt.AvgWidth, rpt.RWPrecastPanel.FsdGrpCode, rpt.RWPrecastPanel.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWPrecastPanel.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWPrecastPanel.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWPrecastPanel.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWPrecastPanel.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWPrecastPanel.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWPrecastPanel.Remarks;
                        i = 72;
                        AverageWidth = GetAverageWidth(rpt.RWTimber.FsdFeature, rpt.RWTimber.AverageWidth, rpt.AvgWidth, rpt.RWTimber.FsdGrpCode, rpt.RWTimber.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWTimber.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWTimber.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWTimber.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWTimber.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWTimber.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWTimber.Remarks;
                        i = 73;
                        AverageWidth = GetAverageWidth(rpt.RWSoliNail.FsdFeature, rpt.RWSoliNail.AverageWidth, rpt.AvgWidth, rpt.RWSoliNail.FsdGrpCode, rpt.RWSoliNail.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWSoliNail.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWSoliNail.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWSoliNail.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWSoliNail.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWSoliNail.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWSoliNail.Remarks;
                        i = 74;
                        AverageWidth = GetAverageWidth(rpt.RWOthers.FsdFeature, rpt.RWOthers.AverageWidth, rpt.AvgWidth, rpt.RWOthers.FsdGrpCode, rpt.RWOthers.FsdGrpType);
                        worksheet.Cell(i, 11).Value = AverageWidth;
                        worksheet.Cell(i, 14).Value = rpt.RWOthers.TotalLength;
                        worksheet.Cell(i, 17).Value = rpt.RWOthers.Condition1;
                        worksheet.Cell(i, 18).Value = rpt.RWOthers.Condition2;
                        worksheet.Cell(i, 19).Value = rpt.RWOthers.Condition3;
                        worksheet.Cell(i, 20).Value = rpt.RWOthers.Needed;
                        worksheet.Cell(i, 22).Value = rpt.RWOthers.Remarks;


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

        public async Task<List<FormFSDetailRequestDTO>> GetRecordList(int headerId)
        {
            return await _repoUnit.FormFSDetailRepository.GetRecordList(headerId);
        }
        private string getClassCategoryByWidth(double? avgwidth)
        {
            string classCategory = "";
            if (avgwidth > Convert.ToDouble(7.5))
            {
                classCategory = "A";
            }
            else if (avgwidth <= Convert.ToDouble(7.5) && avgwidth > Convert.ToDouble(6.5))
            {
                classCategory = "B";
            }
            else if (avgwidth <= Convert.ToDouble(6.5) && avgwidth > Convert.ToDouble(5.5))
            {
                classCategory = "C";
            }
            else if (avgwidth <= Convert.ToDouble(5.5) && avgwidth > Convert.ToDouble(5))
            {
                classCategory = "D";
            }
            else if (avgwidth <= Convert.ToDouble(5) && avgwidth > Convert.ToDouble(4.5))
            {
                classCategory = "E";
            }
            else if (avgwidth <= Convert.ToDouble(4.5) && avgwidth > Convert.ToDouble(0))
            {
                classCategory = "F";
            }
            return classCategory;
        }
    }
}

