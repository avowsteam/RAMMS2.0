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
    public class FormB15Model
    {
        public FormB15HeaderDTO FormB15Header { get; set; }

        public List<FormB15HistoryDTO> RmB15History { get; set; }

        public int view { get; set; }
    }
}
