using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificateCrra
    {
        public int CrraPkRefNo { get; set; }
        public int? CrraPcmamwPkRefNo { get; set; }
        public string CrraDescription { get; set; }
        public decimal? CrraThisPayment { get; set; }
        public decimal? CrraTillLastPayment { get; set; }
        public decimal? CrraTotalToDate { get; set; }

        public virtual RmPaymentCertificateMamw CrraPcmamwPkRefNoNavigation { get; set; }
    }
}
