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


    }
}
