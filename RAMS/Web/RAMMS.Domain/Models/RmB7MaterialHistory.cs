using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7MaterialHistory
    {
        public int B7mhPkRefNo { get; set; }
        public int? B7mhB7hPkRefNo { get; set; }
        public string B7mhCode { get; set; }
        public string B7mhName { get; set; }
        public string B7mhUnits { get; set; }
        public decimal? B7mhUnitPriceBatuNiah { get; set; }
        public decimal? B7mhUnitPriceMiri { get; set; }
        public int? B7mhRevisionNo { get; set; }
        public DateTime? B7mhRevisionDate { get; set; }
        public int? B7mhRevisionYear { get; set; }
        public int? B7mhCrBy { get; set; }
        public string B7mhCrByName { get; set; }
        public DateTime? B7mhCrDt { get; set; }

        public virtual RmB7Hdr B7mhB7hPkRefNoNavigation { get; set; }
    }
}
