
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
    public interface IFormB9Repository : IRepositoryBase<RmB9DesiredService>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormB9ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB9SearchGridDTO> filterOptions);

        Task<List<FormB9HistoryResponseDTO>> GetFormB9HistoryGridList(FilteredPagingDefinition<FormB9HistoryResponseDTO> filterOptions);

        RmB9DesiredService GetHeaderById(int id);

        int? GetMaxRev(int Year);
        Task<int> SaveFormB9(RmB9DesiredService FormB9, List<RmB9DesiredServiceHistory> FormB9History);

      //  Task<FORMB9Rpt> GetReportData(int headerid);

    }
}
