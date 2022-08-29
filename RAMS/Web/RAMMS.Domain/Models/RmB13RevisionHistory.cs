using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB13RevisionHistory
    {
        public int B13rhPkRefNo { get; set; }
        public int? B13rhB13pPkRefNo { get; set; }
        public string B13rhDescription { get; set; }
        public DateTime? B13rhDate { get; set; }
        public int? B13rhRevNo { get; set; }

        public virtual RmB13ProposedPlannedBudget B13rhB13pPkRefNoNavigation { get; set; }
    }
}
