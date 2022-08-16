using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB9ResponseDTO
    {
        public int PkRefNo { get; set; }
        public int? RevisionNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public bool MaxRecord { get; set; }
        public List<FormB9HistoryResponseDTO> FormB9History { get; set; }

}
}
