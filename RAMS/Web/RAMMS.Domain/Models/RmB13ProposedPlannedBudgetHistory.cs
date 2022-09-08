using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmB13ProposedPlannedBudgetHistory
    {
        public int B13phPkRefNo { get; set; }
        public int? B13phB13pPkRefNo { get; set; }
        public string B13phFeature { get; set; }
        public string B13phCode { get; set; }
        public string B13phName { get; set; }
        public decimal? B13phInvCond1 { get; set; }
        public decimal? B13phInvCond2 { get; set; }
        public decimal? B13phInvCond3 { get; set; }
        public decimal? B13phInvTotal { get; set; }
        public decimal? B13phSlCond1 { get; set; }
        public decimal? B13phSlCond2 { get; set; }
        public decimal? B13phSlCond3 { get; set; }
        public decimal? B13phAwqCond1 { get; set; }
        public decimal? B13phAwqCond2 { get; set; }
        public decimal? B13phAwqCond3 { get; set; }
        public decimal? B13phAwqTotal { get; set; }
        public decimal? B13phCrewDaysRequired { get; set; }
        public decimal? B13phCdcLabour { get; set; }
        public decimal? B13phCdcEquipment { get; set; }
        public decimal? B13phCdcMaterial { get; set; }
        public decimal? B13phCrewDaysCost { get; set; }
        public decimal? B13phAverageDailyProduction { get; set; }
        public int? B13phUnitOfService { get; set; }
        public decimal? B13phSlDesired { get; set; }
        public decimal? B13phSlPlanned { get; set; }
        public decimal? B13phSlAvgDesired { get; set; }
        public decimal? B13phSlAnnualWorkQuantity { get; set; }
        public decimal? B13phSlCrewDaysPlanned { get; set; }
        public decimal? B13phSlAvgByActivity { get; set; }
        public decimal? B13phSlTotalByActivity { get; set; }
        public decimal? B13phSlPercentageByActivity { get; set; }
        public decimal? B13phSlTotalByFeature { get; set; }

        public virtual RmB13ProposedPlannedBudget B13phB13pPkRefNoNavigation { get; set; }
    }
}
