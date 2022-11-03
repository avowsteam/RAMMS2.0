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
    public class FormPBController : BaseController
    {
        private IFormPBService _FormPBService;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormPBController(
            IFormPBService service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormPBService = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormPBSearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormPBSearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormPBSearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormPBSearchGridDTO();
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
                searchData.filterData.IWRefNo = Request.Form["columns[5][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[6][search][value]"))
            {
                searchData.filterData.ProjectTitle = Request.Form["columns[6][search][value]"].ToString();
            }


            filteredPagingDefinition.Filters = searchData.filterData;
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            var result = await _FormPBService.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }


        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("User");
            FormPBModel _model = new FormPBModel();
            if (id > 0)
            {
                _model.FormPBHeader = await _FormPBService.GetHeaderById(id);
                _model.FormPBDetail = _model.FormPBHeader.FormPBDetails;
            }
            else
            {
                _model.FormPBHeader = new FormPBHeaderResponseDTO();
                _model.FormPBDetail = new List<FormPBDetailResponseDTO>();
            }



            _model.view = isview;

            if ((_model.FormPBHeader.UseridSp == null || _model.FormPBHeader.UseridSp == 0 || _model.FormPBHeader.SubmitSts == false))
            {
                _model.FormPBHeader.UseridSp = _security.UserID;
                _model.FormPBHeader.SignDateSp = DateTime.Today;
                _model.FormPBHeader.SignSp = true;
            }
            else if ((_model.FormPBHeader.UseridEc == null || _model.FormPBHeader.UseridEc == 0) && _model.FormPBHeader.Status == RAMMS.Common.StatusList.Submitted)
            {
                _model.FormPBHeader.UseridEc = _security.UserID;
                _model.FormPBHeader.SignDateEc = DateTime.Today;
                _model.FormPBHeader.SignEc = true;
            }
            else if ((_model.FormPBHeader.UseridSo == null || _model.FormPBHeader.UseridSo == 0) && _model.FormPBHeader.Status == RAMMS.Common.StatusList.CertifiedbyEC)
            {
                _model.FormPBHeader.UseridSo = _security.UserID;
                _model.FormPBHeader.SignDateSo = DateTime.Today;
                _model.FormPBHeader.SignSo = true;
            }



            return PartialView("~/Views/FormPB/AddFormPB.cshtml", _model);
        }

      

        public async Task<IActionResult> SaveFormPB(FormPBHeaderResponseDTO FormPBHeader)
        {
            var res = await _FormPBService.SaveFormPB(FormPBHeader);
            return Json(res);
        }

      
        public async Task<IActionResult> UpdateFormPB(FormPBHeaderResponseDTO FormPBHeader)
        {
            await _FormPBService.UpdateFormPB(FormPBHeader, FormPBHeader.FormPBDetails);
            return Json(1);
        }

        public async Task<IActionResult> DeleteFormPB(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _FormPBService.DeleteFormPB(id);
            return Json(rowsAffected);
        }

        //public async Task<IActionResult> FormPBDownload(int id, [FromServices] IWebHostEnvironment _environment)
        //{
        //    var content1 = await _FormPBService.FormDownload("FORMPB", id, _environment.WebRootPath + "/Templates/FORMPB.xlsx");
        //    string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    return File(content1, contentType1, "FORMPB" + ".xlsx");
        //}

    }
}

