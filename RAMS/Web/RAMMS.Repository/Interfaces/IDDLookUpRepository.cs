using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Domain.Models;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.ResponseBO.DLP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository.Interfaces
{
    public interface IDDLookUpRepository : IRepositoryBase<RmDdLookup>
    {


        Task<IEnumerable<RmIwSrvProviderMasterDTO>> LoadServiceProviderName();
        Task<IEnumerable<RoadMasterResponseDTO>> GetRMUwithDivisionDetails();
        Task<IEnumerable<RmDdLookup>> GetDdLookUp(DDLookUpDTO rmDdLookup);
        Task<string> GetDesc(DDLookUpDTO rmDdLookup);
        Task<IEnumerable<RmDdLookup>> GetRMUBasedSection(DDLookUpDTO dDLookUp);
        Task<IEnumerable<SelectListItem>> GetDefCode();
        FormAssetTypesDTO GetFormAssetTypes(string typeCode);
        Task<List<UvwSearchData>> GlobalSearchData(string keyWord);
        string GetConcatenateDdlTypeDesc(DDLookUpDTO dto);
        string GetConcatenateDdlTypeValue(DDLookUpDTO dto);
        Task<IEnumerable<RmFormRDistressDetails>> GetDdDistressDetails();

        #region DLP SP
        Task<List<RMSPPLPDTO>> getRMSPPLPData(string keyWord);
        Task<List<RMDlpSpiDTO>> getDLPSPSCurveData(string keyWord);
        Task<List<int>> GetDLPSPYears();
        #endregion

        #region RMI & IRI
        Task<List<DlpIRIDTO>> getRMIIRIData(int year);

        Task<List<int>> GetIRIYears();

        #endregion
        Task<IEnumerable<FormAHeaderRequestDTO>> GetDdYearDetails();
        Task<IEnumerable<FormAHeaderRequestDTO>> GetDdRMUDetails();
    }
}
