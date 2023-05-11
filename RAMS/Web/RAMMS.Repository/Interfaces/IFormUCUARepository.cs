using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository.Interfaces
{
   public interface IFormUCUARepository : IRepositoryBase<RmUcua>
    {
        int? SaveFormUCUA(RmUcua FormUCUA);
        Task<FormUCUARpt> GetReportData(int headerid); 
        Task<List<FormUCUAHeaderRequestDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions);
        int? DeleteFormUCUA(int id);
        Task<int> GetImageId(string iwRefNo, string type);


        Task<List<FormUCUAPhotoTypeDTO>> GetExitingPhotoType(int headerId);
        //Task<RmIwformImage> AddImage(RmIwformImage image);

        Task<List<RmUcuaImage>> AddMultiImage(List<RmUcuaImage> images);
        Task<int> ImageCount(string type, long headerId);
        Task<List<RmUcuaImage>> ImageList(int headerId);
        Task<int> DeleteImage(RmUcuaImage img);
    }
    
}
