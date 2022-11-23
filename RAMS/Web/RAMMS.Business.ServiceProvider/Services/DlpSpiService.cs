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
using RAMMS.DTO;
using RAMMS.DTO.ResponseBO.IRI;
using RAMMS.DTO.JQueryModel;

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
            List<DlpSPIDTO> Dlp = new List<DlpSPIDTO>();
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

        public async Task<int> Save(List<SpiData> spiData)
        {
            return await _repo.Save(spiData);
        }


        public async Task<int> SyncBTN(int year)
        {
            return await _repo.SyncBTN(year);
        }

        public async Task<int> SyncMiri(int year)
        {
            return await _repo.SyncMiri(year);
        }

        #region RMI IRI

        public async Task<int> SaveIRI(List<DlpIRIDTO> model)
        {
            return await _repo.SaveIRI(model);
        }

        public async Task<PagingResult<DlpIRIDTO>> GetFilteredFormAGrid(FilteredPagingDefinition<FormASearchGridDTO> filterOptions)
        {
            PagingResult<DlpIRIDTO> result = new PagingResult<DlpIRIDTO>();

            List<DlpIRIDTO> iRIList = new List<DlpIRIDTO>();
            try
            {
                var filteredRecords = await _repoUnit.DlpSpi.GetFilteredRecordList(filterOptions);

                result.TotalRecords = await _repoUnit.DlpSpi.GetFilteredRecordCount(filterOptions).ConfigureAwait(false);

                var yearList = filteredRecords.Select(a => a.RmiiriYear).GroupBy(a => a.Value).ToList();

                foreach (var item in yearList)
                {
                    DlpIRIDTO model = new DlpIRIDTO();

                    foreach (var listData in filteredRecords.Where(a => a.RmiiriYear == item.Key).ToList())
                    {
                        if (listData.RmiiriType == "RMI")
                        {
                            model.RmiiriRoadLength = listData.RmiiriRoadLength;
                            model.RmiiriPercentage = listData.RmiiriPercentage;
                        }
                        if (listData.RmiiriType == "IRI")
                        {
                            switch (listData.RmiiriConditionNo.Value)
                            {
                                case 1:
                                    model.RmiiriPercentage1 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength1 = listData.RmiiriRoadLength;
                                    model._RmiiriPercentage1 = listData.RmiiriPercentage.Value.ToString();
                                    model._RmiiriRoadLength1 = listData.RmiiriRoadLength.Value.ToString();
                                    model._RmiiriRoadLength = listData.RmiiriRoadLength.Value.ToString();
                                    break;
                                case 2:
                                    model.RmiiriPercentage2 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength2 = listData.RmiiriRoadLength;
                                    model._RmiiriRoadLength2 = listData.RmiiriRoadLength.Value.ToString();
                                    model._RmiiriRoadLength = listData.RmiiriRoadLength.Value.ToString();
                                    break;
                                case 3:
                                    model.RmiiriPercentage3 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength3 = listData.RmiiriRoadLength;
                                    model._RmiiriRoadLength3 = listData.RmiiriRoadLength.Value.ToString();
                                    model._RmiiriRoadLength = listData.RmiiriRoadLength.Value.ToString();
                                    break;
                            }
                            model.RmiiriPkRefNo = listData.RmiiriPkRefNo;
                        }

                    }
                    model.RmiiriYear = item.Key;
                    iRIList.Add(model);

                }

                result.PageResult = iRIList;

                result.PageNo = filterOptions.StartPageNo;
                result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return result;
        }

        public async Task<DlpIRIDTO> GetIRIData(int year)
        {
            try
            {
                DlpIRIDTO model = new DlpIRIDTO();

                var data = _repo.GetIRIData(year);
                if (data != null)
                {
                    foreach (var listData in data.Result.ToList())
                    {
                        if (listData.RmiiriType == "RMI")
                        {
                            model.RmiiriRoadLength = listData.RmiiriRoadLength;
                            model.RmiiriPercentage = listData.RmiiriPercentage;
                        }
                        if (listData.RmiiriType == "IRI")
                        {
                            switch (listData.RmiiriConditionNo.Value)
                            {
                                case 1:
                                    model.RmiiriPercentage1 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength1 = listData.RmiiriRoadLength;
                                    break;
                                case 2:
                                    model.RmiiriPercentage2 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength2 = listData.RmiiriRoadLength;
                                    break;
                                case 3:
                                    model.RmiiriPercentage3 = listData.RmiiriPercentage;
                                    model.RmiiriRoadLength3 = listData.RmiiriRoadLength;
                                    break;
                            }
                            model.RmiiriPkRefNo = listData.RmiiriPkRefNo;
                        }

                    }
                    model.RmiiriYear = year;
                    return model;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? DeleteFormIRI(int id)
        {
            return _repo.DeleteFormIRI(id);
        }
        #endregion
    }
}
