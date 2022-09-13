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
    public interface IFormB15Service
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        Task<FormB15HeaderDTO> GetHeaderById(int id, bool view);
        Task<FormB15HeaderDTO> FindDetails(FormB15HeaderDTO frmB15, int createdBy);
        int? GetMaxRev(int Year, string RmuCode);
        Task<int> SaveFormB15(List<FormB15HistoryDTO> FormB15);
        Task<FormB15HeaderDTO> SaveB15(FormB15HeaderDTO frmb15hdr,List<FormB15HistoryDTO> frmb15, bool updateSubmit);
        Task<List<FormB15HistoryDTO>> GetHistoryData(int historyID);
        Byte[] FormDownload(string formname, int id, string basepath, string filepath);
        Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetData(string RmuCode, int year);
        int Delete(int id);
    }
}
