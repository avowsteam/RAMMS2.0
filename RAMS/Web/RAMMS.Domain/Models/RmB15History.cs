using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB15History
    {
        public int B15hhPkRefNoHistory { get; set; }
        public int? B15hhB15hPkRefNo { get; set; }
        public int? B15hhActId { get; set; }
        public string B15hhFeature { get; set; }
        public string B15hhActCode { get; set; }
        public string B15hhActName { get; set; }
        public decimal? B15hhJan { get; set; }
        public decimal? B15hhFeb { get; set; }
        public decimal? B15hhMar { get; set; }
        public decimal? B15hhApr { get; set; }
        public decimal? B15hhMay { get; set; }
        public decimal? B15hhJun { get; set; }
        public decimal? B15hhJul { get; set; }
        public decimal? B15hhAug { get; set; }
        public decimal? B15hhSep { get; set; }
        public decimal? B15hhOct { get; set; }
        public decimal? B15hhNov { get; set; }
        public decimal? B15hhDec { get; set; }
        public string B15hhUnitOfService { get; set; }
        public decimal? B15hhSubTotal { get; set; }
        public string B15hhRemarks { get; set; }
        public int? B15hhOrder { get; set; }

        public virtual RmB15Hdr B15hhB15hPkRefNoNavigation { get; set; }
    }
}
