﻿using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmFormS2QuarDtl
    {
        public RmFormS2QuarDtl()
        {
            RmFormS2DaySchedule = new HashSet<RmFormS2DaySchedule>();
        }

        public int FsiiqdPkRefNo { get; set; }
        public int? FsiiqdFsiidPkRefNo { get; set; }
        public int? FsiiqdClkPkRefNo { get; set; }
        public int? FsiiqdCrBy { get; set; }
        public DateTime? FsiiqdCrDt { get; set; }

        public virtual RmWeekLookup FsiiqdClkPkRefNoNavigation { get; set; }
        public virtual RmFormS2Dtl FsiiqdFsiidPkRefNoNavigation { get; set; }
        public virtual ICollection<RmFormS2DaySchedule> RmFormS2DaySchedule { get; set; }
    }
}
