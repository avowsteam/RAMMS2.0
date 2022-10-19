using System;
using RAMMS.Business.ServiceProvider.Interfaces;
using System.Linq;
using RAMMS.Repository.Interfaces;
using AutoMapper;
using RAMMS.DTO.RequestBO;
using System.Threading.Tasks;
using RAMMS.DTO.Wrappers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO.ResponseBO;
using RAMMS.Domain.Models;

namespace RAMMS.Business.ServiceProvider.Services
{

    public class DlpSpiService : Interfaces.IDlpSpiService
    {
        private readonly IRepositoryUnit _repoUnit; 
        private readonly IMapper _mapper; 
        private readonly ISecurity _security;
        private readonly IDlpSpi _repo;
        public DlpSpiService(IRepositoryUnit repoUnit, IMapper mapper, ISecurity security, IDlpSpi repo)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit)); 
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
            _security = security ?? throw new ArgumentNullException(nameof(security));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<DlpSPIDTO>> GetDivisionMiri(int year)
        {
            List<RmDlpSpi> res = await _repo.GetDivisionMiri(year);
            List < DlpSPIDTO> Dlp = new List<DlpSPIDTO>();
            Dlp = _mapper.Map<List<DlpSPIDTO>>(res);
            return Dlp;
        }

        public async Task<List<DlpSPIDTO>> GetDivisionRMUMiri(int year)
        {
            List<RmDlpSpi> res = await _repo.GetDivisionRMUMiri(year);
            List<DlpSPIDTO> Dlp = new List<DlpSPIDTO>();
            Dlp = _mapper.Map<List<DlpSPIDTO>>(res);
            return Dlp;
        }

        public async Task<List<DlpSPIDTO>> GetDivisionRMUBTN(int year)
        {
            List<RmDlpSpi> res = await _repo.GetDivisionRMUBTN(year);
            List<DlpSPIDTO> Dlp = new List<DlpSPIDTO>();
            Dlp = _mapper.Map<List<DlpSPIDTO>>(res);
            return Dlp;
        }
    }
}
