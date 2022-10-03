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

        Task<FormB7HeaderDTO> GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        
        Task<int> SaveFormB7(FormB7HeaderDTO FormB7);

        Byte[] FormDownload(string formname, int id, string basepath, string filepath);
    }
}
