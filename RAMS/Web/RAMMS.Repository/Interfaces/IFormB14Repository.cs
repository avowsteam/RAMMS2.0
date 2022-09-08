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
    public interface IFormB14Repository : IRepositoryBase<RmB14Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        RmB14Hdr GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year, string RmuCode);
        Task<int> SaveFormB14(RmB14Hdr FormB14);

        Task<FormB14Rpt> GetReportData(int headerid);
    }
}
