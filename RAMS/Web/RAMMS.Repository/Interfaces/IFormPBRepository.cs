
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
    public interface IFormPBRepository : IRepositoryBase<RmPaymentCertificateHeader>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormPBHeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormPBSearchGridDTO> filterOptions);

        RmPaymentCertificateHeader GetHeaderById(int id);

        Task<int> SaveFormPB(RmPaymentCertificateHeader FormPB);
      
        Task<int> UpdateFormPB(RmPaymentCertificateHeader FormPBHeader, List<RmPaymentCertificate> FormPB);

        int? DeleteFormPB(int id);

        //  Task<FORMPBRpt> GetReportData(int headerid);

    }
}
