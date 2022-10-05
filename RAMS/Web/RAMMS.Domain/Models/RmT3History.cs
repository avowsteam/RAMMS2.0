using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmT3History
    {
        public int T3hhPkRefNoHistory { get; set; }
        public int? T3hhT3hPkRefNo { get; set; }
        public int? T3hhActId { get; set; }
        public string T3hhFeature { get; set; }
        public string T3hhActCode { get; set; }
        public string T3hhActName { get; set; }
        public decimal? T3hhJan { get; set; }
        public decimal? T3hhFeb { get; set; }
        public decimal? T3hhMar { get; set; }
        public decimal? T3hhApr { get; set; }
        public decimal? T3hhMay { get; set; }
        public decimal? T3hhJun { get; set; }
        public decimal? T3hhJul { get; set; }
        public decimal? T3hhAug { get; set; }
        public decimal? T3hhSep { get; set; }
        public decimal? T3hhOct { get; set; }
        public decimal? T3hhNov { get; set; }
        public decimal? T3hhDec { get; set; }
        public string T3hhUnitOfService { get; set; }
        public decimal? T3hhSubTotal { get; set; }
        public int? T3hhOrder { get; set; }

        public virtual RmT3Hdr T3hhT3hPkRefNoNavigation { get; set; }
    }
}
