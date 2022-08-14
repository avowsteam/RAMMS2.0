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
    public class FormB11Controller : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFormN1Service _formN1Service;
        private readonly IFormJServices _formJService;
        private readonly IRoadMasterService _roadMasterService;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB11Service _formB11Service;
        private readonly IAssetsService _AssetService;

        public FormB11Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          IFormN1Service formN1Service,
          IFormJServices formJServices,
          ISecurity security,
          IRoadMasterService roadMasterService,
          ILogger logger,
          IFormB11Service formB11Service,
          IAssetsService assestService
          )
        {
            _userService = userService;
            _dDLookupBO = _ddLookupBO;
            Environment = _environment;
            _webHostEnvironment = webhostenvironment;
            _ddLookupService = ddLookupService;
            _security = security;
            _formN1Service = formN1Service ?? throw new ArgumentNullException(nameof(formN1Service));
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _roadMasterService = roadMasterService ?? throw new ArgumentNullException(nameof(roadMasterService));
            _logger = logger;
            _formB11Service = formB11Service ?? throw new ArgumentNullException(nameof(formB11Service));
            _AssetService = assestService;
        }

        public async Task<IActionResult> Index()
        {
            LoadLookupService("Year");
            var grid = new Models.CDataTable() { Name = "tblFB11HGrid", APIURL = "/FormB11/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget) && _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB11.HeaderGrid.ActionRender" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionDate", title = "Revision Date", render = "frmB11.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "CrByName", title = "User Name" });
            return View(grid);
        }

        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formB11Service.GetHeaderGrid(searchData), JsonOption());
        }

        public async Task<IActionResult> View(int id)
        {
            ViewBag.IsEdit = false;
            return id > 0 ? await ViewRequest(id, 0) : RedirectToAction("404", "Error");
        }

        public async Task<IActionResult> Edit(int id, int view)
        {
            ViewBag.IsEdit = true;
            return id > 0 ? await ViewRequest(id, 1) : RedirectToAction("404", "Error");
        }

        private async Task<IActionResult> ViewRequest(int id, int IsEdit)
        {
            LoadLookupService("Year", "RMU");
            FormB11Model _model = new FormB11Model();
            if (id > 0)
            {
                _model.FormB11Header = await _formB11Service.GetHeaderById(id,IsEdit);
            }
            else
            {
                _model.FormB11Header = new FormB11DTO();
            }

            return PartialView("~/Views/FormB11/_AddFormB11.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year)
        {
            return Json(_formB11Service.GetMaxRev(Year));
        }

        public async Task<IActionResult> GetLabourHistoryData(int Year)
        {
            return Json(_formB11Service.GetLabourHistoryData(Year));
        }

        public async Task<IActionResult> GetMaterialHistoryData(int Year)
        {
            return Json(_formB11Service.GetMaterialHistoryData(Year));
        }

        public async Task<IActionResult> GetEquipmentHistoryData(int Year)
        {
            return Json(_formB11Service.GetEquipmentHistoryData(Year));
        }

        public async Task<IActionResult> GetLabourViewHistoryData(int id)
        {
            return Json(_formB11Service.GetLabourViewHistoryData(id));
        }

        public async Task<IActionResult> GetMaterialViewHistoryData(int id)
        {
            return Json(_formB11Service.GetMaterialViewHistoryData(id));
        }

        public async Task<IActionResult> GetEquipmentViewHistoryData(int id)
        {
            return Json(_formB11Service.GetEquipmentViewHistoryData(id));
        }

        public async Task<IActionResult> SaveFormB11(string formb11data)
        {
            FormB11DTO formb11 = new FormB11DTO();
            formb11 = JsonConvert.DeserializeObject<FormB11DTO>(formb11data);
            await _formB11Service.SaveFormB11(formb11);
            return Json(1);
        }

    }
}
