using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificateHeader
    {
        public RmPaymentCertificateHeader()
        {
            RmPaymentCertificate = new HashSet<RmPaymentCertificate>();
        }

        public int PchPkRefNo { get; set; }
        public string PchRefId { get; set; }
        public string PchSrProvider { get; set; }
        public string PchBank { get; set; }
        public string PchBankAccNo { get; set; }
        public string PchAddress { get; set; }
        public int? PchPaymentCertificateNo { get; set; }
        public DateTime? PchSubmissionDate { get; set; }
        public DateTime? PchContractsEndsOn { get; set; }
        public int? PchSubmissionYear { get; set; }
        public int? PchSubmissionMonth { get; set; }
        public decimal? PchContractRoadLength { get; set; }
        public decimal? PchNetValueDeduction { get; set; }
        public decimal? PchNetValueAddition { get; set; }
        public decimal? PchNetValueInstructedWork { get; set; }
        public decimal? PchNetValueLadInstructedWork { get; set; }
        public decimal? PchTotalPayment { get; set; }
        public bool? PchSignSo { get; set; }
        public int? PchUseridSo { get; set; }
        public string PchUsernameSo { get; set; }
        public string PchDesignationSo { get; set; }
        public DateTime? PchSignDateSo { get; set; }
        public bool? PchActiveYn { get; set; }
        public bool PchSubmitSts { get; set; }
        public string PchStatus { get; set; }
        public string PchAuditLog { get; set; }

        public virtual ICollection<RmPaymentCertificate> RmPaymentCertificate { get; set; }
    }
}
