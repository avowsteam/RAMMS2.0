
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
    public interface IFormPARepository : IRepositoryBase<RmPaymentCertificateMamw>
    {
        //   Task<IEnumerable<RmFormF1Dtl>> FindFormF1DtlByID(int Id);


        Task<List<FormPAHeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormPASearchGridDTO> filterOptions);

        RmPaymentCertificateMamw GetHeaderById(int id);

        Task<int> SaveFormPA(RmPaymentCertificateMamw FormPA);

        Task<int> UpdateFormPA(RmPaymentCertificateMamw FormPAHeader, List<RmPaymentCertificateCrr> FormPACrr, List<RmPaymentCertificateCrra> FormPACrra, List<RmPaymentCertificateCrrd> FormPACrrd);

        int? DeleteFormPA(int id);

        //  Task<FORMPARpt> GetReportData(int headerid);

    }
}
