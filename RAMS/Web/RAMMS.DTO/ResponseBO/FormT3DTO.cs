using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public partial class FormT3HeaderDTO
    {
        public int PkRefNo { get; set; }
        public string PkRefId { get; set; }
        public int? RevisionNo { get; set; }
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public string RmuCode { get; set; }
        public string RmuName { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }
        public string Auditlog { get; set; }

        public List<FormT3HistoryDTO> RmT3History { get; set; }
    }

    public class FormT3HistoryDTO
    {
        public int PkRefNoHistory { get; set; }
        public int? T3hPkRefNo { get; set; }
        public int? ActId { get; set; }
        public string Feature { get; set; }
        public string ActCode { get; set; }
        public string ActName { get; set; }
        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }
        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }
        public string UnitOfService { get; set; }
        public decimal? SubTotal { get; set; }
        public int Order { get; set; }
        public string Remarks { get; set; }

        public virtual FormT3HeaderDTO T3Header { get; set; }
    }
}
