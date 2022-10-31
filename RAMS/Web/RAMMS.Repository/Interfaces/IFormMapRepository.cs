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
    public interface IFormMapRepository : IRepositoryBase<RmMapHeader>
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<RmMapHeader> FindDetails(RmMapHeader frmT3);
        RmMapHeader GetHeaderById(int id, bool view);
        int DeleteHeader(RmMapHeader frmT3);
        List<RmFormDHdr> GetForDDetails(string RMU, int Year, int Month);
        List<RmMapDetails> GetForMapDetails(int ID);
        Task<RmMapHeader> Save(RmMapHeader frm, bool updateSubmit);
        Task<int> SaveFormB14(List<RmMapDetails> Formmap);
    }
}
