using System;
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
    public class FormB11Service : IFormB11Service
    {
        private readonly IFormB11Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;
        public FormB11Service(IRepositoryUnit repoUnit, IFormB11Repository repo,
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

        public async Task<FormB11DTO> GetHeaderById(int id, int IsEdit)
        {
            RmB11Hdr res = _repo.GetHeaderById(id, IsEdit);
            FormB11DTO FormB11 = new FormB11DTO();
            FormB11 = _mapper.Map<FormB11DTO>(res);
            //FormB11.RmB11CrewDayCostHeader = _mapper.Map<List<FormB11CrewDayCostHeaderDTO>>(res.RmB11CrewDayCostHeader);
            FormB11.RmB11LabourCost = _mapper.Map<List<FormB11LabourCostDTO>>(res.RmB11LabourCost);
            FormB11.RmB11EquipmentCost = _mapper.Map<List<FormB11EquipmentCostDTO>>(res.RmB11EquipmentCost);
            FormB11.RmB11MaterialCost = _mapper.Map<List<FormB11MaterialCostDTO>>(res.RmB11MaterialCost);
            return FormB11;
        }
        public int? GetMaxRev(int Year)
        {
            return _repo.GetMaxRev(Year);
        }

        public async Task<List<FormB7LabourHistoryDTO>> GetLabourHistoryData(int year)
        {
            List<RmB7LabourHistory> res = _repo.GetLabourHistoryData(year);
            List<FormB7LabourHistoryDTO> FormB7 = new List<FormB7LabourHistoryDTO>();
            FormB7 = _mapper.Map<List<FormB7LabourHistoryDTO>>(res);
            return FormB7;
        }

        public async Task<List<FormB7MaterialHistoryDTO>> GetMaterialHistoryData(int year)
        {
            List<RmB7MaterialHistory> res = _repo.GetMaterialHistoryData(year);
            List<FormB7MaterialHistoryDTO> FormB7 = new List<FormB7MaterialHistoryDTO>();
            FormB7 = _mapper.Map<List<FormB7MaterialHistoryDTO>>(res);
            return FormB7;
        }

        public async Task<List<FormB7EquipmentsHistoryDTO>> GetEquipmentHistoryData(int year)
        {
            List<RmB7EquipmentsHistory> res = _repo.GetEquipmentHistoryData(year);
            List<FormB7EquipmentsHistoryDTO> FormB7 = new List<FormB7EquipmentsHistoryDTO>();
            FormB7 = _mapper.Map<List<FormB7EquipmentsHistoryDTO>>(res);
            return FormB7;
        }


        public async Task<List<FormB11LabourCostDTO>> GetLabourViewHistoryData(int id)
        {
            List<RmB11LabourCost> res = _repo.GetLabourViewHistoryData(id);
            List<FormB11LabourCostDTO> FormB11 = new List<FormB11LabourCostDTO>();
            FormB11 = _mapper.Map<List<FormB11LabourCostDTO>>(res);
            return FormB11;
        }

        public async Task<List<FormB11MaterialCostDTO>> GetMaterialViewHistoryData(int id)
        {
            List<RmB11MaterialCost> res = _repo.GetMaterialViewHistoryData(id);
            List<FormB11MaterialCostDTO> FormB11 = new List<FormB11MaterialCostDTO>();
            FormB11 = _mapper.Map<List<FormB11MaterialCostDTO>>(res);
            return FormB11;
        }

        public async Task<List<FormB11EquipmentCostDTO>> GetEquipmentViewHistoryData(int id)
        {
            List<RmB11EquipmentCost> res = _repo.GetEquipmentViewHistoryData(id);
            List<FormB11EquipmentCostDTO> FormB11 = new List<FormB11EquipmentCostDTO>();
            FormB11 = _mapper.Map<List<FormB11EquipmentCostDTO>>(res);
            return FormB11;
        }

        public async Task<int> SaveFormB11(FormB11DTO FormB11)
        {
            try
            {
                var domainModelFormB11 = _mapper.Map<RmB11Hdr>(FormB11);
                domainModelFormB11.B11hPkRefNo = 0;

                return await _repo.SaveFormB11(domainModelFormB11);
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public byte[] FormDownload(string formname, int id, string Rmucode, string basepath, string filepath)
        {
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
                List<RmB11LabourCost> lstLabourCost = _repo.GetLabourViewHistoryData(id);
                List<RmB11EquipmentCost> lstEquipmentCost = _repo.GetEquipmentViewHistoryData(id);
                List<RmB11MaterialCost> lstMaterialCost = _repo.GetMaterialViewHistoryData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);

                    using (var book = new XLWorkbook(cachefile))
                    {
                        if (worksheet != null)
                        {
                            if (Rmucode == "Miri")
                                worksheet.Cell(1, 1).Value = "APPENDIX B11B - CREW DAY COST CALCULATION WORK SHEET (RMU MIRI)";
                            else
                                worksheet.Cell(1, 1).Value = "APPENDIX B11A - CREW DAY COST CALCULATION WORK SHEET (RMU BATU NIAH)";
                            List<RmB11LabourCost> lstLC = lstLabourCost.Where(x => x.B11lcLabourOrderId == 0).OrderBy(x => x.B11lcLabourId).ToList();
                            for (int i = 0; i < lstLC.Count(); i++)
                            {
                                worksheet.Cell(3, (5 + i)).Value = lstLC[i].B11lcLabourName;
                                worksheet.Cell(4, (5 + i)).Value = lstLC[i].B11lcLabourPerUnitPrice;
                            }

                            List<RmB11EquipmentCost> lstEC = lstEquipmentCost.Where(x => x.B11ecEquipmentOrderId == 0).OrderBy(x => x.B11ecEquipmentId).ToList();
                            int echItem = lstLC.Count() + 6;
                            worksheet.Cell(5, (echItem - 1)).Value = "Sum of Labour Cost";
                            for (int i = 0; i < lstEC.Count(); i++)
                            {
                                worksheet.Cell(3, (echItem + i)).Value = lstEC[i].B11ecEquipmentName;
                                worksheet.Cell(4, (echItem + i)).Value = lstEC[i].B11ecEquipmentPerUnitPrice;
                            }

                            List<RmB11MaterialCost> lstMC = lstMaterialCost.Where(x => x.B11mcMaterialOrderId == 0).OrderBy(x => x.B11mcMaterialId).ToList();
                            int mcItem = lstEC.Count() + echItem + 1;
                            worksheet.Cell(5, (mcItem - 1)).Value = "Sum of Equipment Cost";
                            for (int i = 0; i < lstMC.Count(); i++)
                            {
                                worksheet.Cell(3, (mcItem + i)).Value = lstMC[i].B11mcMaterialName;
                                worksheet.Cell(4, (mcItem + i)).Value = lstMC[i].B11mcMaterialPerUnitPrice;
                            }
                            int mCost = lstMC.Count + mcItem;
                            worksheet.Cell(5, mCost).Value = "Sum of Material Cost";

                            #region Labour
                            int OrderLabCount = lstLabourCost.Select(x => x.B11lcLabourOrderId).Distinct().Count();
                            for (int i = 0; i < OrderLabCount; i++)
                            {
                                List<RmB11LabourCost> lstdataLC = lstLabourCost.Where(x => x.B11lcLabourOrderId == i).OrderBy(x => x.B11lcLabourId).ToList();
                                for (int k = 0; k < lstdataLC.Count(); k++)
                                {
                                    worksheet.Cell(6 + i, 5 + k).Value = lstdataLC[k].B11lcLabourTotalPrice;
                                }
                                worksheet.Cell(6 + i, (echItem - 1)).Value = lstdataLC.Sum(x => x.B11lcLabourTotalPrice);
                            }
                            #endregion

                            #region Equipment
                            int OrderEqCount = lstEquipmentCost.Select(x => x.B11ecEquipmentOrderId).Distinct().Count();
                            for (int i = 0; i < OrderEqCount; i++)
                            {
                                List<RmB11EquipmentCost> lstdataEC = lstEquipmentCost.Where(x => x.B11ecEquipmentOrderId == i).OrderBy(x => x.B11ecEquipmentId).ToList();
                                for (int k = 0; k < lstdataEC.Count(); k++)
                                {
                                    worksheet.Cell(6 + i, echItem + k).Value = lstdataEC[k].B11ecEquipmentTotalPrice;
                                }
                                worksheet.Cell(6 + i, (mcItem - 1)).Value = lstdataEC.Sum(x => x.B11ecEquipmentTotalPrice);
                            }
                            #endregion

                            #region Material
                            int OrderMCCount = lstMaterialCost.Select(x => x.B11mcMaterialOrderId).Distinct().Count();
                            for (int i = 0; i < OrderMCCount; i++)
                            {
                                List<RmB11MaterialCost> lstdataMC = lstMaterialCost.Where(x => x.B11mcMaterialOrderId == i).OrderBy(x => x.B11mcMaterialId).ToList();
                                for (int k = 0; k < lstdataMC.Count(); k++)
                                {
                                    worksheet.Cell(6 + i, mcItem + k).Value = lstdataMC[k].B11mcMaterialTotalPrice;
                                }
                                worksheet.Cell(6 + i, mCost).Value = lstdataMC.Sum(x => x.B11mcMaterialTotalPrice);
                            }
                            #endregion

                            #region Common Total
                            int CDCCount = lstEquipmentCost.Select(x => x.B11ecEquipmentOrderId).Distinct().Count();
                            for (int i = 0; i < CDCCount; i++)
                            {
                                List<RmB11LabourCost> lstdataLC = lstLabourCost.Where(x => x.B11lcLabourOrderId == i).OrderBy(x => x.B11lcLabourId).ToList();
                                List<RmB11EquipmentCost> lstdataEC = lstEquipmentCost.Where(x => x.B11ecEquipmentOrderId == i).OrderBy(x => x.B11ecEquipmentId).ToList();
                                List<RmB11MaterialCost> lstdataMC = lstMaterialCost.Where(x => x.B11mcMaterialOrderId == i).OrderBy(x => x.B11mcMaterialId).ToList();
                                worksheet.Cell(6 + i, 4).Value = lstdataLC.Sum(x => x.B11lcLabourTotalPrice) + lstdataEC.Sum(x => x.B11ecEquipmentTotalPrice) + lstdataMC.Sum(x => x.B11mcMaterialTotalPrice);
                            }
                            #endregion
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
