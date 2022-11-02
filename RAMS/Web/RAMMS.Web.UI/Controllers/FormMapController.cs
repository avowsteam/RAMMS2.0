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
    public class FormMapController : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormMapService _formMapService;

        public FormMapController(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormMapService formMapService,
          IAssetsService assestService)
        {
            _userService = userService;
            _dDLookupBO = _ddLookupBO;
            Environment = _environment;
            _webHostEnvironment = webhostenvironment;
            _ddLookupService = ddLookupService;
            _security = security;
            _logger = logger;
            _formMapService = formMapService ?? throw new ArgumentNullException(nameof(formMapService));
        }
        public IActionResult Index()
        {
            DDLookUpDTO ddLookup = new DDLookUpDTO();
            ddLookup.Type = "Month";
            ViewData["Months"] = _ddLookupService.GetDdDescValue(ddLookup);
            LoadLookupService("Year", "RMU");
            var grid = new Models.CDataTable() { Name = "tblFMapHGrid", APIURL = "/FormMap/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget); //&& _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmMap.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "RefID", title = "Reference No" });
            grid.Columns.Add(new CDataColumns() { data = "RMU", title = "RMU" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "Month", title = "Month" });
            grid.Columns.Add(new CDataColumns() { data = "Status", title = "Status" });
            return View(grid);
        }

        public async Task<IActionResult> FindDetails(string formMapdata)
        {
            FormMapHeaderDTO formMap = new FormMapHeaderDTO();
            formMap = JsonConvert.DeserializeObject<FormMapHeaderDTO>(formMapdata);
            return Json(await _formMapService.FindDetails(formMap, _security.UserID), JsonOption());
        }
        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formMapService.GetHeaderGrid(searchData), JsonOption());
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.IsAdd = true;
            ViewBag.IsEdit = true;
            return await ViewRequest(0);
        }

        public async Task<IActionResult> View(int id)
        {
            ViewBag.IsEdit = false;
            ViewBag.IsAdd = false;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        public async Task<IActionResult> Edit(int id, int view)
        {
            ViewBag.IsEdit = true;
            ViewBag.IsAdd = false;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            DDLookUpDTO ddLookup = new DDLookUpDTO();
            ddLookup.Type = "Month";
            ViewData["Months"] = await _ddLookupService.GetDdDescValue(ddLookup);
            LoadLookupService("Year", "User", "RMU");
            FormMapModel _model = new FormMapModel();
            if (id > 0)
            {
                _model.FormMapHeader = await _formMapService.GetHeaderById(id, !ViewBag.IsEdit);
                _model.RmMapDetails = _model.FormMapHeader.RmMapDetails;
            }
            else
            {
                _model.FormMapHeader = new FormMapHeaderDTO();
            }
            return PartialView("~/Views/FormMap/_AddFormMap.cshtml", _model);
        }

        [HttpPost] //Tab
        public IActionResult Delete(int id)
        {
            if (id > 0) { return Ok(new { id = _formMapService.Delete(id) }); }
            else { return BadRequest("Invalid Request!"); }

        }
        public async Task<IActionResult> GetForDDetails(string RmuCode, int Year, int Month)
        {
            return Json(_formMapService.GetForDDetails(RmuCode, Year, Month));
        }

        public async Task<IActionResult> SaveFormMap(string formMaphdrdata, string formMapdata, int reload)
        {
            FormMapHeaderDTO formmaphdr = new FormMapHeaderDTO();
            List<FormMapDetailsDTO> formmap = new List<FormMapDetailsDTO>();

            formmaphdr = JsonConvert.DeserializeObject<FormMapHeaderDTO>(formMaphdrdata);
            formmap = JsonConvert.DeserializeObject<List<FormMapDetailsDTO>>(formMapdata);
            //await _formB14Service.SaveFormB14(formb14);
            if (reload == 1)
            {
                formmaphdr.SubmitSts = true;
                formmaphdr.PreparedBy = _security.UserID;
                formmaphdr.PreparedDate = DateTime.Today;
            }
            return await SaveAll(formmaphdr, formmap, false);
        }

        private async Task<JsonResult> SaveAll(DTO.ResponseBO.FormMapHeaderDTO formmaphdr, List<DTO.ResponseBO.FormMapDetailsDTO> formmap, bool updateSubmit)
        {
            formmaphdr.CrBy = _security.UserID;
            formmaphdr.ModBy = _security.UserID;
            formmaphdr.ModDt = DateTime.UtcNow;
            formmaphdr.CrDt = DateTime.UtcNow;
            formmaphdr.ActiveYn = true;
            var result = await _formMapService.SaveMap(formmaphdr, formmap, updateSubmit);
            return Json(new { Id = result.PkRefNo }, JsonOption());
        }

        public async Task<IActionResult> GetForMapDetails(int ID)
        {
            return Json(_formMapService.GetForMapDetails(ID));
        }
    }
}
