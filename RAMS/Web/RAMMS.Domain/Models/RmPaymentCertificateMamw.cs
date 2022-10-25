using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificateMamw
    {
        public RmPaymentCertificateMamw()
        {
            RmPaymentCertificateCrr = new HashSet<RmPaymentCertificateCrr>();
            RmPaymentCertificateCrra = new HashSet<RmPaymentCertificateCrra>();
            RmPaymentCertificateCrrd = new HashSet<RmPaymentCertificateCrrd>();
        }

        public int PcmamwPkRefNo { get; set; }
        public string PcmamwRefId { get; set; }
        public DateTime? PcmamwSubmissionDate { get; set; }
        public DateTime? PcmamwContractsEndsOn { get; set; }
        public int? PcmamwSubmissionYear { get; set; }
        public int? PcmamwSubmissionMonth { get; set; }
        public decimal? PcmamwWorkValueDeduction { get; set; }
        public decimal? PcmamwWorkValueAddition { get; set; }
        public decimal? PcmamwTotalPayment { get; set; }
        public string PcmamwSignSp { get; set; }
        public int? PcmamwUseridSp { get; set; }
        public string PcmamwUsernameSp { get; set; }
        public string PcmamwDesignationSp { get; set; }
        public DateTime? PcmamwSignDateSp { get; set; }
        public string PcmamwSignEc { get; set; }
        public int? PcmamwUseridEc { get; set; }
        public string PcmamwUsernameEc { get; set; }
        public string PcmamwDesignationEc { get; set; }
        public DateTime? PcmamwSignDateEc { get; set; }
        public string PcmamwSignSo { get; set; }
        public int? PcmamwUseridSo { get; set; }
        public string PcmamwUsernameSo { get; set; }
        public string PcmamwDesignationSo { get; set; }
        public DateTime? PcmamwSignDateSo { get; set; }
        public bool? PcmamwActiveYn { get; set; }
        public bool PcmamwSubmitSts { get; set; }
        public string PcmamwStatus { get; set; }
        public string PcmamwAuditLog { get; set; }

        public virtual ICollection<RmPaymentCertificateCrr> RmPaymentCertificateCrr { get; set; }
        public virtual ICollection<RmPaymentCertificateCrra> RmPaymentCertificateCrra { get; set; }
        public virtual ICollection<RmPaymentCertificateCrrd> RmPaymentCertificateCrrd { get; set; }
    }
}
