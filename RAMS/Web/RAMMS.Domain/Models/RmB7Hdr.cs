using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7Hdr
    {
        public RmB7Hdr()
        {
            RmB7EquipmentsHistory = new HashSet<RmB7EquipmentsHistory>();
            RmB7LabourHistory = new HashSet<RmB7LabourHistory>();
            RmB7MaterialHistory = new HashSet<RmB7MaterialHistory>();
        }

        public int B7hPkRefNo { get; set; }
        public int? B7hRevisionNo { get; set; }
        public DateTime? B7hRevisionDate { get; set; }
        public int? B7hRevisionYear { get; set; }
        public int? B7hCrBy { get; set; }
        public string B7hCrByName { get; set; }
        public DateTime? B7hCrDt { get; set; }

        public virtual ICollection<RmB7EquipmentsHistory> RmB7EquipmentsHistory { get; set; }
        public virtual ICollection<RmB7LabourHistory> RmB7LabourHistory { get; set; }
        public virtual ICollection<RmB7MaterialHistory> RmB7MaterialHistory { get; set; }
    }
}
