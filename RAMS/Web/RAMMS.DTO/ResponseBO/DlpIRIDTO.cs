using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class DlpIRIDTO
    {
        public int RmiiriPkRefNo { get; set; }
        public int? RmiiriYear { get; set; }
        public int? RmiiriMonth { get; set; }
        public int? RmiiriConditionNo { get; set; }
        public string RmiiriType { get; set; }
        public decimal? RmiiriPercentage { get; set; }
        public string _RmiiriPercentage { get; set; }

        public decimal? RmiiriRoadLength { get; set; }
        public string _RmiiriRoadLength { get; set; }

        public DateTime? RmiiriCreatedDate { get; set; }
        public DateTime? RmiiriUpdatedDate { get; set; }

        public decimal? RmiiriPercentage1 { get; set; }
        public decimal? RmiiriRoadLength1 { get; set; }

        public string _RmiiriPercentage1 { get; set; }
        public string _RmiiriRoadLength1 { get; set; }

        public decimal? RmiiriPercentage2 { get; set; }
        public decimal? RmiiriRoadLength2 { get; set; }

        public string _RmiiriPercentage2 { get; set; }
        public string _RmiiriRoadLength2 { get; set; }

        public decimal? RmiiriPercentage3 { get; set; }
        public decimal? RmiiriRoadLength3 { get; set; }

        public string _RmiiriPercentage3 { get; set; }
        public string _RmiiriRoadLength3 { get; set; }
    }
}
