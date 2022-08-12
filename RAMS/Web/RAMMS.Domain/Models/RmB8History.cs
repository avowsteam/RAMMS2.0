using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB8History
    {
        public int B8hiPkRefNo { get; set; }
        public int? B8hiB8hPkRefNo { get; set; }
        public int? B8hiItemNo { get; set; }
        public string B8hiDescription { get; set; }
        public int? B8hiUnit { get; set; }
        public string B8hiDivision { get; set; }

        public virtual RmB8Hdr B8hiB8hPkRefNoNavigation { get; set; }
    }
}
