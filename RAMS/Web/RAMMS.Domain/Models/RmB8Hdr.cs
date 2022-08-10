using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB8Hdr
    {
        public RmB8Hdr()
        {
            RmB8History = new HashSet<RmB8History>();
        }

        public int B8hPkRefNo { get; set; }
        public int? B8hRevisionNo { get; set; }
        public DateTime? B8hRevisionDate { get; set; }
        public int? B8hRevisionYear { get; set; }
        public int? B8hCrBy { get; set; }
        public string B8hCrByName { get; set; }
        public DateTime? B8hCrDt { get; set; }

        public virtual ICollection<RmB8History> RmB8History { get; set; }
    }
}
