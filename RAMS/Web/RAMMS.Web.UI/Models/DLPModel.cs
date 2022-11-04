using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider;
using RAMMS.DTO;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;

namespace RAMMS.Web.UI.Models
{
    public class DLPModel
    {
        public int Year { get; set; } 
        public List<DlpSPIDTO> DivisionMiri { get; set; }

        public List<DlpSPIDTO> RmuMiri { get; set; }

        public List<DlpSPIDTO> RmuBTN { get; set; }
        public int view { get; set; }
    }
}
