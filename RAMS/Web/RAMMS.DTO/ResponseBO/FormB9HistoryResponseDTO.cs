using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB9HistoryResponseDTO
    {
        public int PkRefNo { get; set; }
        public int? B9dsPkRefNo { get; set; }
        public string Feature { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Cond1 { get; set; }
        public decimal? Cond2 { get; set; }
        public decimal? Cond3 { get; set; }
        public int? UnitOfService { get; set; }

        public string UnitDescription { get; set; }
        public string Remarks { get; set; }
        public int? RevisionNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RevisionDate { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }

    }
}
