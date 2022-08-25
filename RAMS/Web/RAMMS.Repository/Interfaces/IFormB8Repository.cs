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
    public interface IFormB8Repository : IRepositoryBase<RmB8Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        RmB8Hdr GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB8(RmB8Hdr FormB8);

        Task<List<FormB8Rpt>> GetReportData(int headerid);
    }
}
