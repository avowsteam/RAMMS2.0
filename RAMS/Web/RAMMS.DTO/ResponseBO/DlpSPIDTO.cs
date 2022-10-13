using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class DlpSPIDTO
    {
        public int PkRefNo { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string DivCode { get; set; }
        public string DivName { get; set; }
        public decimal? MPlanned { get; set; }
        public decimal? MActual { get; set; }
        public decimal? CPlan { get; set; }
        public decimal? CActual { get; set; }
        public decimal? PiWorkActual { get; set; }
        public decimal? Pai { get; set; }
        public decimal? Eff { get; set; }
        public decimal? Rb { get; set; }
        public decimal? Iw { get; set; }
        public decimal? Spi { get; set; }
        public decimal? PlannedPer { get; set; }
        public decimal? ActualPer { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
 