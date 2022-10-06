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

        Task<RmB12Hdr> FindDetails(RmB12Hdr frmB12);

        RmB12Hdr GetHeaderById(int id, bool view);

        int? GetMaxRev(int Year);
        Task<RmB12Hdr> Save(RmB12Hdr frm, bool updateSubmit);

        Task<int> SaveFormB12(List<RmB12DesiredServiceLevelHistory> FormB14);

        Task<FormB12Rpt> GetReportData(int headerid);

        int DeleteHeader(RmB12Hdr frmB12);

        List<RmB12DesiredServiceLevelHistory> GetHistoryData(int year);

        List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetDataMiri(int year);

        List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetDataBTN(int year);

        List<RmB10DailyProductionHistory> GetUnitData(int year);
    }
}
