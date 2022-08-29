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
    public class FormB14Model
    {

        public FormB14HeaderDTO FormB14Header { get; set; }

        public FormB14HistoryDTO FormB14History { get; set; }

        public int view { get; set; }


    }


}
