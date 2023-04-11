using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class FormB10Controller : BaseController
    {
        private IFormB10Service _FormB10Service;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormB10Controller(
            IFormB10Service service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormB10Service = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormB10SearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormB10SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormB10SearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormB10SearchGridDTO();
            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                searchData.filterData.SmartSearch = Request.Form["columns[0][search][value]"].ToString();
            }

            if (Request.Form.ContainsKey("columns[1][search][value]"))
            {
                searchData.filterData.Year = Request.Form["columns[1][search][value]"].ToString() == "null" ? "" : Request.Form["columns[1][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[2][search][value]"))
            {
                searchData.filterData.FromDate = Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                searchData.filterData.ToDate = Request.Form["columns[4][search][value]"].ToString();
            }


            filteredPagingDefinition.Filters = searchData.filterData;
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            var result = await _FormB10Service.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }

        public async Task<IActionResult> Add(int id, int isview)
        {

            FormB10Model _model = new FormB10Model();
            _model.FormB10 = await _FormB10Service.GetHeaderById(id);
            _model.view = isview;
            return PartialView("~/Views/FormB10/_AddFormB10.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year)
        {
            return Json(_FormB10Service.GetMaxRev(Year));
        }

        public async Task<IActionResult> SaveFormB10(FormB10ResponseDTO FormB10)
        {
            await _FormB10Service.SaveFormB10(FormB10, FormB10.FormB10History);
            return Json(1);
        }

        public async Task<IActionResult> FormB10Download(int id, [FromServices] IWebHostEnvironment _environment)
        {
            var content1 = await _FormB10Service.FormDownload("FORMB10", id, _environment.WebRootPath + "/Templates/FORMB10.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FORMB10" + ".xlsx");
        }

    }
}

