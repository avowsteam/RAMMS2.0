﻿using System;
using AutoMapper.Configuration.Conventions;

namespace RAMMS.DTO.RequestBO
{
  public class FormUCUAHeaderRequestDTO
    {
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public string RMU { get; set; }
        public string RoadCode { get; set; }
        public string RoadName { get; set; }

        public int? TotalPC { get; set; }
        public int? TotalHV { get; set; }
        public int? TotalMC { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public bool? SubmitSts { get; set; }
        public string Recordedby { get; set; }
        public string Headedby { get; set; }
        public string ReportingName { get; set; }

        public string Location { get; set; }
        public string WorkScope { get; set; }
        public string ReceivedDtFrom { get; set; }
        public string ReceivedDtTo { get; set; }
    }
}
