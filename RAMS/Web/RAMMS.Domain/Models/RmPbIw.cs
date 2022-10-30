using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmPbIw
    {
        public RmPbIw()
        {
            RmPbIwDetails = new HashSet<RmPbIwDetails>();
        }

        public int PbiwPkRefNo { get; set; }
        public string PbiwRefId { get; set; }
        public DateTime? PbiwSubmissionDate { get; set; }
        public int? PbiwSubmissionYear { get; set; }
        public int? PbiwSubmissionMonth { get; set; }
        public decimal? PbiwAmountBeforeLad { get; set; }
        public decimal? PbiwLaDamage { get; set; }
        public decimal? PbiwFinalPayment { get; set; }
        public bool PbiwSignSp { get; set; }
        public int? PbiwUseridSp { get; set; }
        public string PbiwUsernameSp { get; set; }
        public string PbiwDesignationSp { get; set; }
        public DateTime? PbiwSignDateSp { get; set; }
        public bool PbiwSignEc { get; set; }
        public int? PbiwUseridEc { get; set; }
        public string PbiwUsernameEc { get; set; }
        public string PbiwDesignationEc { get; set; }
        public DateTime? PbiwSignDateEc { get; set; }
        public bool PbiwSignSo { get; set; }
        public int? PbiwUseridSo { get; set; }
        public string PbiwUsernameSo { get; set; }
        public string PbiwDesignationSo { get; set; }
        public DateTime? PbiwSignDateSo { get; set; }
        public bool? PbiwActiveYn { get; set; }
        public bool PbiwSubmitSts { get; set; }
        public string PbiwStatus { get; set; }
        public string PbiwAuditLog { get; set; }

        public virtual ICollection<RmPbIwDetails> RmPbIwDetails { get; set; }
    }
}
