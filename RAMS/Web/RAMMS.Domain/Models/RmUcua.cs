using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmUcua
    {
        public int RmmhPkRefNo { get; set; }
        public string RmmhRefId { get; set; }
        public string RmmhReportingName { get; set; }
        public string RmmhLocation { get; set; }
        public string RmmhWorkScope { get; set; }
        public string RmmhUnsafeAct { get; set; }
        public string RmmhUnsafeActDescription { get; set; }
        public string RmmhUnsafeCondition { get; set; }
        public string RmmhUnsafeConditionDescription { get; set; }
        public string RmmhImprovementRecommendation { get; set; }
        public DateTime? RmmhDateReceived { get; set; }
        public DateTime? RmmhDateCommitteeReview { get; set; }
        public string RmmhCommentsOfficeUse { get; set; }
        public string RmmhHseSection { get; set; }
        public string RmmhSafteyCommitteeChairman { get; set; }
        public string RmmhImsRep { get; set; }
        public DateTime? RmmhDateActionTaken { get; set; }
        public string RmmhActionTakenBy { get; set; }
        public string RmmhActionDescription { get; set; }
        public DateTime? RmmhDateEffectivenessActionTaken { get; set; }
        public string RmmhEffectivenessActionTakenBy { get; set; }
        public string RmmhEffectivenessActionDescription { get; set; }
        public string RmmhStatus { get; set; }
        public bool? RmmhActiveYn { get; set; }
        public bool? RmmhSubmitYn { get; set; }
        public string RmmhAuditLog { get; set; }
    }
}
