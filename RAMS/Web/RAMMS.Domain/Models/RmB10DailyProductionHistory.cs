using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB10DailyProductionHistory
    {
        public int B10dphPkRefNo { get; set; }
        public int? B10dphB10dpPkRefNo { get; set; }
        public string B10dphFeature { get; set; }
        public string B10dphCode { get; set; }
        public string B10dphName { get; set; }
        public decimal? B10dphAdpValue { get; set; }
        public string B10dphAdpUnit { get; set; }
        public decimal? B10dphAdpUnitDescription { get; set; }
        public int? B10dphUserId { get; set; }
        public string B10dphUserName { get; set; }

        public virtual RmB10DailyProduction B10dphB10dpPkRefNoNavigation { get; set; }
    }
}
