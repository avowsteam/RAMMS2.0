using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormPACRRResponseDTO
    {

        public int PkRefNo { get; set; }
        public int? PcmamwPkRefNo { get; set; }
        public string Division { get; set; }
        public decimal? Paved { get; set; }
        public decimal? Unpaved { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? ContractRate { get; set; }
        public decimal? TotalAmount { get; set; }


    }
}
