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
    public interface IFormB11Service
    {
        Task<FormB11DTO> GetHeaderById(int headerId);
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        int? GetMaxRev(int Year);
        Task<List<FormB7LabourHistoryDTO>> GetLabourHistoryData(int year);
    }
}
