using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public partial class FormMapHeaderDTO
    {
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public int? RevisionNo { get; set; }
        public string RmuCode { get; set; }
        public string RmuName { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? PreparedBy { get; set; }
        public bool? PreparedSign { get; set; }
        public string PreparedName { get; set; }
        public string PreparedDesig { get; set; }
        public string PreparedOffice { get; set; }
        public DateTime? PreparedDate { get; set; }
        public int? CheckedBy { get; set; }
        public bool? CheckedSign { get; set; }
        public string CheckedName { get; set; }
        public string CheckedDesig { get; set; }
        public string CheckedOffice { get; set; }
        public DateTime? CheckedDate { get; set; }
        public int? VerifiedBy { get; set; }
        public bool? VerifiedSign { get; set; }
        public string VerifiedName { get; set; }
        public string VerifiedDesig { get; set; }
        public string VerifiedOffice { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public bool? ActiveYn { get; set; }
        public bool? SubmitSts { get; set; }
        public string Status { get; set; }
        public string Auditlog { get; set; }

        public List<FormMapDetailsDTO> RmMapDetails { get; set; }
    }

    public class FormMapDetailsDTO {
        public int PkRefNoDetails { get; set; }
        public int? PkRefNo { get; set; }
        public int? ActivityId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public int? ActivityWeekNo { get; set; }
        public string ActivityWeekDay { get; set; }
        public int? ActivityWeekDayNo { get; set; }
        public string ActivityLocationCode { get; set; }
        public decimal? QuantityKm { get; set; }
        public string ProductUnit { get; set; }
        public int? Order { get; set; }

        public virtual FormMapHeaderDTO MapHeader { get; set; }
    }
}
