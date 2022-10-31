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
    public class DLPController : Models.BaseController
    {
        private readonly ILogger _logger;
        private readonly ISecurity _security;
        private readonly IDlpSpiService _dlpSpiService;
        private readonly IDDLookUpService _ddLookupService;
        private readonly IFormAService _formAService;


        public DLPController(
           ISecurity security,
           ILogger logger,
           IDlpSpiService dlpSpiService,
            IDDLookUpService ddLookupService,
            IFormAService formAService
           )
        {

            _security = security;
            _dlpSpiService = dlpSpiService ?? throw new ArgumentNullException(nameof(dlpSpiService));

            _logger = logger;
            _ddLookupService = ddLookupService;
            _formAService = formAService ?? throw new ArgumentNullException(nameof(formAService));

        }

        public async Task<IActionResult> FormSPI()
        {
            LoadLookupService("Year");
            DLPModel model = new DLPModel();
            model.DivisionMiri = await _dlpSpiService.GetDivisionMiri(DateTime.Today.Year);
            model.RmuMiri = await _dlpSpiService.GetDivisionRMUMiri(DateTime.Today.Year);
            model.RmuBTN = await _dlpSpiService.GetDivisionRMUBTN(DateTime.Today.Year);
            return View(model);
        }

        public async Task<IActionResult> GetSPIByYearFormSPI(int Year)
        {
            DLPModel model = new DLPModel();
            model.DivisionMiri = await _dlpSpiService.GetDivisionMiri(Year);
            model.RmuMiri = await _dlpSpiService.GetDivisionRMUMiri(Year);
            model.RmuBTN = await _dlpSpiService.GetDivisionRMUBTN(Year);
            return Json(model);
        }

        public async Task<IActionResult> Save(List<SpiData> spiData)
        {
            await _dlpSpiService.Save(spiData);
            return Json(1);
        }

        #region RMI & IRI
        public async Task<IActionResult> FormIRI_RMI()
        {
            await LoadDropDowns();
            FormAModel formAModel = new FormAModel();
            FormASearchGridDTO filterData = new FormASearchGridDTO();
            formAModel.SearchObj = filterData;
            FormAModel _formAModel = new FormAModel();

            //return View("~/Views/NOD/FormA/landingpage.cshtml", _formAModel);
            return View(_formAModel);
        }

        public async Task<IActionResult> SaveIRI(DlpIRIModel model)
        {
            List<DlpIRIDTO> spiData = new List<DlpIRIDTO>();
            if (model.RmiiriRoadLength != 0)
                spiData.Add(IRIMapping(model, 1, model.RmiiriPercentage, model.RmiiriRoadLength, "RMI"));
            model.RmiiriPercentage1 = (model.RmiiriRoadLength1 / ((model.RmiiriRoadLength1.HasValue ? model.RmiiriRoadLength1.Value : 0) + (model.RmiiriRoadLength2.HasValue ? model.RmiiriRoadLength2.Value : 0) + (model.RmiiriRoadLength3.HasValue ? model.RmiiriRoadLength3.Value : 0))) * 100;
            model.RmiiriPercentage2 = (model.RmiiriRoadLength2 / ((model.RmiiriRoadLength1.HasValue ? model.RmiiriRoadLength1.Value : 0) + (model.RmiiriRoadLength2.HasValue ? model.RmiiriRoadLength2.Value : 0) + (model.RmiiriRoadLength3.HasValue ? model.RmiiriRoadLength3.Value : 0))) * 100;
            model.RmiiriPercentage3 = (model.RmiiriRoadLength3 / ((model.RmiiriRoadLength1.HasValue ? model.RmiiriRoadLength1.Value : 0) + (model.RmiiriRoadLength2.HasValue ? model.RmiiriRoadLength2.Value : 0) + (model.RmiiriRoadLength3.HasValue ? model.RmiiriRoadLength3.Value : 0))) * 100;

            if (model.RmiiriRoadLength1 != 0)
                spiData.Add(IRIMapping(model, 1, model.RmiiriPercentage1, model.RmiiriRoadLength1, "IRI"));

            if (model.RmiiriRoadLength2 != 0)
                spiData.Add(IRIMapping(model, 2, model.RmiiriPercentage2, model.RmiiriRoadLength2, "IRI"));

            if (model.RmiiriRoadLength3 != 0)
                spiData.Add(IRIMapping(model, 3, model.RmiiriPercentage3, model.RmiiriRoadLength3, "IRI"));

            await _dlpSpiService.SaveIRI(spiData);
            return Json(1);
        }

        private DlpIRIDTO IRIMapping(DlpIRIModel model, int? ConditionNo, decimal? RmiiriPercentage, decimal? RmiiriRoadLength, string type)
        {
            return new DlpIRIDTO
            {
                RmiiriConditionNo = ConditionNo,
                RmiiriCreatedDate = DateTime.Now,
                RmiiriPercentage = RmiiriPercentage,
                RmiiriRoadLength = RmiiriRoadLength,
                RmiiriType = type,
                RmiiriYear = model.RmiiriYear
            };
        }

        [HttpPost]
        public async Task<IActionResult> HeaderListEdit(DataTableAjaxPostModel<FormADetailsRequestDTO> searchData)
        {
            FormAModel _formAModel = new FormAModel();
            await LoadDropDowns();
            int headerId = searchData.filterData.HeaderNo;
            _formAModel.SaveFormAModel = new FormAHeaderRequestDTO();
            if (headerId > 0)
            {
                var Result = await _formAService.GetFormAWithDetailsByNoAsync(headerId);
                _formAModel.SaveFormAModel = Result;
                _formAModel.SaveFormAModel.Id = headerId.ToString();
            }
            DlpIRIModel _formIRIModel = new DlpIRIModel();
            searchData.length = 10;
            searchData.start = 0;

            //return PartialView("~/Views/DLP/_AddFormAView.cshtml", _formIRIModel);
            return PartialView("~/Views/DLP/_AddFormIRI_RMIView.cshtml", _formIRIModel);

        }


        public async Task LoadDropDowns()
        {
            DDLookUpDTO ddLookup = new DDLookUpDTO();
            RoadMasterRequestDTO roadMasterReq = new RoadMasterRequestDTO();
            ddLookup.Type = "FormA_Assets";
            ViewData["AssetListing"] = await _ddLookupService.GetDdLookup(ddLookup);

            var ddl = _formAService.GetDropdown(new RequestDropdownFormA { });

            ViewData["RD_Code"] = ddl.RoadCode.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();
            ddLookup.Type = "RD_Name";
            ViewData["RD_Name"] = await _ddLookupService.GetDdDescValue(ddLookup);
            ViewData["Section_Code"] = ddl.Section.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();

            ddLookup.Type = "RMU";
            ViewData["RMU"] = ddl.RMU.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();

            ddLookup.Type = "Month";
            ViewData["Months"] = await _ddLookupService.GetDdDescValue(ddLookup);

            ddLookup.Type = "Year";
            ViewData["Year"] = await _ddLookupService.GetDdLookup(ddLookup);
        }

        public IActionResult AddIRI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadHeaderList(DataTableAjaxPostModel<FormASearchGridDTO> searchData)
        {
            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                searchData.filterData.SmartInputValue = Request.Form["columns[0][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[1][search][value]"))
            {
                searchData.filterData.RMU = Request.Form["columns[1][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[2][search][value]"))
            {
                searchData.filterData.Section = Request.Form["columns[2][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                searchData.filterData.Road_Code = Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[4][search][value]"))
            {
                searchData.filterData.Asset_GroupCode = Request.Form["columns[4][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[5][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[5][search][value]"].ToString()))
                {
                    searchData.filterData.Month = Convert.ToInt32(Request.Form["columns[5][search][value]"].ToString());
                }
            }
            if (Request.Form.ContainsKey("columns[6][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[6][search][value]"].ToString()))
                {
                    searchData.filterData.Year = Convert.ToInt32(Request.Form["columns[6][search][value]"].ToString());
                }
            }
            if (Request.Form.ContainsKey("columns[7][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[7][search][value]"].ToString()))
                {
                    searchData.filterData.ChinageFromKm = Convert.ToInt32(Request.Form["columns[7][search][value]"].ToString());
                }
            }
            if (Request.Form.ContainsKey("columns[8][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[8][search][value]"].ToString()))
                {
                    searchData.filterData.ChinageFromM = Request.Form["columns[8][search][value]"].ToString();
                }
            }
            if (Request.Form.ContainsKey("columns[9][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[9][search][value]"].ToString()))
                {
                    searchData.filterData.ChinageToKm = Convert.ToInt32(Request.Form["columns[9][search][value]"].ToString());
                }
            }
            if (Request.Form.ContainsKey("columns[10][search][value]"))
            {
                if (!string.IsNullOrEmpty(Request.Form["columns[10][search][value]"].ToString()))
                {
                    searchData.filterData.ChinageToM = Request.Form["columns[10][search][value]"].ToString();
                }
            }

            FilteredPagingDefinition<FormASearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormASearchGridDTO>();

            filteredPagingDefinition.Filters = searchData.filterData;
            filteredPagingDefinition.RecordsPerPage = searchData.length; //Convert.ToInt32(Request.Form["length"]);
            filteredPagingDefinition.StartPageNo = searchData.start; //Convert.ToInt32(Request.Form["start"]); //TODO
            if (searchData.order != null)
            {
                filteredPagingDefinition.ColumnIndex = searchData.order[0].column;
                filteredPagingDefinition.sortOrder = searchData.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            var result = await _dlpSpiService.GetFilteredFormAGrid(filteredPagingDefinition).ConfigureAwait(false);

            if (result.PageResult.Count > 0)
            {
                for (int i = 0; i < result.PageResult.Count; i++)
                {
                    //result.PageResult[i].MonthYear = ((result.PageResult[i].Month ?? 0) < 10 ? "0" : "") + (result.PageResult[i].Month ?? 0) + "/" + (result.PageResult[i].Year.HasValue ? result.PageResult[i].Year.Value.ToString() : "2020");//TODO - hardcoded for demo - by John -  To be reworked
                    //result.PageResult[i].Status = result.PageResult[i].SubmitSts ? "Submitted" : "Saved";
                }
            }
            result.TotalRecords = result.PageResult.Count;

            return Json(new { draw = searchData.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });
        }

        public async Task<JsonResult> GetIRIData(int year)
        {
            return Json(await _dlpSpiService.GetIRIData(year), JsonOption());
        }

        #endregion

    }
}
