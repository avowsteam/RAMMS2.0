using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;

namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IDlpSpiService
    {
        Task<List<DlpSPIDTO>> GetDivisionMiri(int year);

        Task<List<DlpSPIDTO>> GetDivisionRMUMiri(int year);

        Task<List<DlpSPIDTO>> GetDivisionRMUBTN(int year);

        Task<int> Save(List<SpiData> spiData);

        #region RMI IRI
        Task<int> SaveIRI(List<DlpIRIDTO> spiData);

        Task<PagingResult<DlpIRIDTO>> GetFilteredFormAGrid(FilteredPagingDefinition<FormASearchGridDTO> filterOptions);

        Task<DlpIRIDTO> GetIRIData(int year);
        #endregion

    }
}
