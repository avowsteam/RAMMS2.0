﻿using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB11LabourCost
    {
        public int B11lcPkRefNo { get; set; }
        public int? B11lcB11hPkRefNo { get; set; }
        public int? B11lcActivityId { get; set; }
        public string B11lcLabourId { get; set; }
        public string B11lcLabourName { get; set; }
        public decimal? B11lcLabourPerUnitPrice { get; set; }
        public int? B11lcLabourNoOfUnits { get; set; }
        public decimal? B11lcLabourTotalPrice { get; set; }
        public int? B11lcLabourOrderId { get; set; }

        public virtual RmB11Hdr B11lcB11hPkRefNoNavigation { get; set; }
    }
}
