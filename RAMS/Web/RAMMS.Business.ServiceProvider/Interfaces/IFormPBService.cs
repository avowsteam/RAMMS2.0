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
    public interface IFormPBService
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormPBHeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormPBSearchGridDTO> filterOptions);

        Task<FormPBHeaderResponseDTO> GetHeaderById(int id);
        Task<int> SaveFormPB(FormPBHeaderResponseDTO FormPB);
       
        Task<int> UpdateFormPB(FormPBHeaderResponseDTO FormPBHeader, List<FormPBDetailResponseDTO> FormPBDetail);
        int? DeleteFormPB(int id);

        Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
