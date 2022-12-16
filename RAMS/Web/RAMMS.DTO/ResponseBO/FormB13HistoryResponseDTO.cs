﻿using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RAMMS.DTO.ResponseBO
{
    public class FormB13HistoryResponseDTO
    {
        public int PkRefNo { get; set; }
        public int? B13pPkRefNo { get; set; }
        public string Feature { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? InvCond1 { get; set; }
        public decimal? InvCond2 { get; set; }
        public decimal? InvCond3 { get; set; }
        public decimal? InvTotal { get; set; }
        public decimal? SlCond1 { get; set; }
        public decimal? SlCond2 { get; set; }
        public decimal? SlCond3 { get; set; }
        public decimal? AwqCond1 { get; set; }
        public decimal? AwqCond2 { get; set; }
        public decimal? AwqCond3 { get; set; }
        public decimal? AwqTotal { get; set; }
        public decimal? CrewDaysRequired { get; set; }
        public decimal? CdcLabour { get; set; }
        public decimal? CdcEquipment { get; set; }
        public decimal? CdcMaterial { get; set; }
        public decimal? CrewDaysCost { get; set; }
        public decimal? AverageDailyProduction { get; set; }
        public string UnitOfService { get; set; }
        public decimal? SlDesired { get; set; }
        public decimal? SlPlanned { get; set; }
        public decimal? SlAvgDesired { get; set; }
        public decimal? SlAnnualWorkQuantity { get; set; }
        public decimal? SlCrewDaysPlanned { get; set; }
        public decimal? SlAvgByActivity { get; set; }
        public decimal? SlTotalByActivity { get; set; }
        public decimal? SlPercentageByActivity { get; set; }
        public decimal? SlTotalByFeature { get; set; }

    }
}