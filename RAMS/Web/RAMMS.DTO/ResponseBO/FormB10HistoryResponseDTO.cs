using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB10HistoryResponseDTO
    {
        public int PkRefNo { get; set; }
        public int? B10dpPkRefNo { get; set; }
        public string Feature { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? AdpValue { get; set; }
        public decimal? AdpUnit { get; set; }
        public decimal? AdpUnitDescription { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
         
    }
}
