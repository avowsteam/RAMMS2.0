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
    public interface IFormB12Service
    {
       
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        Task<FormB12DTO> FindDetails(FormB12DTO frmB12, int createdBy);

        Task<FormB12DTO> GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        
        Task<int> SaveFormB12(FormB12DTO FormB12);

        Task<FormB12DTO> SaveB12(FormB12DTO frmb12hdr, bool updateSubmit);

        Byte[] FormDownload(string formname, int id, string basepath, string filepath);

        int Delete(int id);

        Task<List<FormB12HistoryDTO>> GetHistoryData(int historyID);

        Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetDataMiri( int year);

        Task<List<FormB13HistoryResponseDTO>> GetPlannedBudgetDataBTN(int year);

        Task<List<FormB10HistoryResponseDTO>> GetUnitData(int year);
    }
}
