using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public partial class FormB14HeaderDTO
    {
        public int PkRefNo { get; set; }
        public string PkRefId { get; set; }
        public int? RevisionNo { get; set; }
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public string RmuCode { get; set; }
        public string RmuName { get; set; }
        public int? UseridProsd { get; set; }
        public string UserNameProsd { get; set; }
        public string UserDesignationProsd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtProsd { get; set; }
        public bool? SignProsd { get; set; }
        public int? UseridFclitd { get; set; }
        public string UserNameFclitd { get; set; }
        public string UserDesignationFclitd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}")]
        public DateTime? DtFclitd { get; set; }
        public bool? SignFclitd { get; set; }
        public int? UseridAgrd { get; set; }
        public string UserNameAgrd { get; set; }
        public string UserDesignationAgrd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}")]
        public DateTime? DtAgrd { get; set; }
        public bool? SignAgrd { get; set; }
        public int? UseridEndosd { get; set; }
        public string UserNameEndosd { get; set; }
        public string UserDesignationEndosd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-YYYY}")]
        public DateTime? DtEndosd { get; set; }
        public bool? SignEndosd { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }
        public string Auditlog { get; set; }

        public  List<FormB14HistoryDTO> RmB14History { get; set; }
    }

    public class FormB14HistoryDTO
    {
        public int PkRefNoHistory { get; set; }
        public int? B14hPkRefNo { get; set; }
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

        public virtual FormB14HeaderDTO B14Header { get; set; }
    }


    public class FormAWPBDTO
    {
        public int RefNo { get; set; }
        public string RMU { get; set; }
        public string Feature { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
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
        public string Unit { get; set; }
        public decimal? SubTotal { get; set; }
        public int Order { get; set; }
        public virtual FormB14HeaderDTO B14Header { get; set; }
    }

    }
