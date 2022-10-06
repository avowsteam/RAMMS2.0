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
    public interface IFormT3Repository : IRepositoryBase<RmT3Hdr>
    {
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        Task<RmT3Hdr> FindDetails(RmT3Hdr frmT3);
        RmT3Hdr GetHeaderById(int id, bool view);
        int? GetMaxRev(int Year, string RmuCode);
        Task<RmT3Hdr> Save(RmT3Hdr frm, bool updateSubmit);
        Task<int> SaveFormT3(List<RmT3History> FormT3);
        List<FormT3Rpt> GetReportData(int headerid);
        List<RmT3History> GetHistoryData(int year);
        List<RmB14History> GetPlannedBudgetData(string RmuCode, int year);
        int? GetB14RevisionNo(string RmuCode, int? year);
        List<RmB10DailyProductionHistory> GetUnitData(int year);
        bool isF1Exist(int id);
        int DeleteHeader(RmT3Hdr frmT3);
    }
}
