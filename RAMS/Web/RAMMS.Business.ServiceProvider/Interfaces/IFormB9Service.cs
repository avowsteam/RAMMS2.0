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
    public interface IFormB9Service
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormB9ResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormB9SearchGridDTO> filterOptions)


      //  Task<FormF1ResponseDTO> GetHeaderById(int id);

        Task<int> SaveFormB9(FormB9ResponseDTO FormB9, List<FormB9HistoryResponseDTO> FormB9History);

        //      Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
