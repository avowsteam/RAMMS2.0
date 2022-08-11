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

        public async Task<FormB11DTO> GetHeaderById(int id)
        {
            RmB11Hdr res = _repo.GetHeaderById(id);
            FormB11DTO FormB11 = new FormB11DTO();
            FormB11 = _mapper.Map<FormB11DTO>(res);
            FormB11.RmB11CrewDayCostHeader = _mapper.Map<List<FormB11CrewDayCostHeaderDTO>>(res.RmB11CrewDayCostHeader);
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
    }
}
