using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmRmiIri
    {
        public int RmiiriPkRefNo { get; set; }
        public int? RmiiriYear { get; set; }
        public int? RmiiriMonth { get; set; }
        public int? RmiiriConditionNo { get; set; }
        public string RmiiriType { get; set; }
        public decimal? RmiiriPercentage { get; set; }
        public decimal? RmiiriRoadLength { get; set; }
        public DateTime? RmiiriCreatedDate { get; set; }
        public DateTime? RmiiriUpdatedDate { get; set; }
    }
}
