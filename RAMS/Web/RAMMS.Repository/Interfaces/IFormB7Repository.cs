using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository.Interfaces
{
    public interface IFormB7Repository : IRepositoryBase<RmB7Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        RmB7Hdr GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB7(RmB7Hdr FormB7);

        Task<FormB7Rpt> GetReportData(int headerid);
    }
}
