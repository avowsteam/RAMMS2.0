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
    public interface IFormB12Repository : IRepositoryBase<RmB12Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);

        RmB12Hdr GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB12(RmB12Hdr FoRmB12);

        //Task<FoRmB12Rpt> GetReportData(int headerid);
    }
}
