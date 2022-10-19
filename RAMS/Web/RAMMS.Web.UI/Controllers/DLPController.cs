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
        private readonly ILogger _logger;
        private readonly ISecurity _security;
        private readonly IDlpSpiService _dlpSpiService;
       

        public DLPController(
           ISecurity security,
           ILogger logger,
           IDlpSpiService dlpSpiService
           )
        {
           
            _security = security;
            _dlpSpiService = dlpSpiService ?? throw new ArgumentNullException(nameof(dlpSpiService));

            _logger = logger;
            
        }

        public async Task<IActionResult> FormSPI()
        {
            LoadLookupService("Year");
            DLPModel model = new DLPModel();
            model.DivisionMiri = await _dlpSpiService.GetDivisionMiri(DateTime.Today.Year);
            model.RmuMiri = await _dlpSpiService.GetDivisionRMUMiri (DateTime.Today.Year);
            model.RmuBTN = await _dlpSpiService.GetDivisionRMUBTN(DateTime.Today.Year);
            return View(model);
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
