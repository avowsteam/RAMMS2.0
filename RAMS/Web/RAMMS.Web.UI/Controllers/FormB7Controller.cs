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

namespace RAMMS.Web.UI.Controllers
{
    public class FormB7Controller : Models.BaseController
    {

        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB7Service _formB7Service;

        public FormB7Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormB7Service formB7Service,
          IAssetsService assestService
          )
        {
            _userService = userService;
            _dDLookupBO = _ddLookupBO;
            Environment = _environment;
            _webHostEnvironment = webhostenvironment;
            _ddLookupService = ddLookupService;
            _security = security;
            _logger = logger;
            _formB7Service = formB7Service ?? throw new ArgumentNullException(nameof(formB7Service));
        }

        public async Task<IActionResult> Index()
        {
            LoadLookupService("Year");
            var grid = new Models.CDataTable() { Name = "tblFB7HGrid", APIURL = "/FormB7/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget) && _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB7.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionDate", title = "Revision Date", render = "frmB7.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "CrByName", title = "User Name" });
            return View(grid);
        }

        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formB7Service.GetHeaderGrid(searchData), JsonOption());
        }

        public async Task<IActionResult> View(int id)
        {
            ViewBag.IsEdit = false;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        public async Task<IActionResult> Edit(int id, int view)
        {
            ViewBag.IsEdit = true;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            LoadLookupService("Year");
            FormB7Model _model = new FormB7Model();
            if (id > 0)
            {
                _model.FormB7Header = await _formB7Service.GetHeaderById(id);

            }
            else
            {
                _model.FormB7Header = new FormB7HeaderDTO();
            }



            return PartialView("~/Views/FormB7/_AddFormB7.cshtml", _model);
        }
        public async Task<IActionResult> SaveFormB7(FormB7HeaderDTO FormB7)
        {
            await _formB7Service.SaveFormB7(FormB7);
            return Json(1);
        }

        public async Task<IActionResult> GetMaxRev(int Year)
        {
            return Json( _formB7Service.GetMaxRev(Year));
        }

        public IActionResult Download(int id)
        {
            var content1 = _formB7Service.FormDownload("FormB7", id, _webHostEnvironment.WebRootPath, _webHostEnvironment.WebRootPath + "/Templates/FormB7.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FormB7" + ".xlsx");
        }

    }
}
