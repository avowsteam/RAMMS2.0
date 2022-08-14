using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB11MaterialCost
    {
        public int B11mcPkRefNo { get; set; }
        public int? B11mcB11hPkRefNo { get; set; }
        public int? B11mcActivityId { get; set; }
        public string B11mcMaterialId { get; set; }
        public string B11mcMaterialName { get; set; }
        public decimal? B11mcMaterialPerUnitPrice { get; set; }
        public int? B11mcMaterialNoOfUnits { get; set; }
        public decimal? B11mcMaterialTotalPrice { get; set; }
        public int? B11mcMaterialOrderId { get; set; }

        public virtual RmB11Hdr B11mcB11hPkRefNoNavigation { get; set; }
    }
}
