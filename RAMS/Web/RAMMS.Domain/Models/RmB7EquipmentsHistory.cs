using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7EquipmentsHistory
    {
        public int B7ehPkRefNo { get; set; }
        public int? B7ehB7hPkRefNo { get; set; }
        public string B7ehCode { get; set; }
        public string B7ehName { get; set; }
        public int? B7ehUnitInHrs { get; set; }
        public decimal? B7ehUnitPriceBatuNiah { get; set; }
        public decimal? B7ehUnitPriceMiri { get; set; }
        public int? B7ehRevisionNo { get; set; }
        public DateTime? B7ehRevisionDate { get; set; }
        public int? B7ehRevisionYear { get; set; }
        public int? B7ehCrBy { get; set; }
        public string B7ehCrByName { get; set; }
        public DateTime? B7ehCrDt { get; set; }

        public virtual RmB7Hdr B7ehB7hPkRefNoNavigation { get; set; }
    }
}
