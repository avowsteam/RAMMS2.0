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
    public class FormB14Controller : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB14Service _formB14Service;


        public FormB14Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormB14Service formB14Service,
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
            _formB14Service = formB14Service ?? throw new ArgumentNullException(nameof(formB14Service));
        }


        public IActionResult Index()
        {
            LoadLookupService("Year", "RMU");
            var grid = new Models.CDataTable() { Name = "tblFB14HGrid", APIURL = "/FormB14/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget); //&& _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB14.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "RMU", title = "RMU" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "IssueDate", title = "Issue Date", render = "frmB14.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "Status", title = "Status" });
            return View(grid);
        }

        public async Task<IActionResult> FindDetails(string formb14data)
        {
            FormB14HeaderDTO formb14 = new FormB14HeaderDTO();
            formb14 = JsonConvert.DeserializeObject<FormB14HeaderDTO>(formb14data);
            return Json(await _formB14Service.FindDetails(formb14, _security.UserID), JsonOption());
        }
        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formB14Service.GetHeaderGrid(searchData), JsonOption());
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

        [HttpPost] //Tab
        public IActionResult Delete(int id)
        {
            if (id > 0) { return Ok(new { id = _formB14Service.Delete(id) }); }
            else { return BadRequest("Invalid Request!"); }

        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            LoadLookupService("Year", "User", "RMU");
            FormB14Model _model = new FormB14Model();
            if (id > 0)
            {
                _model.FormB14Header = await _formB14Service.GetHeaderById(id, !ViewBag.IsEdit);
                _model.RmB14History = _model.FormB14Header.RmB14History;
            }
            else
            {
                _model.FormB14Header = new FormB14HeaderDTO();
            }
            return PartialView("~/Views/FormB14/_AddFormB14.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year, string RmuCode)
        {
            return Json(_formB14Service.GetMaxRev(Year, RmuCode));
        }

        //public async Task<IActionResult> SaveFormB14(string formb14hdrdata, string formb14data)
        //{
        //    List<FormB14HistoryDTO> formb14 = new List<FormB14HistoryDTO>();
        //    formb14 = JsonConvert.DeserializeObject<List<FormB14HistoryDTO>>(formb14data);
        //    await _formB14Service.SaveFormB14(formb14);
        //    return Json(1);
        //}

        public async Task<IActionResult> SaveFormB14(string formb14hdrdata, string formb14data, int reload)
        {
            FormB14HeaderDTO formb14hdr = new FormB14HeaderDTO();
            List<FormB14HistoryDTO> formb14 = new List<FormB14HistoryDTO>();

            formb14hdr = JsonConvert.DeserializeObject<FormB14HeaderDTO>(formb14hdrdata);
            formb14 = JsonConvert.DeserializeObject<List<FormB14HistoryDTO>>(formb14data);
            //await _formB14Service.SaveFormB14(formb14);
            if (reload == 1)
            {
                formb14hdr.SubmitSts = true;
                formb14hdr.UseridProsd = _security.UserID;
                formb14hdr.DtProsd = DateTime.Today;
            }
            return await SaveAll(formb14hdr, formb14, false);
        }

        public async Task<IActionResult> Submit(string formb14hdrdata, string formb14data, int reload)
        {
            FormB14HeaderDTO formb14hdr = new FormB14HeaderDTO();
            List<FormB14HistoryDTO> formb14 = new List<FormB14HistoryDTO>();

            formb14hdr = JsonConvert.DeserializeObject<FormB14HeaderDTO>(formb14hdrdata);
            formb14 = JsonConvert.DeserializeObject<List<FormB14HistoryDTO>>(formb14data);

            formb14hdr.SubmitSts = true;
            formb14hdr.UseridProsd = _security.UserID;
            formb14hdr.DtProsd = DateTime.Today;
            return await SaveAll(formb14hdr, formb14, true);
        }

        private async Task<JsonResult> SaveAll(DTO.ResponseBO.FormB14HeaderDTO formb14hdr, List<DTO.ResponseBO.FormB14HistoryDTO> formb14, bool updateSubmit)
        {
            formb14hdr.CrBy = _security.UserID;
            formb14hdr.ModBy = _security.UserID;
            formb14hdr.ModDt = DateTime.UtcNow;
            formb14hdr.CrDt = DateTime.UtcNow;
            formb14hdr.ActiveYn = true;
            var result = await _formB14Service.SaveB14(formb14hdr, formb14, updateSubmit);
            return Json(new { Id = result.PkRefNo }, JsonOption());
        }

        public async Task<IActionResult> GetHistoryData(int HistoryID)
        {
            return Json(_formB14Service.GetHistoryData(HistoryID));
        }

        public async Task<IActionResult> GetPlannedBudgetData(string RmuCode, int Year)
        {
            return Json(_formB14Service.GetPlannedBudgetData(RmuCode, Year));
        }
    }
}
