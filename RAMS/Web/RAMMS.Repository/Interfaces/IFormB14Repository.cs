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
        Task<RmB14Hdr> FindDetails(RmB14Hdr frmB14);
        RmB14Hdr GetHeaderById(int id, bool view);
        int? GetMaxRev(int Year, string RmuCode);
        Task<RmB14Hdr> Save(RmB14Hdr frm, bool updateSubmit);
        //Task<List<RmB14History>> SaveAD(List<RmB14History> frmMAD, bool updateSubmit);
        Task<int> SaveFormB14(List<RmB14History> FormB14);
        List<FormB14Rpt> GetReportData(int headerid);
        List<RmB14History> GetHistoryData(int year);
        List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetData(string RmuCode, int year);
        List<RmB10DailyProductionHistory> GetUnitData(int year);
        bool isF1Exist(int id);
        int DeleteHeader(RmB14Hdr frmB14);
        Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData);
    }
}
