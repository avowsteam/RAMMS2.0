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

        Task<FormB14HeaderDTO> GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year, string RmuCode);
        
        Task<int> SaveFormB14(FormB14HeaderDTO FormB14);

        Byte[] FormDownload(string formname, int id, string basepath, string filepath);

        Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData);
    }
}
