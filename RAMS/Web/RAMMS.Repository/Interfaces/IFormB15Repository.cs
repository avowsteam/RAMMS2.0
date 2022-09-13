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
    public interface IFormB15Repository : IRepositoryBase<RmB15Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<RmB15Hdr> FindDetails(RmB15Hdr frmB15);
        RmB15Hdr GetHeaderById(int id, bool view);
        int? GetMaxRev(int Year, string RmuCode);
        Task<RmB15Hdr> Save(RmB15Hdr frm, bool updateSubmit);
        //Task<List<RmB15History>> SaveAD(List<RmB15History> frmMAD, bool updateSubmit);
        Task<int> SaveFormB15(List<RmB15History> FormB15);
        Task<FormB15Rpt> GetReportData(int headerid);
        List<RmB15History> GetHistoryData(int year);
        bool isF1Exist(int id);
        int DeleteHeader(RmB15Hdr frmB15);
    }
}
