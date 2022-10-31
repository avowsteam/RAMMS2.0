﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAMMS.Web.UI.Models
{
    public class DlpIRIModel
    {
        public int RmiiriPkRefNo { get; set; }
        public int? RmiiriYear { get; set; }
        public int? RmiiriMonth { get; set; }
        public int? RmiiriConditionNo { get; set; }
        public string RmiiriType { get; set; }
        public decimal? RmiiriPercentage { get; set; }
        public decimal? RmiiriRoadLength { get; set; }
        public DateTime? RmiiriCreatedDate { get; set; }
        public DateTime? RmiiriUpdatedDate { get; set; }

        public decimal? RmiiriPercentage1 { get; set; }
        public decimal? RmiiriRoadLength1 { get; set; }

        public decimal? RmiiriPercentage2 { get; set; }
        public decimal? RmiiriRoadLength2 { get; set; }

        public decimal? RmiiriPercentage3 { get; set; }
        public decimal? RmiiriRoadLength3 { get; set; }
    }
}
