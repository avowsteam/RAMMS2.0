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
    public interface IFormPAService
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormPAHeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormPASearchGridDTO> filterOptions);

        Task<FormPAHeaderResponseDTO> GetHeaderById(int id);
        Task<int> SaveFormPA(FormPAHeaderResponseDTO FormPA);
       
        Task<int> UpdateFormPA(FormPAHeaderResponseDTO FormPAHeader, List<FormPACRRResponseDTO> FormPACrr, List<FormPACRRAResponseDTO> FormPACrra, List<FormPACRRDResponseDTO> FormPACrrd);
        int? DeleteFormPA(int id);

      //  Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
