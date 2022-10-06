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
    public class FormB12Controller : Models.BaseController
    {

        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB12Service _formB12Service;

        public FormB12Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormB12Service formB12Service,
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
            _formB12Service = formB12Service ?? throw new ArgumentNullException(nameof(formB12Service));
        }

        public async Task<IActionResult> Index()
        {
            LoadLookupService("Year");
            var grid = new Models.CDataTable() { Name = "tblFB12HGrid", APIURL = "/FormB12/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget) && _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB12.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "ReferenceNo", title = "Reference No" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionDate", title = "Revision Date", render = "frmB12.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "CrByName", title = "User Name" });
            return View(grid);
        }

        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formB12Service.GetHeaderGrid(searchData), JsonOption());
        }

        public async Task<IActionResult> FindDetails(string formb12data)
        {
            FormB12DTO formb12 = new FormB12DTO();
            formb12 = JsonConvert.DeserializeObject<FormB12DTO>(formb12data);
            return Json(await _formB12Service.FindDetails(formb12, _security.UserID), JsonOption());
        }

        public async  Task<IActionResult> Add()
        {
            ViewBag.IsAdd = true;
            ViewBag.IsEdit = true;
            return await ViewRequest(0);
        }

        public async Task<IActionResult> View(int id)
        {
            ViewBag.IsEdit = false;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        public async Task<IActionResult> EditForm(int id, int view)
        {
            ViewBag.IsEdit = true;
            return id > 0 ? await ViewRequest(id) : RedirectToAction("404", "Error");
        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            LoadLookupService("Year");
           
            FormB12Model _model = new FormB12Model();
            if (id > 0)
            {
                _model.FormB12Header = await _formB12Service.GetHeaderById(id, !ViewBag.IsEdit);
            }
            else
            {
                _model.FormB12Header = new FormB12DTO();
            }



            return PartialView("~/Views/FormB12/_AddFormB12.cshtml", _model);
        }


        public async Task<IActionResult> GetHistoryData(int HistoryID)
        {
            return Json(_formB12Service.GetHistoryData(HistoryID));
        }

        public async Task<IActionResult> GetMaxRev(int Year)
        {
            return Json(_formB12Service.GetMaxRev(Year));
        }

        public async Task<IActionResult> GetPlannedBudgetDataMiri(int Year)
        {
            return Json(_formB12Service.GetPlannedBudgetDataMiri(Year));
        }

        public async Task<IActionResult> GetPlannedBudgetDataBTN(int Year)
        {
            return Json(_formB12Service.GetPlannedBudgetDataBTN(Year));
        }

        public async Task<IActionResult> GetUnitData(int Year)
        {
            return Json(_formB12Service.GetUnitData(Year));
        }


        public async Task<IActionResult> SaveFormB12(FormB12DTO formb14)
        {

            formb14.CrBy = _security.UserID;
            formb14.CrByName = _security.UserName;
            formb14.CrDt = DateTime.Today;
            formb14.SubmitSts = false;
            return await SaveAll(formb14, false);
        }

        public async Task<IActionResult> Submit(FormB12DTO formb14, int reload)
        {
         
            //List<FormB14HistoryDTO> formb14 = new List<FormB14HistoryDTO>();

            //formb12hdr = JsonConvert.DeserializeObject<FormB14HeaderDTO>(formb14hdrdata);
            //formb14 = JsonConvert.DeserializeObject<List<FormB14HistoryDTO>>(formb14data);

            formb14.SubmitSts = true;
            formb14.CrBy  = _security.UserID;
            formb14.CrByName = _security.UserName;
            formb14.CrDt  = DateTime.Today;
            return await SaveAll(formb14, true);
        }

        private async Task<JsonResult> SaveAll(DTO.ResponseBO.FormB12DTO formb14hdr, bool updateSubmit)
        {
            formb14hdr.CrBy = _security.UserID;       
            formb14hdr.CrDt = DateTime.UtcNow;
            formb14hdr.ActiveYn = true;
            var result = await _formB12Service.SaveB12(formb14hdr, updateSubmit);
            return Json(new { Id = result.PkRefNo }, JsonOption());
        }


    }
}
