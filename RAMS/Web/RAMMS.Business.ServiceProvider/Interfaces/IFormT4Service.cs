using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IFormT4Service
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormT4HeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormT4SearchGridDTO> filterOptions);

        Task<FormT4HeaderResponseDTO> GetHeaderById(int id);
        Task<FormT4HeaderResponseDTO> SaveFormT4(FormT4HeaderResponseDTO FormT4);
        int? GetMaxRev(int Year, String RMU);

        Task<int> UpdateFormT4(FormT4HeaderResponseDTO FormT4, List<FormT4ResponseDTO> FormT4History);
      
        int? DeleteFormT4(int id);

        Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
