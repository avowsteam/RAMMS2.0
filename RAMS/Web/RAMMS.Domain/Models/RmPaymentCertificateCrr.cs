using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificateCrr
    {
        public int CrrPkRefNo { get; set; }
        public int? CrrPcmamwPkRefNo { get; set; }
        public string CrrDivision { get; set; }
        public decimal? CrrPaved { get; set; }
        public decimal? CrrUnpaved { get; set; }
        public decimal? CrrSubTotal { get; set; }
        public decimal? CrrContractRate { get; set; }
        public decimal? CrrTotalAmount { get; set; }

        public virtual RmPaymentCertificateMamw CrrPcmamwPkRefNoNavigation { get; set; }
    }
}
