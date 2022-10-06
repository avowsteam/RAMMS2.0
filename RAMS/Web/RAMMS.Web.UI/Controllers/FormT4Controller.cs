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
    public class FormT4Controller : BaseController
    {
        private IFormT4Service _FormT4Service;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormT4Controller(
            IFormT4Service service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormT4Service = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormT4SearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormT4SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormT4SearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormT4SearchGridDTO();
            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                searchData.filterData.SmartSearch = Request.Form["columns[0][search][value]"].ToString();
            }

            if (Request.Form.ContainsKey("columns[1][search][value]"))
            {
                searchData.filterData.RMU = Request.Form["columns[1][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[2][search][value]"))
            {
                searchData.filterData.Year = Request.Form["columns[2][search][value]"].ToString() == "null" ? "" : Request.Form["columns[2][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                searchData.filterData.FromDate = Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[4][search][value]"))
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
            var result = await _FormT4Service.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }


        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("User");
            FormT4Model _model = new FormT4Model();
            if (id > 0)
                _model.FormT4Header = await _FormT4Service.GetHeaderById(id);
            else
                _model.FormT4Header = new FormT4HeaderResponseDTO();

            _model.view = isview;

       

            return PartialView("~/Views/FormT4/_AddFormT4.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year, string RMU)
        {
            return Json(_FormT4Service.GetMaxRev(Year, RMU));
        }

        public async Task<IActionResult> SaveFormT4(FormT4HeaderResponseDTO FormT4Header)
        {
            FormT4Header = await _FormT4Service.SaveFormT4(FormT4Header);
            return Json(FormT4Header.PkRefNo);
        }

        public async Task<IActionResult> UpdateFormT4(FormT4HeaderResponseDTO FormT4Header)
        {
            await _FormT4Service.UpdateFormT4(FormT4Header, FormT4Header.FormT4);
            return Json(1);
        }

        public async Task<IActionResult> DeleteFormT4(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _FormT4Service.DeleteFormT4(id);
            return Json(rowsAffected);
        }

        public async Task<IActionResult> FormT4Download(int id, [FromServices] IWebHostEnvironment _environment)
        {
            var content1 = await _FormT4Service.FormDownload("FORMT4", id, _environment.WebRootPath + "/Templates/FORMT4.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FORMT4" + ".xlsx");
        }

    }
}

