using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.RequestBO
{
    public class ProcessDTO
    {
        public int RefId { get; set; }
        public string Form { get; set; }
        public string Stage { get; set; }
        public bool IsApprove { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserDesignation { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Remarks { get; set; }
    }

    public class EmailFromD
    {
        public string RefNo { get; set; }
        public string Title { get; set; }
        public bool IsEmail { get; set; }
        public int VerifierId { get; set; }
        public int SubmittedByUserId { get; set; }
    }
}
