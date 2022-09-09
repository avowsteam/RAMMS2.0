using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB14History
    {
        public int B14hhPkRefNo { get; set; }
        public int? B14hhB14hPkRefNo { get; set; }
        public int? B14hhActId { get; set; }
        public string B14hhFeature { get; set; }
        public string B14hhActCode { get; set; }
        public string B14hhActName { get; set; }
        public decimal? B14hhJan { get; set; }
        public decimal? B14hhFeb { get; set; }
        public decimal? B14hhMar { get; set; }
        public decimal? B14hhApr { get; set; }
        public decimal? B14hhMay { get; set; }
        public decimal? B14hhJun { get; set; }
        public decimal? B14hhJul { get; set; }
        public decimal? B14hhAug { get; set; }
        public decimal? B14hhSep { get; set; }
        public decimal? B14hhOct { get; set; }
        public decimal? B14hhNov { get; set; }
        public decimal? B14hhDec { get; set; }
        public string B14hhUnitOfService { get; set; }
        public decimal? B14hhSubTotal { get; set; }

        public virtual RmB14Hdr B14hhB14hPkRefNoNavigation { get; set; }
    }
}
