﻿using AutoMapper;
using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormMDTO
    {
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public string Dist { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string DivCode { get; set; }
        public string RmuCode { get; set; }
        public string RmuName { get; set; }
        public string SecCode { get; set; }
        public string SecName { get; set; }
        public string RdCode { get; set; }
        public string RdName { get; set; }
        public string ActCode { get; set; }
        public string ActName { get; set; }
        public string CrewSup { get; set; }
        public string Desc { get; set; }
        public int? LocChKm { get; set; }
        public int? LocChM { get; set; }
        public DateTime? AuditedDate { get; set; }
        public string AuditTimeFrm { get; set; }
        public string AuditTimeTo { get; set; }
        public string AuditedBy { get; set; }
        public string AuditType { get; set; }
        public string SrProvider { get; set; }
        public int? FinalResult { get; set; }
        public string FinalResultValue { get; set; }
        public string Findings { get; set; }
        public string CorrectiveActions { get; set; }
        public bool? SignAudit { get; set; }
        public int? UseridAudit { get; set; }
        public string UsernameAudit { get; set; }
        public string DesignationAudit { get; set; }
        public DateTime? DateAudit { get; set; }
        public string OfAudit { get; set; }
        public bool? SignWit { get; set; }
        public int? UseridWit { get; set; }
        public string UsernameWit { get; set; }
        public string DesignationWit { get; set; }
        public DateTime? DateWit { get; set; }
        public string OfWit { get; set; }

        public bool? SignWitone { get; set; }
        public int? UseridWitone { get; set; }
        public string UsernameWitone { get; set; }
        public string DesignationWitone { get; set; }
        public DateTime? DateWitone { get; set; }
        public string OfWitone { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public bool SubmitSts { get; set; }
        public bool ActiveYn { get; set; }
        public string status { get; set; }
        public string AuditLog { get; set; }

        public FormMAuditDetailsDTO FormMAD { get; set; }
    }
    public class FormMAuditDetailsDTO
    {
        public int PkRefNo { get; set; }
        public int? FmhPkRefNo { get; set; }
        public int? Category { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public string ActivityFor { get; set; }
        public bool? IsEditable { get; set; }
        public int? TallyBox { get; set; }
        public int? Weightage { get; set; }
        public int? Total { get; set; }
        public int? A1tallyBox { get; set; }
        public int? A1total { get; set; }
        public int? A2tallyBox { get; set; }
        public int? A2total { get; set; }
        public int? A3tallyBox { get; set; }
        public int? A3total { get; set; }
        public int? A4tallyBox { get; set; }
        public int? A4total { get; set; }
        public int? A5tallyBox { get; set; }
        public int? A5total { get; set; }
        public int? A6tallyBox { get; set; }
        public int? A6total { get; set; }
        public int? A7tallyBox { get; set; }
        public int? A7total { get; set; }
        public int? A8tallyBox { get; set; }
        public int? A8total { get; set; }
        public int? B1tallyBox { get; set; }
        public int? B1total { get; set; }
        public int? B2tallyBox { get; set; }
        public int? B2total { get; set; }
        public int? B3tallyBox { get; set; }
        public int? B3total { get; set; }
        public int? B4tallyBox { get; set; }
        public int? B4total { get; set; }
        public int? B5tallyBox { get; set; }
        public int? B5total { get; set; }
        public int? B6tallyBox { get; set; }
        public int? B6total { get; set; }
        public int? B7tallyBox { get; set; }
        public int? B7total { get; set; }
        public int? B8tallyBox { get; set; }
        public int? B8total { get; set; }
        public int? B9tallyBox { get; set; }
        public int? B9total { get; set; }
        public int? C1tallyBox { get; set; }
        public int? C1total { get; set; }
        public int? C2tallyBox { get; set; }
        public int? C2total { get; set; }
        public int? D1tallyBox { get; set; }
        public int? D1total { get; set; }
        public int? D2tallyBox { get; set; }
        public int? D2total { get; set; }
        public int? D3tallyBox { get; set; }
        public int? D3total { get; set; }
        public int? D4tallyBox { get; set; }
        public int? D4total { get; set; }
        public int? D5tallyBox { get; set; }
        public int? D5total { get; set; }
        public int? D6tallyBox { get; set; }
        public int? D6total { get; set; }
        public int? D7tallyBox { get; set; }
        public int? D7total { get; set; }
        public int? D8tallyBox { get; set; }
        public int? D8total { get; set; }
        public int? E1tallyBox { get; set; }
        public int? E1total { get; set; }
        public int? E2tallyBox { get; set; }
        public int? E2total { get; set; }
        public int? F1tallyBox { get; set; }
        public int? F1total { get; set; }
        public int? F2tallyBox { get; set; }
        public int? F2total { get; set; }
        public int? F3tallyBox { get; set; }
        public int? F3total { get; set; }
        public int? F4tallyBox { get; set; }
        public int? F4total { get; set; }
        public int? F5tallyBox { get; set; }
        public int? F5total { get; set; }
        public int? F6tallyBox { get; set; }
        public int? F6total { get; set; }
        public int? F7tallyBox { get; set; }
        public int? F7total { get; set; }
        public int? G1tallyBox { get; set; }
        public int? G1total { get; set; }
        public int? G2tallyBox { get; set; }
        public int? G2total { get; set; }
        public int? G3tallyBox { get; set; }
        public int? G3total { get; set; }
        public int? G4tallyBox { get; set; }
        public int? G4total { get; set; }
        public int? G5tallyBox { get; set; }
        public int? G5total { get; set; }
        public int? G6tallyBox { get; set; }
        public int? G6total { get; set; }
        public int? G7tallyBox { get; set; }
        public int? G7total { get; set; }
        public int? G8tallyBox { get; set; }
        public int? G8total { get; set; }
        public int? G9tallyBox { get; set; }
        public int? G9total { get; set; }
        public int? G10tallyBox { get; set; }
        public int? G10total { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public string OtherSign { get; set; }
        public string MiscellanousSign { get; set; }
    }

}
