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
   public interface IFormT3Service
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<FormT3HeaderDTO> GetHeaderById(int id, bool view);
        Task<FormT3HeaderDTO> FindDetails(FormT3HeaderDTO frmT3, int createdBy);
        int? GetMaxRev(int Year, string RmuCode);
        Task<int> SaveFormT3(List<FormT3HistoryDTO> FormT3);
        Task<FormT3HeaderDTO> SaveT3(FormT3HeaderDTO frmT3hdr, List<FormT3HistoryDTO> frmT3, bool updateSubmit);
        Task<List<FormT3HistoryDTO>> GetHistoryData(int historyID);
        Byte[] FormDownload(string formname, int id, string basepath, string filepath);
        Task<List<FormB14HistoryDTO>> GetPlannedBudgetData(string RmuCode, int year);
        Task<List<FormB10HistoryResponseDTO>> GetUnitData(int year);
        int Delete(int id);
    }
}
