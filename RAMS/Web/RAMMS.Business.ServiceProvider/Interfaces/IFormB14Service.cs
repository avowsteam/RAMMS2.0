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
    public interface IFormB14Service
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData);
        Task<FormB14HeaderDTO> GetHeaderById(int id, bool view);
        Task<FormB14HeaderDTO> FindDetails(FormB14HeaderDTO frmB14, int createdBy);
        int? GetMaxRev(int Year, string RmuCode);
        Task<int> SaveFormB14(List<FormB14HistoryDTO> FormB14);
        Task<FormB14HeaderDTO> SaveB14(FormB14HeaderDTO frmb14hdr, List<FormB14HistoryDTO> frmb14, bool updateSubmit);
        Task<List<FormB14HistoryDTO>> GetHistoryData(int historyID);
        Byte[] FormDownload(string formname, int id, string basepath, string filepath);
        Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetData(string RmuCode, int year);
        int Delete(int id);
    }
}
