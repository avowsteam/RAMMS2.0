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
        private IWebHostEnvironment _environment;
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
 

        //public async Task<IActionResult> Add(int id, int view)
        //{
        //    LoadLookupService("Supervisor", "User");

        //    FormASearchDropdown ddl = _formJService.GetDropdown(new RequestDropdownFormA { });
        //    ViewData["SectionCode"] = ddl.Section.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();

        //    FormB9Model _model = new FormB9Model();
        //    if (id > 0)
        //    {
        //        _model.FormB9 = await _FormB9Service.GetHeaderById(id);
        //    }
        //    else
        //    {
        //        _model.FormB9 = new FormB9ResponseDTO();
        //    }

        //    _model.FormB9Dtl = new FormB9DtlResponseDTO();
        //    _model.FormB9 = _model.FormB9 ?? new FormB9ResponseDTO();
        //    _model.view = view;


        //    if (_model.FormB9.UseridRcd == 0)
        //    {
        //        _model.FormB9.UseridRcd = _security.UserID;
        //        _model.FormB9.DateRcd = DateTime.Today;
        //        _model.FormB9.SignRcd = true;

        //    }
        //    if (_model.FormB9.UseridHdd == 0 && _model.FormB9.Status == RAMMS.Common.StatusList.Submitted)
        //    {
        //        _model.FormB9.UseridHdd = _security.UserID;
        //        _model.FormB9.DateHdd = DateTime.Today;
        //        _model.FormB9.SignHdd = true;
        //    }

        //    return PartialView("~/Views/FrmT/_AddFormB9.cshtml", _model);
        //}



        //public async Task<IActionResult> SaveFormB9(FormB9Model frm)
        //{
        //    int refNo = 0;
        //    frm.FormB9.ActiveYn = true;
        //    if (frm.FormB9.PkRefNo == 0)
        //    {
        //        frm.FormB9 = await _FormB9Service.SaveFormB9(frm.FormB9);

        //        return Json(new { FormExist = frm.FormB9.FormExist, RefId = frm.FormB9.PkRefId, PkRefNo = frm.FormB9.PkRefNo, Status = frm.FormB9.Status });
        //    }
        //    else
        //    {
        //        if (frm.FormB9.Status == "Initialize")
        //            frm.FormB9.Status = "Saved";
        //        refNo = await _FormB9Service.Update(frm.FormB9);
        //    }
        //    return Json(refNo);


        //}

       
        //public async Task<IActionResult> SaveFormB9Dtl(FormB9DtlResponseDTO FormB9Dtl)
        //{
        //    int? refNo = 0;


        //    if (FormB9Dtl.PkRefNo == 0)
        //    {
        //        refNo = _FormB9Service.SaveFormB9Dtl(FormB9Dtl);

        //    }
        //    else
        //    {
        //        _FormB9Service.UpdateFormB9Dtl(FormB9Dtl);
        //    }


        //    return Json(refNo);


        //}


        //public async Task<IActionResult> GetFormB9DtlById(int id)
        //{
        //    FormB9Model _model = new FormB9Model();
        //    _model.FormB9Dtl = new FormB9DtlResponseDTO();
        //    _model.FormB9Vechicle = new FormB9VehicleResponseDTO();
        //    if (id > 0)
        //    {
        //        _model.FormB9Dtl = await _FormB9Service.GetFormB9DtlById(id);
        //    }
        //    return PartialView("~/Views/FrmT/_VechicleDetails.cshtml", _model);

        //}

      
        //public async Task<IActionResult> FormB9Download(int id, [FromServices] IWebHostEnvironment _environment)
        //{
            //var content1 = await _FormB9Service.FormDownload("FormB9", id, _environment.WebRootPath + "/Templates/FormB9.xlsx");
            //string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //return File(content1, contentType1, "FormB9" + ".xlsx");
       // }

    }
}

