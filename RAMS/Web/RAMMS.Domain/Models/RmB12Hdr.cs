using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB12Hdr
    {
        public RmB12Hdr()
        {
            RmB12DesiredServiceLevelHistory = new HashSet<RmB12DesiredServiceLevelHistory>();
        }

        public int B12hPkRefNo { get; set; }
        public int? B12hRevisionNo { get; set; }
        public DateTime? B12hRevisionDate { get; set; }
        public int? B12hRevisionYear { get; set; }
        public int? B12hCrBy { get; set; }
        public string B12hCrByName { get; set; }
        public DateTime? B12hCrDt { get; set; }

        public virtual ICollection<RmB12DesiredServiceLevelHistory> RmB12DesiredServiceLevelHistory { get; set; }
    }
}
