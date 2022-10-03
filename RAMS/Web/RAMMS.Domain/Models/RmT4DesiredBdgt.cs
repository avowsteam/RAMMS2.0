using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmT4DesiredBdgt
    {
        public int T4dbPkRefNo { get; set; }
        public int? T4dbT4pdbhPkRefNo { get; set; }
        public string T4dbFeature { get; set; }
        public string T4dbCode { get; set; }
        public string T4dbName { get; set; }
        public decimal? T4dbInvCond1 { get; set; }
        public decimal? T4dbInvCond2 { get; set; }
        public decimal? T4dbInvCond3 { get; set; }
        public decimal? T4dbInvTotal { get; set; }
        public decimal? T4dbSlCond1 { get; set; }
        public decimal? T4dbSlCond2 { get; set; }
        public decimal? T4dbSlCond3 { get; set; }
        public decimal? T4dbSlAsl { get; set; }
        public decimal? T4dbAwqCond1 { get; set; }
        public decimal? T4dbAwqCond2 { get; set; }
        public decimal? T4dbAwqCond3 { get; set; }
        public decimal? T4dbAwqTotal { get; set; }
        public decimal? T4dbAverageDailyProduction { get; set; }
        public string T4dbUnitOfService { get; set; }
        public decimal? T4dbCrewDaysRequired { get; set; }
        public decimal? T4dbCdcLabour { get; set; }
        public decimal? T4dbCdcEquipment { get; set; }
        public decimal? T4dbCdcMaterial { get; set; }
        public decimal? T4dbCrewDaysCost { get; set; }
        public decimal? T4dbDbActTotal { get; set; }
        public decimal? T4dbDbActPercentage { get; set; }
        public decimal? T4dbDbFeatureTotal { get; set; }
        public decimal? T4dbDbFeaturePercentage { get; set; }

        public virtual RmT4DesiredBdgtHeader T4dbT4pdbhPkRefNoNavigation { get; set; }
    }
}
