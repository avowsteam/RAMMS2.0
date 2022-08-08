using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB7LabourDTO
    {
        public int B7lPkRefNo { get; set; }
        public int? B7lRevisionNo { get; set; }
        public DateTime? B7lRevisionDate { get; set; }
        public int? B7lRevisionYear { get; set; }
        public int? B7lCrBy { get; set; }
        public string B7lCrByName { get; set; }
        public DateTime? B7lCrDt { get; set; }

        public virtual ICollection<FormB7LabourHistoryDTO> B7LabourHistory { get; set; }
    }

    public class FormB7LabourHistoryDTO
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

        public virtual FormB7LabourDTO B7lhB7lPkRefNoNavigation { get; set; }
    }

    public class FormB7MaterialDTO
    {
       
        public int B7mPkRefNo { get; set; }
        public int? B7mRevisionNo { get; set; }
        public DateTime? B7mRevisionDate { get; set; }
        public int? B7mRevisionYear { get; set; }
        public int? B7mCrBy { get; set; }
        public string B7mCrByName { get; set; }
        public DateTime? B7mCrDt { get; set; }

        public virtual ICollection<FormB7MaterialHistoryDTO> RmB7MaterialHistory { get; set; }
    }

    public class FormB7MaterialHistoryDTO
    {
        public int B7mhPkRefNo { get; set; }
        public int? B7mhB7mPkRefNo { get; set; }
        public string B7mhCode { get; set; }
        public string B7mhName { get; set; }
        public int? B7mhUnits { get; set; }
        public decimal? B7mhUnitPriceBatuNiah { get; set; }
        public decimal? B7mhUnitPriceMiri { get; set; }
        public int? B7mhRevisionNo { get; set; }
        public DateTime? B7mhRevisionDate { get; set; }
        public int? B7mhRevisionYear { get; set; }
        public int? B7mhCrBy { get; set; }
        public string B7mhCrByName { get; set; }
        public DateTime? B7mhCrDt { get; set; }

        public virtual FormB7MaterialDTO B7mhB7mPkRefNoNavigation { get; set; }
    }

    public  class FormB7EquipmentsDTO
    {     
        public int B7ePkRefNo { get; set; }
        public int? B7eRevisionNo { get; set; }
        public DateTime? B7eRevisionDate { get; set; }
        public int? B7eRevisionYear { get; set; }
        public int? B7eCrBy { get; set; }
        public string B7eCrByName { get; set; }
        public DateTime? B7eCrDt { get; set; }

        public virtual ICollection<FormB7EquipmentsHistoryDTO> RmB7EquipmentsHistory { get; set; }
    }

    public  class FormB7EquipmentsHistoryDTO
    {
        public int B7ehPkRefNo { get; set; }
        public int? B7ehB7ePkRefNo { get; set; }
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

        public virtual FormB7EquipmentsDTO B7ehB7ePkRefNoNavigation { get; set; }
    }
}
