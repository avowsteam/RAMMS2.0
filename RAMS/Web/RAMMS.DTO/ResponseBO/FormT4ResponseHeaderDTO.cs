using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormT4HeaderResponseDTO
    {

        public FormT4HeaderResponseDTO()
        {
            FormT4 = new List<FormT4ResponseDTO>();
        }
        public int PkRefNo { get; set; }
        public string PkRefId { get; set; }
        public int? RevisionNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public string Rmu { get; set; }
        public decimal? AdjustableQuantity { get; set; }
        public decimal? RoutineMaintenance { get; set; }
        public decimal? PeriodicMaintenance { get; set; }
        public decimal? OtherMaintenance { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public bool SubmitSts { get; set; }
        public bool ActiveYn { get; set; }
        public string Status { get; set; }
        public string AuditLog { get; set; }

        public List<FormT4ResponseDTO> FormT4 { get; set; }

    }
}
