using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB14Hdr
    {
        public RmB14Hdr()
        {
            RmB14History = new HashSet<RmB14History>();
        }

        public int B14hPkRefNo { get; set; }
        public string B14hPkRefId { get; set; }
        public int? B14hRevisionNo { get; set; }
        public DateTime? B14hRevisionDate { get; set; }
        public int? B14hRevisionYear { get; set; }
        public string B14hRmuCode { get; set; }
        public string B14hRmuName { get; set; }
        public int? B14hUseridProsd { get; set; }
        public string B14hUserNameProsd { get; set; }
        public string B14hUserDesignationProsd { get; set; }
        public DateTime? B14hDtProsd { get; set; }
        public bool? B14hSignProsd { get; set; }
        public int? B14hUseridFclitd { get; set; }
        public string B14hUserNameFclitd { get; set; }
        public string B14hUserDesignationFclitd { get; set; }
        public DateTime? B14hDtFclitd { get; set; }
        public bool? B14hSignFclitd { get; set; }
        public int? B14hUseridAgrd { get; set; }
        public string B14hUserNameAgrd { get; set; }
        public string B14hUserDesignationAgrd { get; set; }
        public DateTime? B14hDtAgrd { get; set; }
        public bool? B14hSignAgrd { get; set; }
        public int? B14hUseridEndosd { get; set; }
        public string B14hUserNameEndosd { get; set; }
        public string B14hUserDesignationEndosd { get; set; }
        public DateTime? B14hDtEndosd { get; set; }
        public bool? B14hSignEndosd { get; set; }
        public int? B14hCrBy { get; set; }
        public DateTime? B14hCrDt { get; set; }
        public int? B14hModBy { get; set; }
        public DateTime? B14hModDt { get; set; }
        public bool B14hActiveYn { get; set; }
        public bool B14hSubmitSts { get; set; }
        public string B14hStatus { get; set; }
        public string B14hAuditlog { get; set; }

        public virtual ICollection<RmB14History> RmB14History { get; set; }
    }
}
