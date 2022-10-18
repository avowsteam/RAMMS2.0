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
    public interface IFormP1Service
    {
        //Task<FormF1ResponseDTO> FindF1ByW1ID(int id);
        //Task<FormF1ResponseDTO> FindFormF1ByID(int id);
        //Task<IEnumerable<FormF1DtlResponseDTO>> FindFormF1DtlByID(int id);

        Task<PagingResult<FormP1HeaderResponseDTO>> GetHeaderList(FilteredPagingDefinition<FormP1SearchGridDTO> filterOptions);

        Task<FormP1HeaderResponseDTO> GetHeaderById(int id);
        Task<int> SaveFormP1(FormP1HeaderResponseDTO FormP1);
       
        Task<int> UpdateFormP1(FormP1HeaderResponseDTO FormP1Header, List<FormP1ResponseDTO> FormP1Detail);
        int? DeleteFormP1(int id);

      //  Task<byte[]> FormDownload(string formname, int id, string filepath);

    }
}
