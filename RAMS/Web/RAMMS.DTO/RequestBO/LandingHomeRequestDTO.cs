﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.RequestBO
{
    public class LandingHomeRequestDTO
    {
        public List<string> RMU { get; set; }
        public string Section { get; set; }
        public string RFCRMU { get; set; }
        public int RFCYear { get; set; }
    }
}
