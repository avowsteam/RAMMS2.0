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
    public class FormB13Model
    {

        public FormB13ResponseDTO FormB13 { get; set; }

        public FormB13HistoryResponseDTO FormB13History { get; set; }
     
        public int view { get; set; }


    }

 
}
