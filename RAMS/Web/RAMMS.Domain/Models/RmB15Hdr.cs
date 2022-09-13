using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB15Hdr
    {
        public RmB15Hdr()
        {
            RmB15History = new HashSet<RmB15History>();
        }

        public int B15hPkRefNo { get; set; }
        public string B15hPkRefId { get; set; }
        public int? B15hRevisionNo { get; set; }
        public DateTime? B15hRevisionDate { get; set; }
        public int? B15hRevisionYear { get; set; }
        public string B15hRmuCode { get; set; }
        public string B15hRmuName { get; set; }
        public int? B15hUseridProsd { get; set; }
        public string B15hUserNameProsd { get; set; }
        public string B15hUserDesignationProsd { get; set; }
        public DateTime? B15hDtProsd { get; set; }
        public bool? B15hSignProsd { get; set; }
        public int? B15hUseridFclitd { get; set; }
        public string B15hUserNameFclitd { get; set; }
        public string B15hUserDesignationFclitd { get; set; }
        public DateTime? B15hDtFclitd { get; set; }
        public bool? B15hSignFclitd { get; set; }
        public int? B15hUseridAgrd { get; set; }
        public string B15hUserNameAgrd { get; set; }
        public string B15hUserDesignationAgrd { get; set; }
        public DateTime? B15hDtAgrd { get; set; }
        public bool? B15hSignAgrd { get; set; }
        public int? B15hUseridEndosd { get; set; }
        public string B15hUserNameEndosd { get; set; }
        public string B15hUserDesignationEndosd { get; set; }
        public DateTime? B15hDtEndosd { get; set; }
        public bool? B15hSignEndosd { get; set; }
        public int? B15hCrBy { get; set; }
        public DateTime? B15hCrDt { get; set; }
        public int? B15hModBy { get; set; }
        public DateTime? B15hModDt { get; set; }
        public bool B15hActiveYn { get; set; }
        public bool B15hSubmitSts { get; set; }
        public string B15hStatus { get; set; }
        public string B15hAuditlog { get; set; }

        public virtual ICollection<RmB15History> RmB15History { get; set; }
    }
}
