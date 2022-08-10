using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB10DailyProduction
    {
        public RmB10DailyProduction()
        {
            RmB10DailyProductionHistory = new HashSet<RmB10DailyProductionHistory>();
        }

        public int B10dpPkRefNo { get; set; }
        public int? B10dpRevisionNo { get; set; }
        public DateTime? B10dpRevisionDate { get; set; }
        public int? B10dpRevisionYear { get; set; }
        public int? B10dpUserId { get; set; }
        public string B10dpUserName { get; set; }

        public virtual ICollection<RmB10DailyProductionHistory> RmB10DailyProductionHistory { get; set; }
    }
}
