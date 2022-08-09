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
    public class FormB7Model
    {

        public FormB7HeaderDTO FormB7Header { get; set; }

        public FormB7LabourHistoryDTO FormB7LabourHistory { get; set; }

        public FormB7MaterialHistoryDTO FormB7MaterialHistory { get; set; }

        public FormB7EquipmentsHistoryDTO FormB7EquipmentsHistory { get; set; }

        public int view { get; set; }


    }

 
}
