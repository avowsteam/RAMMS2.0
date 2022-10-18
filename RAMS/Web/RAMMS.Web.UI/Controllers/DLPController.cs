using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using RAMMS.Business.ServiceProvider;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RAMMS.Web.UI.Controllers
{
    public class DLPController : Models.BaseController
    {
        public IActionResult FormSPI()
        {
            LoadLookupService("Year");
            return View();
        }

        public IActionResult FormIRI_RMI()
        {
            return View();
        }

        public IActionResult AddIRI()
        {
            return View();
        }
    }
}
