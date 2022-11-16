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
    public class FrmUCUAController : BaseController
    {
        private IFormUCUAService _formUCUAService;
        private IFormTService _formTService;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IWebHostEnvironment _environment;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FrmUCUAController(
            IFormUCUAService ucuaservice,
            IFormTService service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _formUCUAService = ucuaservice;
            _userService = userService;
            _formTService = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormUCUASearchGridDTO> searchData)
        {
            int _id = 0;
            DateTime dt;
            FilteredPagingDefinition<FormUCUASearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormUCUASearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormUCUASearchGridDTO();
            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                searchData.filterData.SmartSearch = Request.Form["columns[0][search][value]"].ToString();
            }

            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                searchData.filterData.ReportingName = Request.Form["columns[3][search][value]"].ToString() == "null" ? "" : Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[4][search][value]"))
            {
                searchData.filterData.Location = Request.Form["columns[4][search][value]"].ToString();

            }
            if (Request.Form.ContainsKey("columns[5][search][value]"))
            {
                searchData.filterData.WorkScope = Request.Form["columns[5][search][value]"].ToString();
            }
            
            if (Request.Form.ContainsKey("columns[6][search][value]"))
            {
                searchData.filterData.ReceivedDtFrom = Request.Form["columns[6][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[7][search][value]"))
            {
                searchData.filterData.ReceivedDtTo = Request.Form["columns[7][search][value]"].ToString();
            }


            filteredPagingDefinition.Filters = searchData.filterData;
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            var result = await _formUCUAService.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }

        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("Supervisor", "User");

            //FormASearchDropdown ddl = _formJService.GetDropdown(new RequestDropdownFormA { });
            //ViewData["SectionCode"] = ddl.Section.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();

            FormUCUAModel _model = new FormUCUAModel();
            if (id > 0)
            {
                _model.FormUCUA = await _formUCUAService.GetHeaderById(id);
            }
            else
            {
                _model.FormUCUA = new FormUCUAResponseDTO();
            }

            _model.FormUCUADtl = new FormTDtlResponseDTO();
            _model.FormUCUA = _model.FormUCUA ?? new FormUCUAResponseDTO();
            _model.view = isview;


            //if (_model.FormUCUA.UseridRcd == 0)
            //{
            //    _model.FormUCUA.UseridRcd = _security.UserID;
            //   // _model.FormUCUA.DateRcd = DateTime.Today;
            // //   _model.FormUCUA.SignRcd = true;

            //}
            //if (_model.FormUCUA.UseridHdd == 0 && _model.FormUCUA.Status == RAMMS.Common.StatusList.Submitted)
            //{
            //    _model.FormUCUA.UseridHdd = _security.UserID;
            //   // _model.FormUCUA.DateHdd = DateTime.Today;
            //   // _model.FormUCUA.SignHdd = true;
            //}

            return PartialView("~/Views/FrmUCUA/_AddUCUA.cshtml", _model);
        }
        public async Task<IActionResult> SaveFormUCUA(FormUCUAModel frm)
        {
            int refNo = 0;
           frm.FormUCUA.ActiveYn = true;
            if (frm.FormUCUA.PkRefNo == 0)
            {
                frm.FormUCUA = await _formUCUAService.SaveFormUCUA(frm.FormUCUA);

                return Json(new { FormExist = frm.FormUCUA.FormExist, RefId = frm.FormUCUA.RefId, PkRefNo = frm.FormUCUA.PkRefNo, Status = frm.FormUCUA.Status });
            }
            else
            {
                if (frm.FormUCUA.Status == "Initialize")
                    frm.FormUCUA.Status = "Saved";
                refNo = await _formUCUAService.Update(frm.FormUCUA);
            }
            return Json(refNo);

        }
        public async Task<IActionResult> FormTDownload(int id, [FromServices] IWebHostEnvironment _environment)
        {
            try
            {
                var content1 = await _formUCUAService.FormDownload("FORMUCUA", id, _environment.WebRootPath + "/Templates/FORMUCUA.xlsx");
                string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(content1, contentType1, "FORMUCUA" + ".xlsx");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IActionResult> DeleteFormUCUA(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _formUCUAService.DeleteFormUCUA(id);
            return Json(rowsAffected);
        }
    }
}
