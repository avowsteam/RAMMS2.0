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
    public class FormB13Controller : BaseController
    {
        private IFormB13Service _FormB13Service;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormB13Controller(
            IFormB13Service service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormB13Service = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormB13SearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormB13SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormB13SearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormB13SearchGridDTO();
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
            var result = await _FormB13Service.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }


        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("User");
            FormB13Model _model = new FormB13Model();
            if (id > 0)
                _model.FormB13 = await _FormB13Service.GetHeaderById(id);
            else
                _model.FormB13 = new FormB13ResponseDTO();

            _model.view = isview;

            if ((_model.FormB13.UseridProsd == null || _model.FormB13.UseridProsd == 0 || _model.FormB13.SubmitSts == false))
            {
                _model.FormB13.UseridProsd = _security.UserID;
                _model.FormB13.DtProsd = DateTime.Today;
                _model.FormB13.SignProsd = true;
            }
            else if ((_model.FormB13.UseridFclitd == null || _model.FormB13.UseridFclitd == 0) && _model.FormB13.Status == RAMMS.Common.StatusList.Submitted)
            {
                _model.FormB13.UseridFclitd = _security.UserID;
                _model.FormB13.DtFclitd = DateTime.Today;
                _model.FormB13.SignFclitd = true;
            }
            else if ((_model.FormB13.UseridAgrd == null || _model.FormB13.UseridAgrd == 0) && _model.FormB13.Status == RAMMS.Common.StatusList.Facilitated)
            {
                _model.FormB13.UseridAgrd = _security.UserID;
                _model.FormB13.DtAgrd = DateTime.Today;
                _model.FormB13.SignAgrd = true;
            }
            else if ((_model.FormB13.UseridEdosd == null || _model.FormB13.UseridEdosd == 0) && _model.FormB13.Status == RAMMS.Common.StatusList.Agreed)
            {
                _model.FormB13.UseridEdosd = _security.UserID;
                _model.FormB13.DtEdosd = DateTime.Today;
                _model.FormB13.SignEdosd = true;
            }


            return PartialView("~/Views/FormB13/_AddFormB13.cshtml", _model);
        }

        public async Task<IActionResult> GetMaxRev(int Year, string RMU)
        {
            return Json(_FormB13Service.GetMaxRev(Year, RMU));
        }

        public async Task<IActionResult> SaveFormB13(FormB13ResponseDTO FormB13)
        {
            FormB13 = await _FormB13Service.SaveFormB13(FormB13);
            return Json(FormB13.PkRefNo);
        }

        public async Task<IActionResult> UpdateFormB13(FormB13ResponseDTO FormB13)
        {
            await _FormB13Service.UpdateFormB13(FormB13, FormB13.FormB13History);
            return Json(1);
        }

        public async Task<IActionResult> DeleteFormB13(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _FormB13Service.DeleteFormB13(id);
            return Json(rowsAffected);
        }

        public async Task<IActionResult> FormB13Download(int id, [FromServices] IWebHostEnvironment _environment)
        {
            var content1 = await _FormB13Service.FormDownload("FORMB13", id, _environment.WebRootPath + "/Templates/FORMB13.xlsx");
            string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(content1, contentType1, "FORMB13" + ".xlsx");
        }

    }
}

