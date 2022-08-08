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

namespace RAMMS.Web.UI.Controllers
{
    public class FormB7Controller : Models.BaseController
    {

        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFormN1Service _formN1Service;
        private readonly IFormJServices _formJService;
        private readonly IRoadMasterService _roadMasterService;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormG1G2Service _formG1G2Service;
        private readonly IAssetsService _AssetService;

        public FormB7Controller(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          IFormN1Service formN1Service,
          IFormJServices formJServices,
          ISecurity security,
          IRoadMasterService roadMasterService,
          ILogger logger,
          IFormG1G2Service formG1G2Service,
          IAssetsService assestService
          )
        {
            _userService = userService;
            _dDLookupBO = _ddLookupBO;
            Environment = _environment;
            _webHostEnvironment = webhostenvironment;
            _ddLookupService = ddLookupService;
            _security = security;
            _formN1Service = formN1Service ?? throw new ArgumentNullException(nameof(formN1Service));
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _roadMasterService = roadMasterService ?? throw new ArgumentNullException(nameof(roadMasterService));
            _logger = logger;
            _formG1G2Service = formG1G2Service ?? throw new ArgumentNullException(nameof(formG1G2Service));
            _AssetService = assestService;
        }

        public async Task<IActionResult> Index()
        {
            LoadLookupService("Year");
            var grid = new Models.CDataTable() { Name = "tblFB7HGrid", APIURL = "/FormB7/HeaderList", LeftFixedColumn = 1 };
            grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget) && _security.isOperRAMSExecutive;
            grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = null, title = "Action", IsFreeze = true, sortable = false, render = "frmB7.HeaderGrid.ActionRender" });
            grid.Columns.Add(new CDataColumns() { data = "B7lRevisionYear", title = "Year" });
            grid.Columns.Add(new CDataColumns() { data = "B7lRevisionNo", title = "Revision Number" });
            grid.Columns.Add(new CDataColumns() { data = "B7lRevisionDate", title = "Revision Date", render = "frmB7.HeaderGrid.DateOfIns" });
            grid.Columns.Add(new CDataColumns() { data = "B7lCrByName", title = "User Name" });
            return View(grid);
        }

        //public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        //{
        //    if (searchData.order != null && searchData.order.Count > 0)
        //    {
        //        searchData.order = searchData.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
        //    }
        //    return Json(await _C1C2Service.GetHeaderGrid(searchData), JsonOption());
        //}

        public async Task<IActionResult> Edit(int id, int view)
        {

            FormB7Model _model = new FormB7Model();
            if (id > 0)
            {
                //  _model.FormB9 = await _FormB9Service.GetHeaderById(id);
            }
            else
            {
                _model.FormB7Labour  = new FormB7LabourDTO();
            }



            return PartialView("~/Views/FormB7/_AddFormB7.cshtml", _model);
        }
    }
}
