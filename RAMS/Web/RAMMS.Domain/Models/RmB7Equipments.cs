using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB7Equipments
    {
        public RmB7Equipments()
        {
            RmB7EquipmentsHistory = new HashSet<RmB7EquipmentsHistory>();
        }

        public int B7ePkRefNo { get; set; }
        public int? B7eRevisionNo { get; set; }
        public DateTime? B7eRevisionDate { get; set; }
        public int? B7eRevisionYear { get; set; }
        public int? B7eCrBy { get; set; }
        public string B7eCrByName { get; set; }
        public DateTime? B7eCrDt { get; set; }

        public virtual ICollection<RmB7EquipmentsHistory> RmB7EquipmentsHistory { get; set; }
    }
}
