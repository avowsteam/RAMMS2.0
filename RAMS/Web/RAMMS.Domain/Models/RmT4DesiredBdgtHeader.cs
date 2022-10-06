using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmT4DesiredBdgtHeader
    {
        public RmT4DesiredBdgtHeader()
        {
            RmT4DesiredBdgt = new HashSet<RmT4DesiredBdgt>();
        }

        public int T4dbhPkRefNo { get; set; }
        public string T4dbhPkRefId { get; set; }
        public int? T4dbhRevisionNo { get; set; }
        public DateTime? T4dbhRevisionDate { get; set; }
        public int? T4dbhRevisionYear { get; set; }
        public string T4dbhRmu { get; set; }
        public decimal? T4dbhAdjustableQuantity { get; set; }
        public decimal? T4dbhRoutineMaintenance { get; set; }
        public decimal? T4dbhPeriodicMaintenance { get; set; }
        public decimal? T4dbhOtherMaintenance { get; set; }
        public int? T4dbhModBy { get; set; }
        public DateTime? T4dbhModDt { get; set; }
        public int? T4dbhCrBy { get; set; }
        public DateTime? T4dbhCrDt { get; set; }
        public bool T4dbhSubmitSts { get; set; }
        public bool T4dbhActiveYn { get; set; }
        public string T4dbhStatus { get; set; }
        public string T4dbhAuditLog { get; set; }

        public virtual ICollection<RmT4DesiredBdgt> RmT4DesiredBdgt { get; set; }
    }
}
