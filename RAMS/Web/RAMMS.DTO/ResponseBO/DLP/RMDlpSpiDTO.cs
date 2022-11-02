using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO.DLP
{
    public class RMDlpSpiDTO
    {
        public int SpiPkRefNo { get; set; }
        public int? SpiYear { get; set; }
        public int? SpiMonth { get; set; }
        public string SpiDivCode { get; set; }
        public string SpiDivName { get; set; }
        public decimal? SpiMPlanned { get; set; }
        public decimal? SpiMActual { get; set; }
        public decimal? SpiCPlan { get; set; }
        public decimal? SpiCActual { get; set; }
        public decimal? SpiPiWorkActual { get; set; }
        public decimal? SpiPai { get; set; }
        public decimal? SpiEff { get; set; }
        public decimal? SpiRb { get; set; }
        public decimal? SpiIw { get; set; }
        public decimal? SpiSpi { get; set; }
        public decimal? SpiPlannedPer { get; set; }
        public decimal? SpiActualPer { get; set; }
        public DateTime? SpiCreatedDate { get; set; }
        public DateTime? SpiUpdatedDate { get; set; }

        //public int SpplpPkRefNo { get; set; }
        //public int? SpplpYear { get; set; }
        //public int? SpplpMonth { get; set; }
        //public string SpplpDivCode { get; set; }
        //public string SpplpDivName { get; set; }
        //public decimal? SpplpMPlanned { get; set; }
        //public decimal? SpplpMActual { get; set; }
        //public decimal? SpplpCPlan { get; set; }
        //public decimal? SpplpCActual { get; set; }
        //public decimal? SpplpPiWorkActual { get; set; }
        //public decimal? SpplpPai { get; set; }
        //public decimal? SpplpEff { get; set; }
        //public decimal? SpplpRb { get; set; }
        //public decimal? SpplpIw { get; set; }
        //public decimal? SpplpSpi { get; set; }
        //public decimal? SpplpPlannedPer { get; set; }
        //public decimal? SpplpActualPer { get; set; }
        //public DateTime? SpplpCreatedDate { get; set; }
        //public DateTime? SpplpUpdatedDate { get; set; }
    }
}
