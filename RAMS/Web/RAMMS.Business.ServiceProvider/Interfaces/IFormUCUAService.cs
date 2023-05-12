using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Domain.Models;
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
    public interface IFormUCUAService
    {
        Task<FormUCUAResponseDTO> GetHeaderById(int id);
        Task<FormUCUAResponseDTO> SaveFormUCUA(FormUCUAResponseDTO FormUCUA);
        Task<int> Update(FormUCUAResponseDTO formUCUA);
        Task<byte[]> FormDownload(string formname, int id, string filepath);
        Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions);
        int? DeleteFormUCUA(int id);

        Task<int> LastInsertedIMAGENO(string hederId, string type);

        Task<int> SaveImage(List<FormUCUAImageResponseDTO> image);

       

        Task<List<FormUCUAPhotoTypeDTO>> GetExitingPhotoType(int headerId);
        //Task<FormUCUAImagesDTO> AddImage(FormUCUAImagesDTO imageDTO);
        //Task<(IList<FormUCUAImagesDTO>, int)> AddMultiImage(IList<FormUCUAImagesDTO> imagesDTO);
        //Task<List<RmUcuaImage>> AddMultiImageTab(List<FormUCUAImagesDTO> imagesDTO);
        Task<List<RmUcuaImage>> AddMultiImage(List<FormUCUAImagesDTO> imagesDTO);
        List<FormUCUAImagesDTO> ImageList(int headerId);
        Task<int> DeleteImage(int headerId, int imgId);
        List<FormUCUAImagesDTO> ImageListWeb(int headerId);
    }
}
