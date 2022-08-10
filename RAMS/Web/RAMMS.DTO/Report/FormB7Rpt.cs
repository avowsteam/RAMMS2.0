using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Report
{
    public class FormB7Rpt
    {
        public int? Year { get; set; }
        public List<Details> Labours { get; set; }

        public List<Details> Materials { get; set; }

        public List<Details> Equipments { get; set; }

    }


    public class Details
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string UnitPriceBatuNiah { get; set; }
        public string UnitPriceMiri { get; set; }
    }
}
