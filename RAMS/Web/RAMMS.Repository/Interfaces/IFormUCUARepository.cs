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
   public interface IFormUCUARepository : IRepositoryBase<RmUcua>
    {
        int? SaveFormUCUA(RmUcua FormUCUA);
        Task<FORMTRpt> GetReportData(int headerid); 
        Task<List<FormUCUAHeaderRequestDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions);
    }
    
}
