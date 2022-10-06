using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB12DesiredServiceLevelHistory
    {
        public int B12dslhPkRefNo { get; set; }
        public int? B12dslhB12hPkRefNo { get; set; }
        public string B12dslhFeature { get; set; }
        public string B12dslhActCode { get; set; }
        public string B12dslhActName { get; set; }
        public decimal? B12dslhRmuMiri { get; set; }
        public decimal? B12dslhRmuBatuniah { get; set; }
        public string B12dslhUnitOfService { get; set; }
        public int? B12dslhOrder { get; set; }

        public virtual RmB12Hdr B12dslhB12hPkRefNoNavigation { get; set; }
    }
}
