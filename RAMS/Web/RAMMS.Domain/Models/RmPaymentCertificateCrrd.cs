using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPaymentCertificateCrrd
    {
        public int CrrdPkRefNo { get; set; }
        public int? CrrdPcmamwPkRefNo { get; set; }
        public string CrrdDescription { get; set; }
        public decimal? CrrdThisPayment { get; set; }
        public decimal? CrrdTillLastPayment { get; set; }
        public decimal? CrrdTotalToDate { get; set; }

        public virtual RmPaymentCertificateMamw CrrdPcmamwPkRefNoNavigation { get; set; }
    }
}
