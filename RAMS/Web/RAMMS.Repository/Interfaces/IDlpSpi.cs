using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;

namespace RAMMS.Repository.Interfaces
{
    public interface IDlpSpi : IRepositoryBase<RmDlpSpi>
    {
        Task<List<RmDlpSpi>> GetDivisionMiri(int year);

        Task<List<RmDlpSpi>> GetDivisionRMUMiri(int year);

        Task<List<RmDlpSpi>> GetDivisionRMUBTN(int year);

        Task<int> Save(List<SpiData> spiData);

        Task<int> SyncBTN(int year);

        Task<int> SyncMiri(int year);

        #region RMI & IRI
        Task<int> SaveIRI(List<DlpIRIDTO> model);
        Task<List<RmRmiIri>> GetIRIData(int year);
        #endregion
    }
}
