using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormP1HeaderResponseDTO
    {
        public FormP1HeaderResponseDTO()
        {
            FormP1Details = new List<FormP1ResponseDTO>();
        }
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public string SrProvider { get; set; }
        public string Bank { get; set; }
        public string BankAccNo { get; set; }
        public string Address { get; set; }
        public int? PaymentCertificateNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SubmissionDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContractsEndsOn { get; set; }
        public int? SubmissionYear { get; set; }
        public int? SubmissionMonth { get; set; }
        public decimal? ContractRoadLength { get; set; }
        public decimal? NetValueDeduction { get; set; }
        public decimal? NetValueAddition { get; set; }
        public decimal? NetValueInstructedWork { get; set; }
        public decimal? NetValueLadInstructedWork { get; set; }
        public decimal? TotalPayment { get; set; }
        public bool SignSo { get; set; }
        public int? UseridSo { get; set; }
        public string UsernameSo { get; set; }
        public string DesignationSo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SignDateSo { get; set; }
        public bool ActiveYn { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }
        public string AuditLog { get; set; }
       
        public List<FormP1ResponseDTO> FormP1Details { get; set; }

    }
}
