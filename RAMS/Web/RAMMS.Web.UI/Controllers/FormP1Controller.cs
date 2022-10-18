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
    public class FormP1Controller : BaseController
    {
        private IFormP1Service _FormP1Service;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormP1Controller(
            IFormP1Service service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormP1Service = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormP1SearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormP1SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormP1SearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormP1SearchGridDTO();
            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                searchData.filterData.SmartSearch = Request.Form["columns[0][search][value]"].ToString();
            }

            if (Request.Form.ContainsKey("columns[1][search][value]"))
            {
                searchData.filterData.YearFrom = Request.Form["columns[1][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[2][search][value]"))
            {
                searchData.filterData.YearTo = Request.Form["columns[2][search][value]"].ToString() == "null" ? "" : Request.Form["columns[2][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                searchData.filterData.MonthFrom = Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[4][search][value]"))
            {
                searchData.filterData.MonthTo = Request.Form["columns[4][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[5][search][value]"))
            {
                searchData.filterData.CertificateNo = Request.Form["columns[5][search][value]"].ToString();
            }


            filteredPagingDefinition.Filters = searchData.filterData;
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            var result = await _FormP1Service.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }


        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("User");
            FormP1Model _model = new FormP1Model();
            if (id > 0)
                _model.FormP1Header = await _FormP1Service.GetHeaderById(id);
            else
                _model.FormP1Header = new FormP1HeaderResponseDTO();

            _model.view = isview;

            if ((_model.FormP1Header.UseridSo == null || _model.FormP1Header.UseridSo == 0 || _model.FormP1Header.SubmitSts == false))
            {
                _model.FormP1Header.UseridSo = _security.UserID;
                _model.FormP1Header.SignDateSo = DateTime.Today;
                _model.FormP1Header.SignSo = true;
            }
             

            return PartialView("~/Views/FormP1/AddFormP1.cshtml", _model);
        }

      

        public async Task<IActionResult> SaveFormP1(FormP1HeaderResponseDTO FormP1Header)
        {
            var res = await _FormP1Service.SaveFormP1(FormP1Header);
            return Json(res);
        }

        public async Task<IActionResult> UpdateFormP1(FormP1HeaderResponseDTO FormP1Header)
        {
            await _FormP1Service.UpdateFormP1(FormP1Header, FormP1Header.FormP1Details);
            return Json(1);
        }

        public async Task<IActionResult> DeleteFormP1(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _FormP1Service.DeleteFormP1(id);
            return Json(rowsAffected);
        }

        //public async Task<IActionResult> FormP1Download(int id, [FromServices] IWebHostEnvironment _environment)
        //{
        //    var content1 = await _FormP1Service.FormDownload("FORMP1", id, _environment.WebRootPath + "/Templates/FORMP1.xlsx");
        //    string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    return File(content1, contentType1, "FORMP1" + ".xlsx");
        //}

    }
}

