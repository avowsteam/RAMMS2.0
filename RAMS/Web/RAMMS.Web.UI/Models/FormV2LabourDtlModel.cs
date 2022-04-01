﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RAMMS.DTO;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RAMMS.Web.UI.Models
{
    public class FormV2LabourDtlModel
    {
        public FormV2SearchGridDTO SearchObj { get; set; }
        public FormV2LabourDetailsResponseDTO SaveFormDLabourModel { get; set; }

        public IEnumerable<FormV2LabourDetailsResponseDTO> FormV2LabourDtlList { get; set; }

        public string HeaderNo { get; set; }
        public string LabourDesc { get; set; }

        public IEnumerable<SelectListItem> selectList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }


    }
}
