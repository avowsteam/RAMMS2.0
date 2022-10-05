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
    public class FormT3Controller : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormT3Service _formT3Service;

        public FormT3Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormT3Service formT3Service,
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
            _formT3Service = formT3Service ?? throw new ArgumentNullException(nameof(formT3Service));
        }


        public IActionResult Index()
        {
            LoadLookupService("Year", "RMU");
            var grid = new Models.CDataTable() { Name = "tblFT3HGrid", APIURL = "/FormT3/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget); //&& _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmT3.HeaderGrid.ActionRender", className = "" });
            grid.Columns.Add(new CDataColumns() { data = "RefID", title = "Reference No" });
            grid.Columns.Add(new CDataColumns() { data = "RMU", title = "RMU" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "RevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "IssueDate", title = "Issue Date", render = "frmT3.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "Status", title = "Status" });
            return View(grid);
        }

        public async Task<IActionResult> FindDetails(string formT3data)
        {
            FormT3HeaderDTO formT3 = new FormT3HeaderDTO();
            formT3 = JsonConvert.DeserializeObject<FormT3HeaderDTO>(formT3data);
            return Json(await _formT3Service.FindDetails(formT3, _security.UserID), JsonOption());
        }
        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {
            if (searchData.order != null && searchData.order.Count > 0)
            {
                searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            return Json(await _formT3Service.GetHeaderGrid(searchData), JsonOption());
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
            if (id > 0) { return Ok(new { id = _formT3Service.Delete(id) }); }
            else { return BadRequest("Invalid Request!"); }

        }

        private async Task<IActionResult> ViewRequest(int id)
        {
            LoadLookupService("Year", "User", "RMU");
            FormT3Model _model = new FormT3Model();
            if (id > 0)
            {
                _model.FormT3Header = await _formT3Service.GetHeaderById(id, !ViewBag.IsEdit);
                _model.RmT3History = _model.FormT3Header.RmT3History;
            }
            else
            {
                _model.FormT3Header = new FormT3HeaderDTO();
            }
            return PartialView("~/Views/FormT3/_AddFormT3.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year, string RmuCode)
        {
            return Json(_formT3Service.GetMaxRev(Year, RmuCode));
        }

        //public async Task<IActionResult> SaveFormT3(string formT3hdrdata, string formT3data)
        //{
        //    List<FormT3HistoryDTO> formT3 = new List<FormT3HistoryDTO>();
        //    formT3 = JsonConvert.DeserializeObject<List<FormT3HistoryDTO>>(formT3data);
        //    await _formT3Service.SaveFormT3(formT3);
        //    return Json(1);
        //}

        public async Task<IActionResult> SaveFormT3(string formT3hdrdata, string formT3data, int reload)
        {
            FormT3HeaderDTO formT3hdr = new FormT3HeaderDTO();
            List<FormT3HistoryDTO> formT3 = new List<FormT3HistoryDTO>();

            formT3hdr = JsonConvert.DeserializeObject<FormT3HeaderDTO>(formT3hdrdata);
            formT3 = JsonConvert.DeserializeObject<List<FormT3HistoryDTO>>(formT3data);
            //await _formT3Service.SaveFormT3(formT3);
            if (reload == 1)
            {
                formT3hdr.SubmitSts = true;
                //formT3hdr.UseridProsd = _security.UserID;
                //formT3hdr.DtProsd = DateTime.Today;
            }
            return await SaveAll(formT3hdr, formT3, false);
        }

        public async Task<IActionResult> Submit(string formT3hdrdata, string formT3data, int reload)
        {
            FormT3HeaderDTO formT3hdr = new FormT3HeaderDTO();
            List<FormT3HistoryDTO> formT3 = new List<FormT3HistoryDTO>();

            formT3hdr = JsonConvert.DeserializeObject<FormT3HeaderDTO>(formT3hdrdata);
            formT3 = JsonConvert.DeserializeObject<List<FormT3HistoryDTO>>(formT3data);

            formT3hdr.SubmitSts = true;
            //formT3hdr.UseridProsd = _security.UserID;
            //formT3hdr.DtProsd = DateTime.Today;
            return await SaveAll(formT3hdr, formT3, true);
        }

        private async Task<JsonResult> SaveAll(DTO.ResponseBO.FormT3HeaderDTO formT3hdr, List<DTO.ResponseBO.FormT3HistoryDTO> formT3, bool updateSubmit)
        {
            formT3hdr.CrBy = _security.UserID;
            formT3hdr.ModBy = _security.UserID;
            formT3hdr.ModDt = DateTime.UtcNow;
            formT3hdr.CrDt = DateTime.UtcNow;
            formT3hdr.ActiveYn = true;
            var result = await _formT3Service.SaveT3(formT3hdr, formT3, updateSubmit);
            return Json(new { Id = result.PkRefNo }, JsonOption());
        }

        public async Task<IActionResult> GetHistoryData(int HistoryID)
        {
            return Json(_formT3Service.GetHistoryData(HistoryID));
        }

        public async Task<IActionResult> GetPlannedBudgetData(string RmuCode, int Year)
        {
            return Json(_formT3Service.GetPlannedBudgetData(RmuCode, Year));
        }

        public async Task<IActionResult> GetUnitData(int Year)
        {
            return Json(_formT3Service.GetUnitData(Year));
        }

        public IActionResult Download(int id)
        {
            var content1 = _formT3Service.FormDownload("FormT3", id, _webHostEnvironment.WebRootPath, _webHostEnvironment.WebRootPath + "/Templates/FormT3.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FormT3" + ".xlsx");
        }
    }
}
