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
    public class AWPBController : Models.BaseController
    {


        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormB14Service _formB14Service;


        public AWPBController(IHostingEnvironment _environment,
          IDDLookupBO _ddLookupBO,
          IDDLookUpService ddLookupService,
          IUserService userService,
          IWebHostEnvironment webhostenvironment,
          ISecurity security,
          ILogger logger,
          IFormB14Service formB14Service,
          IAssetsService assestService
          )
        {
            _userService = userService;
            _dDLookupBO = _ddLookupBO;
            Environment = _environment;
            _webHostEnvironment = webhostenvironment;
            _ddLookupService = ddLookupService;
            _security = security;
            _logger = logger;
            _formB14Service = formB14Service ?? throw new ArgumentNullException(nameof(formB14Service));
        }
        public IActionResult Index()
        {
            var grid = new Models.CDataTable() { Name = "tblFAWPBHGrid", APIURL = "/AWPB/HeaderList", LeftFixedColumn = 1 };
            //grid.IsModify = _security.IsPCModify(ModuleNameList.Annual_Work_Planned_Budget);
            //grid.IsDelete = _security.IsPCDelete(ModuleNameList.Annual_Work_Planned_Budget) && _security.isOperRAMSExecutive;
            //grid.IsView = _security.IsPCView(ModuleNameList.Annual_Work_Planned_Budget);
            grid.Columns.Add(new CDataColumns() { data = "Feature", title = "Feature" , render = "frmAwpb.HeaderGrid.ActionRender", });
            grid.Columns.Add(new CDataColumns() { data = "ActivityCode", title = "Activity Code" });
            grid.Columns.Add(new CDataColumns() { data = "ActivityName", title = "Activity Name" });
            grid.Columns.Add(new CDataColumns() { data = "Jan", title = "Jan" });
            grid.Columns.Add(new CDataColumns() { data = "Feb", title = "Feb" });
            grid.Columns.Add(new CDataColumns() { data = "Mar", title = "Mar" });
            grid.Columns.Add(new CDataColumns() { data = "Apr", title = "Apr" });
            grid.Columns.Add(new CDataColumns() { data = "May", title = "May" });
            grid.Columns.Add(new CDataColumns() { data = "Jun", title = "Jun" });
            grid.Columns.Add(new CDataColumns() { data = "Jul", title = "Jul" });
            grid.Columns.Add(new CDataColumns() { data = "Aug", title = "Aug" });
            grid.Columns.Add(new CDataColumns() { data = "Sep", title = "Sep" });
            grid.Columns.Add(new CDataColumns() { data = "Oct", title = "Oct" });
            grid.Columns.Add(new CDataColumns() { data = "Nov", title = "Nov" });
            grid.Columns.Add(new CDataColumns() { data = "Dec", title = "Dec" });
            grid.Columns.Add(new CDataColumns() { data = "SubTotal", title = "Sub-Total" });
            grid.Columns.Add(new CDataColumns() { data = "Unit", title = "Unit" });
            return View(grid);
        }

        public async Task<JsonResult> HeaderList(DataTableAjaxPostModel searchData)
        {

            return Json(await _formB14Service.GetAWPBHeaderGrid(searchData), JsonOption());
        }
    }
}
