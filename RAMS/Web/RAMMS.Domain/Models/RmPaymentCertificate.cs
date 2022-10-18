using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificate
    {
        public int PcPkRefNo { get; set; }
        public int? PcPchPkRefNo { get; set; }
        public string PcDescription { get; set; }
        public string PcPaymentType { get; set; }
        public decimal? PcAmount { get; set; }
        public decimal? PcAddition { get; set; }
        public decimal? PcDeduction { get; set; }
        public decimal? PcPreviousPayment { get; set; }
        public decimal? PcTotalToDate { get; set; }
        public decimal? PcAmountIncludedInPc { get; set; }

        public virtual RmPaymentCertificateHeader PcPchPkRefNoNavigation { get; set; }
    }
}
