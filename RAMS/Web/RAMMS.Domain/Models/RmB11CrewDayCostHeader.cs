using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB11CrewDayCostHeader
    {
        public int B11cdchPkRefNo { get; set; }
        public int? B11cdchB11hPkRefNo { get; set; }
        public string B11cdchFeature { get; set; }
        public int? B11cdchActivityCode { get; set; }
        public string B11cdchActivityName { get; set; }
        public decimal? B11cdchCrewDayCost { get; set; }

        public virtual RmB11Hdr B11cdchB11hPkRefNoNavigation { get; set; }
    }
}
