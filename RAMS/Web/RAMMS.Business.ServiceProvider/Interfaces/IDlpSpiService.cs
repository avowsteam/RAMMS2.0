using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    }
}
