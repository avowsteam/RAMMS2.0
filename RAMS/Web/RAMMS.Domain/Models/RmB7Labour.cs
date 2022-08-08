using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7Labour
    {
        public RmB7Labour()
        {
            RmB7LabourHistory = new HashSet<RmB7LabourHistory>();
        }

        public int B7lPkRefNo { get; set; }
        public int? B7lRevisionNo { get; set; }
        public DateTime? B7lRevisionDate { get; set; }
        public int? B7lRevisionYear { get; set; }
        public int? B7lCrBy { get; set; }
        public string B7lCrByName { get; set; }
        public DateTime? B7lCrDt { get; set; }

        public virtual ICollection<RmB7LabourHistory> RmB7LabourHistory { get; set; }
    }
}
