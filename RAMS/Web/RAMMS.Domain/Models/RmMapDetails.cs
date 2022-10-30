using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmMapDetails
    {
        public int RmmdPkRefNoDetails { get; set; }
        public int? RmmdRmmhPkRefNo { get; set; }
        public int? RmmdActivityId { get; set; }
        public DateTime? RmmdActivityDate { get; set; }
        public int? RmmdActivityWeekNo { get; set; }
        public string RmmdActivityWeekDay { get; set; }
        public int? RmmdActivityWeekDayNo { get; set; }
        public string RmmdActivityLocationCode { get; set; }
        public decimal? RmmdQuantityKm { get; set; }
        public string RmmdProductUnit { get; set; }
        public int? RmmdOrder { get; set; }
    }
}
