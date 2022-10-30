using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormPAHeaderResponseDTO
    {
 
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SubmissionDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContractsEndsOn { get; set; }
        public int? SubmissionYear { get; set; }
        public int? SubmissionMonth { get; set; }
        public decimal? WorkValueDeduction { get; set; }
        public decimal? WorkValueAddition { get; set; }
        public decimal? TotalPayment { get; set; }
        public bool SignSp { get; set; }
        public int? UseridSp { get; set; }
        public string UsernameSp { get; set; }
        public string DesignationSp { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SignDateSp { get; set; }
        public bool SignEc { get; set; }
        public int? UseridEc { get; set; }
        public string UsernameEc { get; set; }
        public string DesignationEc { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SignDateEc { get; set; }
        public bool SignSo { get; set; }
        public int? UseridSo { get; set; }
        public string UsernameSo { get; set; }
        public string DesignationSo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SignDateSo { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }
        public string AuditLog { get; set; }

        public List<RmPaymentCertificateCrr> RmPaymentCertificateCrr { get; set; }
        public List<RmPaymentCertificateCrra> RmPaymentCertificateCrra { get; set; }
        public List<RmPaymentCertificateCrrd> RmPaymentCertificateCrrd { get; set; }
    }
}
