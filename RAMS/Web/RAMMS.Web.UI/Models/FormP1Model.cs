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
    public class FormP1Model
    {

        public FormP1HeaderResponseDTO FormP1Header { get; set; }

        public FormP1ResponseDTO FormP1Detail { get; set; }
     
        public int view { get; set; }


    }

 
}
