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
    public class FormB15Controller : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB15Service _formB15Service;

        public FormB15Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormB15Service formB15Service,
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
            _formB15Service = formB15Service ?? throw new ArgumentNullException(nameof(formB15Service));
        }

        public IActionResult Index()
        {
            LoadLookupService("Year", "RMU");
            var grid = new Models.CDataTable() { Name = "tblFB15HGrid", APIURL = "/FormB15/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget); //&& _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB15.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "RefID", title = "Reference No" });
            grid.Columns.Add(new CDataColumns() { data = "RMU", title = "RMU" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "IssueDate", title = "Issue Date", render = "frmB15.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "Status", title = "Status" });
            return View(grid);
        }

        public async Task<IActionResult> FindDetails(string formb15data)
        {
            FormB15HeaderDTO formb15 = new FormB15HeaderDTO();
            formb15 = JsonConvert.DeserializeObject<FormB15HeaderDTO>(formb15data);
            return Json(await _formB15Service.FindDetails(formb15, _security.UserID), JsonOption());
        }
        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formB15Service.GetHeaderGrid(searchData), JsonOption());
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
            if (id > 0) { return Ok(new { id = _formB15Service.Delete(id) }); }
            else { return BadRequest("Invalid Request!"); }

        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            LoadLookupService("Year", "User", "RMU");
            FormB15Model _model = new FormB15Model();
            if (id > 0)
            {
                _model.FormB15Header = await _formB15Service.GetHeaderById(id, !ViewBag.IsEdit);
                _model.RmB15History = _model.FormB15Header.RmB15History;
            }
            else
            {
                _model.FormB15Header = new FormB15HeaderDTO();
            }
            return PartialView("~/Views/FormB15/_AddFormB15.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year, string RmuCode)
        {
            return Json(_formB15Service.GetMaxRev(Year, RmuCode));
        }

        //public async Task<IActionResult> SaveFormB15(string formb15hdrdata, string formb15data)
        //{
        //    List<FormB15HistoryDTO> formb15 = new List<FormB15HistoryDTO>();
        //    formb15 = JsonConvert.DeserializeObject<List<FormB15HistoryDTO>>(formb15data);
        //    await _formB15Service.SaveFormB15(formb15);
        //    return Json(1);
        //}

        public async Task<IActionResult> SaveFormB15(string formb15hdrdata, string formb15data, int reload)
        {
            FormB15HeaderDTO formb15hdr = new FormB15HeaderDTO();
            List<FormB15HistoryDTO> formb15 = new List<FormB15HistoryDTO>();

            formb15hdr = JsonConvert.DeserializeObject<FormB15HeaderDTO>(formb15hdrdata);
            formb15 = JsonConvert.DeserializeObject<List<FormB15HistoryDTO>>(formb15data);
            //await _formB15Service.SaveFormB15(formb15);
            if (reload == 1)
            {
                formb15hdr.SubmitSts = true;
                formb15hdr.UseridProsd = _security.UserID;
                formb15hdr.DtProsd = DateTime.Today;
            }
            return await SaveAll(formb15hdr, formb15, false);
        }

        public async Task<IActionResult> Submit(string formb15hdrdata, string formb15data, int reload)
        {
            FormB15HeaderDTO formb15hdr = new FormB15HeaderDTO();
            List<FormB15HistoryDTO> formb15 = new List<FormB15HistoryDTO>();

            formb15hdr = JsonConvert.DeserializeObject<FormB15HeaderDTO>(formb15hdrdata);
            formb15 = JsonConvert.DeserializeObject<List<FormB15HistoryDTO>>(formb15data);

            formb15hdr.SubmitSts = true;
            formb15hdr.UseridProsd = _security.UserID;
            formb15hdr.DtProsd = DateTime.Today;
            return await SaveAll(formb15hdr, formb15, true);
        }

        private async Task<JsonResult> SaveAll(DTO.ResponseBO.FormB15HeaderDTO formb15hdr, List<DTO.ResponseBO.FormB15HistoryDTO> formb15, bool updateSubmit)
        {
            formb15hdr.CrBy = _security.UserID;
            formb15hdr.ModBy = _security.UserID;
            formb15hdr.ModDt = DateTime.UtcNow;
            formb15hdr.CrDt = DateTime.UtcNow;
            formb15hdr.ActiveYn = true;
            var result = await _formB15Service.SaveB15(formb15hdr, formb15, updateSubmit);
            return Json(new { Id = result.PkRefNo }, JsonOption());
        }

        public async Task<IActionResult> GetHistoryData(int HistoryID)
        {
            return Json(_formB15Service.GetHistoryData(HistoryID));
        }

        public async Task<IActionResult> GetPlannedBudgetData(string RmuCode, int Year)
        {
            return Json(_formB15Service.GetPlannedBudgetData(RmuCode,Year));
        }
        public IActionResult Download(int id)
        {
            var content1 = _formB15Service.FormDownload("FormB15", id, _webHostEnvironment.WebRootPath, _webHostEnvironment.WebRootPath + "/Templates/FormB15.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FormB15" + ".xlsx");
        }
    }
}
