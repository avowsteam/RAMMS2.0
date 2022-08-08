using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7LabourHistory
    {
        public int B7lhPkRefNo { get; set; }
        public int? B7lhB7lPkRefNo { get; set; }
        public string B7lhCode { get; set; }
        public string B7lhName { get; set; }
        public int? B7lhUnitInHrs { get; set; }
        public decimal? B7lhUnitPriceBatuNiah { get; set; }
        public decimal? B7lhUnitPriceMiri { get; set; }
        public int? B7lhRevisionNo { get; set; }
        public DateTime? B7lhRevisionDate { get; set; }
        public int? B7lhRevisionYear { get; set; }
        public int? B7lhCrBy { get; set; }
        public string B7lhCrByName { get; set; }
        public DateTime? B7lhCrDt { get; set; }

        public virtual RmB7Labour B7lhB7lPkRefNoNavigation { get; set; }
    }
}
