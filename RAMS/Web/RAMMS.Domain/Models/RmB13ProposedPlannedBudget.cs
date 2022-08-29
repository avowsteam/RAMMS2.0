using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB13ProposedPlannedBudget
    {
        public RmB13ProposedPlannedBudget()
        {
            RmB13ProposedPlannedBudgetHistory = new HashSet<RmB13ProposedPlannedBudgetHistory>();
            RmB13RevisionHistory = new HashSet<RmB13RevisionHistory>();
        }

        public int B13pPkRefNo { get; set; }
        public int? B13pRevisionNo { get; set; }
        public DateTime? B13pRevisionDate { get; set; }
        public int? B13pRevisionYear { get; set; }
        public string B13pRmu { get; set; }
        public decimal? B13pAdjustableQuantity { get; set; }
        public decimal? B13pRoutineMaintenance { get; set; }
        public decimal? B13pPeriodicMaintenance { get; set; }
        public decimal? B13pOtherMaintenance { get; set; }
        public int? B13pUseridProsd { get; set; }
        public string B13pUserNameProsd { get; set; }
        public string B13pUserDesignationProsd { get; set; }
        public DateTime? B13pDtProsd { get; set; }
        public bool B13pSignProsd { get; set; }
        public int? B13pUseridFclitd { get; set; }
        public string B13pUserNameFclitd { get; set; }
        public string B13pUserDesignationFclitd { get; set; }
        public DateTime? B13pDtFclitd { get; set; }
        public bool B13pSignFclitd { get; set; }
        public int? B13pUseridAgrd { get; set; }
        public string B13pUserNameAgrd { get; set; }
        public string B13pUserDesignationAgrd { get; set; }
        public DateTime? B13pDtAgrd { get; set; }
        public bool B13pSignAgrd { get; set; }
        public int? B13pUseridEdosd { get; set; }
        public string B13pUserNameEdosd { get; set; }
        public string B13pUserDesignationEdosd { get; set; }
        public DateTime? B13pDtEdosd { get; set; }
        public bool B13pSignEdosd { get; set; }
        public int? B13pModBy { get; set; }
        public DateTime? B13pModDt { get; set; }
        public int? B13pCrBy { get; set; }
        public DateTime? B13pCrDt { get; set; }
        public bool B13pSubmitSts { get; set; }
        public bool B13pActiveYn { get; set; }
        public string B13pStatus { get; set; }
        public string B13pAuditLog { get; set; }

        public virtual ICollection<RmB13ProposedPlannedBudgetHistory> RmB13ProposedPlannedBudgetHistory { get; set; }
        public virtual ICollection<RmB13RevisionHistory> RmB13RevisionHistory { get; set; }
    }
}
