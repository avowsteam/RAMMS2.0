
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
    public interface IFormT4Repository : IRepositoryBase<RmT4DesiredBdgtHeader>
    {
        
        Task<List<FormT4HeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormT4SearchGridDTO> filterOptions);

        RmT4DesiredBdgtHeader GetHeaderById(int id);

        Task<RmT4DesiredBdgtHeader> SaveFormT4(RmT4DesiredBdgtHeader FormT4);
        int? GetMaxRev(int Year, string RMU);
        Task<int> UpdateFormT4(RmT4DesiredBdgtHeader FormT4, List<RmT4DesiredBdgt> FormT4History);

        int? DeleteFormT4(int id);
 
    }
}
