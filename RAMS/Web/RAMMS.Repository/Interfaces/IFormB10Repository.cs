
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
    public interface IFormB10Repository : IRepositoryBase<RmB10DailyProduction>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormB10ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB10SearchGridDTO> filterOptions);
        
        RmB10DailyProduction GetHeaderById(int id);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB10(RmB10DailyProduction FormB10, List<RmB10DailyProductionHistory> FormB10History);

      //  Task<FORMB10Rpt> GetReportData(int headerid);

    }
}
