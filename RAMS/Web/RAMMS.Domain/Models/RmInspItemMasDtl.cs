﻿using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmInspItemMasDtl
    {
        public int IimdPkRefNo { get; set; }
        public int? IimdIimPkRefNo { get; set; }
        public string IimdInspCode { get; set; }
        public string IimdInspCodeDesc { get; set; }
        public int? IimModBy { get; set; }
        public DateTime? IimModDt { get; set; }
        public int? IimCrBy { get; set; }
        public DateTime? IimCrDt { get; set; }
        public bool IimSubmitSts { get; set; }
        public bool? IimActiveYn { get; set; }

        public virtual RmInspItemMas IimdIimPkRefNoNavigation { get; set; }
    }
}
