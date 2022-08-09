using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB11DTO
    {
        public int B11hPkRefNo { get; set; }
        public string B11hRmuCode { get; set; }
        public string B11hRmuName { get; set; }
        public int? B11hRevisionNo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? B11hRevisionDate { get; set; }
        public int? B11hRevisionYear { get; set; }
        public int? B11hCrBy { get; set; }
        public string B11hCrByName { get; set; }
        public DateTime? B11hCrDt { get; set; }

        public List<FormB11CrewDayCostHeaderDTO> CrewDayCostHeader { get; set; }
        public List<FormB11LabourCostDTO> LabourCost { get; set; }
        public List<FormB11EquipmentCostDTO> EquipmentCost { get; set; }
        public List<FormB11MaterialCostDTO> MaterialCost { get; set; }

    }

    public class FormB11CrewDayCostHeaderDTO
    {
        public int B11cdchPkRefNo { get; set; }
        public int? B11cdchB11hPkRefNo { get; set; }
        public string B11cdchFeature { get; set; }
        public int? B11cdchActivityCode { get; set; }
        public string B11cdchActivityName { get; set; }
        public decimal? B11cdchCrewDayCost { get; set; }
    }

    public class FormB11LabourCostDTO
    {
        public int B11lcPkRefNo { get; set; }
        public int? B11lcB11hPkRefNo { get; set; }
        public int? B11lcActivityId { get; set; }
        public int? B11lcLabourId { get; set; }
        public decimal? B11lcLabourPerUnitPrice { get; set; }
        public int? B11lcLabourNoOfUnits { get; set; }
        public decimal? B11lcLabourTotalPrice { get; set; }
    }

    public class FormB11EquipmentCostDTO
    {
        public int B11ecPkRefNo { get; set; }
        public int? B11ecB11hPkRefNo { get; set; }
        public int? B11ecActivityId { get; set; }
        public int? B11ecEquipmentId { get; set; }
        public decimal? B11ecEquipmentPerUnitPrice { get; set; }
        public int? B11ecEquipmentNoOfUnits { get; set; }
        public decimal? B11ecEquipmentTotalPrice { get; set; }
    }

    public class FormB11MaterialCostDTO
    {
        public int B11mcPkRefNo { get; set; }
        public int? B11mcB11hPkRefNo { get; set; }
        public int? B11mcActivityId { get; set; }
        public int? B11mcMaterialId { get; set; }
        public decimal? B11mcMaterialPerUnitPrice { get; set; }
        public int? B11mcMaterialNoOfUnits { get; set; }
        public decimal? B11mcMaterialTotalPrice { get; set; }
    }
}
