using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB13ResponseDTO
    {
        public FormB13ResponseDTO()
        {
            FormB13History = new List<FormB13HistoryResponseDTO>();
            FormB13RevisionHistory = new List<FormB13HistoryRevisionResponseDTO>();
        }
        public int PkRefNo { get; set; }
        public int? RevisionNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public string Rmu { get; set; }
        public string PkRefId { get; set; }
        public decimal? AdjustableQuantity { get; set; }
        public decimal? RoutineMaintenance { get; set; }
        public decimal? PeriodicMaintenance { get; set; }
        public decimal? OtherMaintenance { get; set; }
        public int? UseridProsd { get; set; }
        public string UserNameProsd { get; set; }
        public string UserDesignationProsd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtProsd { get; set; }
        public bool SignProsd { get; set; }
        public int? UseridFclitd { get; set; }
        public string UserNameFclitd { get; set; }
        public string UserDesignationFclitd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtFclitd { get; set; }
        public bool SignFclitd { get; set; }
        public int? UseridAgrd { get; set; }
        public string UserNameAgrd { get; set; }
        public string UserDesignationAgrd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtAgrd { get; set; }
        public bool SignAgrd { get; set; }
        public int? UseridEdosd { get; set; }
        public string UserNameEdosd { get; set; }
        public string UserDesignationEdosd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtEdosd { get; set; }
        public bool SignEdosd { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public bool SubmitSts { get; set; }
        public bool ActiveYn { get; set; }
        public string Status { get; set; }
        public string AuditLog { get; set; }
        public string Description { get; set; }
        public List<FormB13HistoryResponseDTO> FormB13History { get; set; }
        public List<FormB13HistoryRevisionResponseDTO> FormB13RevisionHistory { get; set; }
    }
}
