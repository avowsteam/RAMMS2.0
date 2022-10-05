using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmT3Hdr
    {
        public RmT3Hdr()
        {
            RmT3History = new HashSet<RmT3History>();
        }

        public int T3hPkRefNo { get; set; }
        public string T3hPkRefId { get; set; }
        public int? T3hRevisionNo { get; set; }
        public DateTime? T3hRevisionDate { get; set; }
        public int? T3hRevisionYear { get; set; }
        public string T3hRmuCode { get; set; }
        public string T3hRmuName { get; set; }
        public int? T3hCrBy { get; set; }
        public DateTime? T3hCrDt { get; set; }
        public int? T3hModBy { get; set; }
        public DateTime? T3hModDt { get; set; }
        public bool T3hActiveYn { get; set; }
        public bool T3hSubmitSts { get; set; }
        public string T3hStatus { get; set; }
        public string T3hAuditlog { get; set; }

        public virtual ICollection<RmT3History> RmT3History { get; set; }
    }
}
