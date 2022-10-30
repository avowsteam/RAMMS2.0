using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPbIwDetails
    {
        public int PbiwdPkRefNo { get; set; }
        public int? PbiwdPbiwPkRefNo { get; set; }
        public string PbiwdIwRef { get; set; }
        public string PbiwdProjectTitle { get; set; }
        public DateTime? PbiwdCompletionDate { get; set; }
        public string PbiwdCompletionRefNo { get; set; }
        public decimal? PbiwdAmountBeforeLad { get; set; }
        public decimal? PbiwdLaDamage { get; set; }
        public decimal? PbiwdFinalPayment { get; set; }

        public virtual RmPbIw PbiwdPbiwPkRefNoNavigation { get; set; }
    }
}
