using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB11EquipmentCost
    {
        public int B11ecPkRefNo { get; set; }
        public int? B11ecB11hPkRefNo { get; set; }
        public int? B11ecActivityId { get; set; }
        public int? B11ecEquipmentId { get; set; }
        public decimal? B11ecEquipmentPerUnitPrice { get; set; }
        public int? B11ecEquipmentNoOfUnits { get; set; }
        public decimal? B11ecEquipmentTotalPrice { get; set; }

        public virtual RmB11Hdr B11ecB11hPkRefNoNavigation { get; set; }
    }
}
