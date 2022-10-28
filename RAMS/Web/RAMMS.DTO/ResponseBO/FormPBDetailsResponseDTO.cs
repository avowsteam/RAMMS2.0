using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormPBDetailResponseDTO
    {
        public int PkRefNo { get; set; }
        public int? PbiwPkRefNo { get; set; }
        public string IwRef { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string CompletionRefNo { get; set; }
        public decimal? AmountBeforeLad { get; set; }
        public decimal? LaDamage { get; set; }
        public decimal? FinalPayment { get; set; }

    }
}
