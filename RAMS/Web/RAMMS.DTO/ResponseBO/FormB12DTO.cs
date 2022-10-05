using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public  class FormB12DTO
    {
        public int PkRefNo { get; set; }

        public string PkRefId { get; set; }
        public int? RevisionNo { get; set; }
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public int? CrBy { get; set; }
        public string CrByName { get; set; }
        public DateTime? CrDt { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }
        public string Auditlog { get; set; }

        public  List<FormB12HistoryDTO> FormB12History { get; set; }
    }

    public  class FormB12HistoryDTO
    {
        public int PkRefNo { get; set; }
        public int? B12hPkRefNo { get; set; }
        public string Feature { get; set; }
        public string ActCode { get; set; }
        public string ActName { get; set; }
        public decimal? RmuMiri { get; set; }
        public decimal? RmuBatuniah { get; set; }
        public string UnitOfService { get; set; }

        public  FormB12DTO FormB12 { get; set; }
    }
}
