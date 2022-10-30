using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmMapHeader
    {
        public RmMapHeader()
        {
            RmMapDetails = new HashSet<RmMapDetails>();
        }

        public int RmmhPkRefNo { get; set; }
        public string RmmhRefId { get; set; }
        public int? RmmhRevisionNo { get; set; }
        public string RmmhRmuCode { get; set; }
        public string RmmhRmuName { get; set; }
        public int? RmmhYear { get; set; }
        public int? RmmhMonth { get; set; }
        public DateTime? RmmhCreatedDate { get; set; }
        public int? RmmhPreparedBy { get; set; }
        public bool? RmmhPreparedSign { get; set; }
        public string RmmhPreparedName { get; set; }
        public string RmmhPreparedDesig { get; set; }
        public string RmmhPreparedOffice { get; set; }
        public DateTime? RmmhPreparedDate { get; set; }
        public int? RmmhCheckedBy { get; set; }
        public bool? RmmhCheckedSign { get; set; }
        public string RmmhCheckedName { get; set; }
        public string RmmhCheckedDesig { get; set; }
        public string RmmhCheckedOffice { get; set; }
        public DateTime? RmmhCheckedDate { get; set; }
        public int? RmmhVerifiedBy { get; set; }
        public bool? RmmhVerifiedSign { get; set; }
        public string RmmhVerifiedName { get; set; }
        public string RmmhVerifiedDesig { get; set; }
        public string RmmhVerifiedOffice { get; set; }
        public DateTime? RmmhVerifiedDate { get; set; }
        public int? RmmhCrBy { get; set; }
        public DateTime? RmmhCrDt { get; set; }
        public int? RmmhModBy { get; set; }
        public DateTime? RmmhModDt { get; set; }
        public bool? RmmhActiveYn { get; set; }
        public bool? RmmhSubmitSts { get; set; }
        public string RmmhStatus { get; set; }
        public string RmmhAuditlog { get; set; }

        public virtual ICollection<RmMapDetails> RmMapDetails { get; set; }
    }
}
