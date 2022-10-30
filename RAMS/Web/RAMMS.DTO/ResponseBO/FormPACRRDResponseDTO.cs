using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormPACRRDResponseDTO
    {

        public int PkRefNo { get; set; }
        public int? PcmamwPkRefNo { get; set; }
        public string Description { get; set; }
        public decimal? ThisPayment { get; set; }
        public decimal? TillLastPayment { get; set; }
        public decimal? TotalToDate { get; set; }

    }
}
