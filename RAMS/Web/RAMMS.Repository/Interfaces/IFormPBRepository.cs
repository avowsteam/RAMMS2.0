
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
    public interface IFormPBRepository : IRepositoryBase<RmPbIw>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormPBHeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormPBSearchGridDTO> filterOptions);

        RmPbIw GetHeaderById(int id);

        Task<int> SaveFormPB(RmPbIw FormPB, bool update = false);


        Task<int> UpdateFormPB(RmPbIw FormPBHeader, List<RmPbIwDetails> FormPB);

        int? DeleteFormPB(int id);

        //  Task<FORMPBRpt> GetReportData(int headerid);

    }
}
