
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RAMMS.Repository.Interfaces
{
    public interface IFormB13Repository : IRepositoryBase<RmB13ProposedPlannedBudget>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormB13ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB13SearchGridDTO> filterOptions);

        RmB13ProposedPlannedBudget GetHeaderById(int id);

        Task<RmB13ProposedPlannedBudget> SaveFormB13(RmB13ProposedPlannedBudget FormB13);
        int? GetMaxRev(int Year, string RMU);
        Task<int> UpdateFormB13(RmB13ProposedPlannedBudget FormB13, List<RmB13ProposedPlannedBudgetHistory> FormB13History);

        int? DeleteFormB13(int id);

        //  Task<FORMB13Rpt> GetReportData(int headerid);

    }
}
