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
    public class FormB9Controller : BaseController
    {
        private IFormB9Service _FormB9Service;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormB9Controller(
            IFormB9Service service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormB9Service = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormB9SearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormB9SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormB9SearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormB9SearchGridDTO();
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
            var result = await _FormB9Service.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }

 
        public async Task<IActionResult> Add(int id, int view)
        {
            FormB9Model _model = new FormB9Model();
            _model.FormB9 = await _FormB9Service.GetHeaderById(id);
            return PartialView("~/Views/FormB9/_AddFormB9.cshtml", _model);
        }


        public async Task<IActionResult> SaveFormB9(FormB9ResponseDTO FormB9)
        {
            await _FormB9Service.SaveFormB9(FormB9, FormB9.FormB9History);
            return Json(1);
        }

    }
}

