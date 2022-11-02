using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO.RequestBO;

namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IFormMapService
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<FormMapHeaderDTO> GetHeaderById(int id, bool view);
        Task<FormMapHeaderDTO> FindDetails(FormMapHeaderDTO frmMap, int createdBy);
        //Task<int> SaveFormMap(List<FormMapDetailsDTO> FormMap);
        //Task<FormMapHeaderDTO> SaveT3(FormMapHeaderDTO frmT3hdr, List<FormMapDetailsDTO> frmT3, bool updateSubmit);
        //Task<List<FormMapDetailsDTO>> GetHistoryData(int historyID);
        //Byte[] FormDownload(string formname, int id, string basepath, string filepath);
        Task<List<FormDHeaderResponseDTO>> GetForDDetails(string RMU, int Year, int Month);
        Task<List<FormMapDetailsDTO>> GetForMapDetails(int ID);
        int Delete(int id);
        Task<FormMapHeaderDTO> SaveMap(FormMapHeaderDTO frmmaphdr, List<FormMapDetailsDTO> frmmap, bool updateSubmit);
    }
}
