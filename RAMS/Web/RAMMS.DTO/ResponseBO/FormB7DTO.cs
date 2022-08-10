using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB7HeaderDTO
    {
        public int B7hPkRefNo { get; set; }
        public int? B7hRevisionNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? B7hRevisionDate { get; set; }
        public int? B7hRevisionYear { get; set; }
        public int? B7hCrBy { get; set; }
        public string B7hCrByName { get; set; }
        public DateTime? B7hCrDt { get; set; }

        public List<FormB7EquipmentsHistoryDTO> RmB7EquipmentsHistory { get; set; }
        public List<FormB7LabourHistoryDTO> RmB7LabourHistory { get; set; }
        public List<FormB7MaterialHistoryDTO> RmB7MaterialHistory { get; set; }
    }

    public class FormB7LabourHistoryDTO
    {
        public int B7lhPkRefNo { get; set; }
        public int? B7lhB7hPkRefNo { get; set; }
        public string B7lhCode { get; set; }
        public string B7lhName { get; set; }
        public int? B7lhUnitInHrs { get; set; }
        public decimal? B7lhUnitPriceBatuNiah { get; set; }
        public decimal? B7lhUnitPriceMiri { get; set; }
        public int? B7lhRevisionNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? B7lhRevisionDate { get; set; }
        public int? B7lhRevisionYear { get; set; }
        public int? B7lhCrBy { get; set; }
        public string B7lhCrByName { get; set; }
        public DateTime? B7lhCrDt { get; set; }

    }

 
    public class FormB7MaterialHistoryDTO
    {
        public int B7mhPkRefNo { get; set; }
        public int? B7mhB7hPkRefNo { get; set; }
        public string B7mhCode { get; set; }
        public string B7mhName { get; set; }
        public string B7mhUnits { get; set; }
        public decimal? B7mhUnitPriceBatuNiah { get; set; }
        public decimal? B7mhUnitPriceMiri { get; set; }
        public int? B7mhRevisionNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? B7mhRevisionDate { get; set; }
        public int? B7mhRevisionYear { get; set; }
        public int? B7mhCrBy { get; set; }
        public string B7mhCrByName { get; set; }
        public DateTime? B7mhCrDt { get; set; }


    }

    public  class FormB7EquipmentsHistoryDTO
    {
        public int B7ehPkRefNo { get; set; }
        public int? B7ehB7hPkRefNo { get; set; }
        public string B7ehCode { get; set; }
        public string B7ehName { get; set; }
        public int? B7ehUnitInHrs { get; set; }
        public decimal? B7ehUnitPriceBatuNiah { get; set; }
        public decimal? B7ehUnitPriceMiri { get; set; }
        public int? B7ehRevisionNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? B7ehRevisionDate { get; set; }
        public int? B7ehRevisionYear { get; set; }
        public int? B7ehCrBy { get; set; }
        public string B7ehCrByName { get; set; }
        public DateTime? B7ehCrDt { get; set; }

    }
}
