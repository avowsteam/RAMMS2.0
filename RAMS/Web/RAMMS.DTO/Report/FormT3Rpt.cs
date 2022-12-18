﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Report
{
    public class FormT3Rpt
    {
        public int? RevisionNo { get; set; }
        public DateTime? RevisionDate { get; set; }
        public int? RevisionYear { get; set; }
        public string RmuCode { get; set; }
        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }
        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }
        public string UnitOfService { get; set; }
        public decimal? SubTotal { get; set; }
        public string Remarks { get; set; }
    }
}
