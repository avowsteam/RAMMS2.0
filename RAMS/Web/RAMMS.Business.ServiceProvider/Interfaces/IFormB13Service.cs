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
    public interface IFormB13Service
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormB13ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB13SearchGridDTO> filterOptions);

        Task<FormB13ResponseDTO> GetHeaderById(int id);
        Task<FormB13ResponseDTO> SaveFormB13(FormB13ResponseDTO FormB13);
        int? GetMaxRev(int Year, String RMU);

        Task<int> UpdateFormB13(FormB13ResponseDTO FormB13, List<FormB13HistoryResponseDTO> FormB13History);
        int? DeleteFormB13(int id);

        Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
