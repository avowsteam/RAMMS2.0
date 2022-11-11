using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormUCUAResponseDTO
    {
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public string ReportingName { get; set; }
        public string  Location { get; set; }
        public string  WorkScope { get; set; }
        public bool  UnsafeAct { get; set; }
        public bool hdnUnsafeAct { get; set; }
        public string  UnsafeActDescription { get; set; }
        public bool  UnsafeCondition { get; set; }
        public bool hdnUnsafeCondition { get; set; }
        public string  UnsafeConditionDescription { get; set; }
        public string  ImprovementRecommendation { get; set; }
        public DateTime?  DateReceived { get; set; }
        public DateTime?  DateCommitteeReview { get; set; }
        public string  CommentsOfficeUse { get; set; }
        public string  HseSection { get; set; }
        public string  SafteyCommitteeChairman { get; set; }
        public string  ImsRep { get; set; }
        public DateTime?  DateActionTaken { get; set; }
        public string  ActionTakenBy { get; set; }
        public string  ActionDescription { get; set; }
        public DateTime?  DateEffectivenessActionTaken { get; set; }
        public string  EffectivenessActionTakenBy { get; set; }
        public string  EffectivenessActionDescription { get; set; }
        public string  Status { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitYn { get; set; }
        public bool FormExist { get; set; }
        public int? AuditedBy { get; set; }
        public string AuditedName { get; set; }
        public string AuditedDesig { get; set; }
        public DateTime? AuditedDt { get; set; }
        public bool? AuditedSign { get; set; }

        public string AuditedSignature { get; set; }
        

    }
}
