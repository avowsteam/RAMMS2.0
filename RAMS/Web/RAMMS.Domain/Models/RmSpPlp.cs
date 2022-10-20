using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmSpPlp
    {
        public int SpplpPkRefNo { get; set; }
        public int? SpplpYear { get; set; }
        public int? SpplpMonth { get; set; }
        public string SpplpDivCode { get; set; }
        public string SpplpDivName { get; set; }
        public decimal? SpplpMPlanned { get; set; }
        public decimal? SpplpMActual { get; set; }
        public decimal? SpplpCPlan { get; set; }
        public decimal? SpplpCActual { get; set; }
        public decimal? SpplpPiWorkActual { get; set; }
        public decimal? SpplpPai { get; set; }
        public decimal? SpplpEff { get; set; }
        public decimal? SpplpRb { get; set; }
        public decimal? SpplpIw { get; set; }
        public decimal? SpplpSpi { get; set; }
        public decimal? SpplpPlannedPer { get; set; }
        public decimal? SpplpActualPer { get; set; }
        public DateTime? SpplpCreatedDate { get; set; }
        public DateTime? SpplpUpdatedDate { get; set; }
    }
}
