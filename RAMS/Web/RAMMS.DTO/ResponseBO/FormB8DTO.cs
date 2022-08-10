using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB8HeaderDTO
    {
        public int B8hPkRefNo { get; set; }
        public int? B8hRevisionNo { get; set; }
        public DateTime? B8hRevisionDate { get; set; }
        public int? B8hRevisionYear { get; set; }
        public int? B8hCrBy { get; set; }
        public string B8hCrByName { get; set; }
        public DateTime? B8hCrDt { get; set; }

        public List<FormB8HistoryDTO> RmB8History { get; set; }

    }

    public class FormB8HistoryDTO
    {
        public int B8hiPkRefNo { get; set; }
        public int? B8hiB8hPkRefNo { get; set; }
        public int? B8hiItemNo { get; set; }
        public string B8hiDescription { get; set; }
        public int? B8hiUnit { get; set; }
        public string B8hiDivision { get; set; }

        public FormB8HeaderDTO B8hiB8hPkRefNoNavigation { get; set; }

    }

 
   
}
