
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
    public interface IFormP1Repository : IRepositoryBase<RmPaymentCertificateHeader>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormP1HeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormP1SearchGridDTO> filterOptions);

        RmPaymentCertificateHeader GetHeaderById(int id);

        Task<int> SaveFormP1(RmPaymentCertificateHeader FormP1, bool update = false);
         
        Task<int> UpdateFormP1(RmPaymentCertificateHeader FormP1Header, List<RmPaymentCertificate> FormP1);

        int? DeleteFormP1(int id);

        //  Task<FORMP1Rpt> GetReportData(int headerid);

    }
}
