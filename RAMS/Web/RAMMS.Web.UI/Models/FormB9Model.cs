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
    public class FormB9Model
    {

        public FormB9ResponseDTO FormB9 { get; set; }

        public FormB9HistoryResponseDTO FormB9History { get; set; }
     
        public int view { get; set; }


    }

 
}
