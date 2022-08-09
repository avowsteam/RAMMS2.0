using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7Material
    {
        public RmB7Material()
        {
            RmB7MaterialHistory = new HashSet<RmB7MaterialHistory>();
        }

        public int B7mPkRefNo { get; set; }
        public int? B7mRevisionNo { get; set; }
        public DateTime? B7mRevisionDate { get; set; }
        public int? B7mRevisionYear { get; set; }
        public int? B7mCrBy { get; set; }
        public string B7mCrByName { get; set; }
        public DateTime? B7mCrDt { get; set; }

        public virtual ICollection<RmB7MaterialHistory> RmB7MaterialHistory { get; set; }
    }
}
