using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Report
{
    public class FormB12Rpt
    {
        public int? Year { get; set; }
        public List<B12History> B12History { get; set; }

    }


    public class B12History
    {
        public string Feature { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string UnitPriceBatuNiah { get; set; }
        public string UnitPriceMiri { get; set; }
    }
}
