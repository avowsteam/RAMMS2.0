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
    public interface IFormB10Service
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormB10ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB10SearchGridDTO> filterOptions);
       
        Task<FormB10ResponseDTO> GetHeaderById(int id);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB10(FormB10ResponseDTO FormB10, List<FormB10HistoryResponseDTO> FormB10History);

        Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
