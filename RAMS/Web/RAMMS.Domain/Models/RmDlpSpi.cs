using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmDlpSpi
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
    }
}
