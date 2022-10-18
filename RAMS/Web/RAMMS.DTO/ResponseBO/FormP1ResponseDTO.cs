using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormP1ResponseDTO
    {

      

        public int PkRefNo { get; set; }
        public int? hPkRefNo { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? PreviousPayment { get; set; }
        public decimal? TotalToDate { get; set; }
        public decimal? AmountIncludedInPc { get; set; }

     

    }
}
