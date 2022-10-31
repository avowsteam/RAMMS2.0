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
    public class FormPAController : BaseController
    {
        private IFormPAService _FormPAService;
        private readonly IFormJServices _formJService;
        private ISecurity _security;
        private IUserService _userService;
        private IRoadMasterService _roadMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormPAController(
            IFormPAService service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices)
        {
            _userService = userService;
            _FormPAService = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _webHostEnvironment = webhostenvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHeaderList(DataTableAjaxPostModel<FormPASearchGridDTO> searchData)
        {

            FilteredPagingDefinition<FormPASearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormPASearchGridDTO>();
            searchData.filterData = searchData.filterData ?? new FormPASearchGridDTO();
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



            filteredPagingDefinition.Filters = searchData.filterData;
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            var result = await _FormPAService.GetHeaderList(filteredPagingDefinition);
            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });

        }


        public async Task<IActionResult> Add(int id, int isview)
        {
            LoadLookupService("User");
            FormPAModel _model = new FormPAModel();
            if (id > 0)
            {
                _model.FormPAHeader = await _FormPAService.GetHeaderById(id);
                _model.FormPACRR = _model.FormPAHeader.RmPaymentCertificateCrr.Count == 0 ? GetListRmPaymentCertificateCrr() : _model.FormPAHeader.RmPaymentCertificateCrr;
                _model.FormPACRRA = _model.FormPAHeader.RmPaymentCertificateCrra.Count == 0 ? GetListRmPaymentCertificateCrra() : _model.FormPAHeader.RmPaymentCertificateCrra;
                _model.FormPACRRD = _model.FormPAHeader.RmPaymentCertificateCrrd.Count == 0 ? GetListRmPaymentCertificateCrrd() : _model.FormPAHeader.RmPaymentCertificateCrrd;
            }
            else
            {
                _model.FormPAHeader = new FormPAHeaderResponseDTO();
                _model.FormPACRR = GetListRmPaymentCertificateCrr();
                _model.FormPACRRA = GetListRmPaymentCertificateCrra();
                _model.FormPACRRD = GetListRmPaymentCertificateCrrd();
            }



            _model.view = isview;

            if ((_model.FormPAHeader.UseridSo == null || _model.FormPAHeader.UseridSo == 0 || _model.FormPAHeader.SubmitSts == false))
            {
                _model.FormPAHeader.UseridSo = _security.UserID;
                _model.FormPAHeader.SignDateSo = DateTime.Today;
                _model.FormPAHeader.SignSo = true;
            }


            return PartialView("~/Views/FormPA/AddFormPA.cshtml", _model);
        }

        private List<FormPACRRResponseDTO> GetListRmPaymentCertificateCrr()
        {
            List<FormPACRRResponseDTO> FormPACRR = new List<FormPACRRResponseDTO>();
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Kuching" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Samarahan" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Serian" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Sri Aman" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Betong" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Sarikei" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Mukah" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Sibu" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Kapit" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Bintulu" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Miri" });
            FormPACRR.Add(new FormPACRRResponseDTO { Division = "Limbang" });
            return FormPACRR;
        }

        private List<FormPACRRAResponseDTO> GetListRmPaymentCertificateCrra()
        {
            List<FormPACRRAResponseDTO> FormPACRRA = new List<FormPACRRAResponseDTO>();
            FormPACRRA.Add(new FormPACRRAResponseDTO { Description = "Adjustable Quantity(Clause 12.4 of the Agreement)#" });
            FormPACRRA.Add(new FormPACRRAResponseDTO { Description = "Ancillary Cost(Clause 12.5 of the Agreement)#" });
            FormPACRRA.Add(new FormPACRRAResponseDTO { Description = "Payment withheld(Clause 9.2 of the Agreement)#" });
            FormPACRRA.Add(new FormPACRRAResponseDTO { Description = "Positive Adjustment from Previous Payment Certificate(if any)" });
            return FormPACRRA;
        }

        private List<FormPACRRDResponseDTO> GetListRmPaymentCertificateCrrd()
        {
            List<FormPACRRDResponseDTO> FormPACRRD = new List<FormPACRRDResponseDTO>();
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Adjustable Quantity(Clause 12.4 of the Agreement)*" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Ancillary Cost(Clause 12.5 of the Agreement)*" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Deduction(Clause 9.1.1(a) of the Agreement)#" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Deduction(Clause 9.1.3 of the Agreement)" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Administrative charges(Clause 9.1.1(c) of the Agreement)" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Administrative charges(Clause 9.3.5 of the Agreement)" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Payment withheld(Clause 9.2 of the Agreement)#" });
            FormPACRRD.Add(new FormPACRRDResponseDTO { Description = "Negative Adjustment from Previous Payment Certificate(if any)" });
            return FormPACRRD;
        }


        public async Task<IActionResult> SaveFormPA(FormPAHeaderResponseDTO FormPAHeader)
        {
            var res = await _FormPAService.SaveFormPA(FormPAHeader);
            return Json(res);
        }

        public async Task<IActionResult> UpdateFormPA(FormPAHeaderResponseDTO FormPAHeader)
        {
            await _FormPAService.UpdateFormPA(FormPAHeader, FormPAHeader.RmPaymentCertificateCrr, FormPAHeader.RmPaymentCertificateCrra, FormPAHeader.RmPaymentCertificateCrrd);
            return Json(1);
        }

        public async Task<IActionResult> DeleteFormPA(int id)
        {
            int? rowsAffected = 0;
            rowsAffected = _FormPAService.DeleteFormPA(id);
            return Json(rowsAffected);
        }

        //public async Task<IActionResult> FormPADownload(int id, [FromServices] IWebHostEnvironment _environment)
        //{
        //    var content1 = await _FormPAService.FormDownload("FORMPA", id, _environment.WebRootPath + "/Templates/FORMPA.xlsx");
        //    string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    return File(content1, contentType1, "FORMPA" + ".xlsx");
        //}

    }
}

