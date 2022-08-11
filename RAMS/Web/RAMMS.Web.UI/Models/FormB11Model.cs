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
    public class FormB11Model
    {
        public FormB11DTO FormB11Header { get; set; }

        public FormB11LabourCostDTO FormB11LabourHistory { get; set; }

        public FormB11EquipmentCostDTO FormB11EquipmentsHistory { get; set; }

        public FormB11MaterialCostDTO FormB11MaterialHistory { get; set; }

        public int view { get; set; }

    }
}
