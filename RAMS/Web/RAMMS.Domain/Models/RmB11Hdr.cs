using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB11Hdr
    {
        public RmB11Hdr()
        {
            RmB11CrewDayCostHeader = new HashSet<RmB11CrewDayCostHeader>();
            RmB11EquipmentCost = new HashSet<RmB11EquipmentCost>();
            RmB11LabourCost = new HashSet<RmB11LabourCost>();
            RmB11MaterialCost = new HashSet<RmB11MaterialCost>();
        }

        public int B11hPkRefNo { get; set; }
        public string B11hRmuCode { get; set; }
        public string B11hRmuName { get; set; }
        public int? B11hRevisionNo { get; set; }
        public DateTime? B11hRevisionDate { get; set; }
        public int? B11hRevisionYear { get; set; }
        public int? B11hCrBy { get; set; }
        public string B11hCrByName { get; set; }
        public DateTime? B11hCrDt { get; set; }

        public virtual ICollection<RmB11CrewDayCostHeader> RmB11CrewDayCostHeader { get; set; }
        public virtual ICollection<RmB11EquipmentCost> RmB11EquipmentCost { get; set; }
        public virtual ICollection<RmB11LabourCost> RmB11LabourCost { get; set; }
        public virtual ICollection<RmB11MaterialCost> RmB11MaterialCost { get; set; }
    }
}
