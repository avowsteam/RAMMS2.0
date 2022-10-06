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
    public class FormT4Model
    {

        public FormT4HeaderResponseDTO FormT4Header { get; set; }

        public FormT4ResponseDTO FormT4 { get; set; }
     
        public int view { get; set; }


    }

 
}
