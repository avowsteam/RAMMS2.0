﻿using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmMapDetails
    {
        public int RmmdPkRefNo { get; set; }
        public int? RmmdRmmhPkRefNo { get; set; }
        public int? RmmdActivityId { get; set; }
        public DateTime? RmmdActivityDate { get; set; }
        public int? RmmdActivityWeekNo { get; set; }
        public string RmmdActivityWeekDay { get; set; }
        public int? RmmdActivityWeekDayNo { get; set; }
        public string RmmdActivityLocationCode { get; set; }
        public decimal? RmmdQuantityKm { get; set; }

        public virtual RmMapHeader RmmdRmmhPkRefNoNavigation { get; set; }
    }
}