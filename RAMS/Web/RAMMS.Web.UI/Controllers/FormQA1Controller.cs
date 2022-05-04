﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.Web.UI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO;
using RAMMS.DTO.Wrappers;
using System.Collections.Generic;
using RAMMS.Business.ServiceProvider;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.IO;

namespace RAMMS.Web.UI.Controllers
{
    public class FormQA1Controller : Models.BaseController
    {
        private IHostingEnvironment Environment;
        private readonly ILogger _logger;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFormN1Service _formN1Service;
        private readonly IFormJServices _formJService;
        private readonly IFormQa1Service _formQa1Service;
        private readonly IRoadMasterService _roadMasterService;
        private readonly IDDLookupBO _dDLookupBO;
        private readonly IFormV2Service _formV2Service;

        FormQa1Model _formQa1Model = new FormQa1Model();
        public FormQA1Controller(IHostingEnvironment _environment,
            IDDLookupBO _ddLookupBO,
            IDDLookUpService ddLookupService,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IFormN1Service formN1Service,
            IFormJServices formJServices,
            ISecurity security,
            IRoadMasterService roadMasterService,
            ILogger logger,
            IFormQa1Service formQa1Service,
            IFormV2Service formV2Service
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
            _formQa1Service = formQa1Service ?? throw new ArgumentNullException(nameof(formQa1Service));
            _roadMasterService = roadMasterService ?? throw new ArgumentNullException(nameof(roadMasterService));
            _logger = logger;
            _formV2Service = formV2Service ?? throw new ArgumentNullException(nameof(formV2Service));
        }

        private List<SelectListItem> DDLYESNO()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Yes", Value = "Yes" });
            list.Add(new SelectListItem { Text = "No", Value = "No" });
            return list;
        }

        private List<SelectListItem> DDLGOODFAIRINADE()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Good", Value = "Good" });
            list.Add(new SelectListItem { Text = "Fiar", Value = "Fair" });
            list.Add(new SelectListItem { Text = "Inadequate Capacity", Value = "Inadequate Capacity" });
            return list;
        }

        private List<SelectListItem> DDLWELLNO()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Well", Value = "Well" });
            list.Add(new SelectListItem { Text = "No", Value = "No" });
            return list;
        }

        private List<SelectListItem> DDLADENO()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Adequate", Value = "Adequate" });
            list.Add(new SelectListItem { Text = "No", Value = "No" });
            return list;
        }

        private List<SelectListItem> DDLSTHD()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Soft", Value = "Soft" });
            list.Add(new SelectListItem { Text = "Hard", Value = "Hard" });
            return list;
        }

        private async Task LoadDropDown()
        {
            DDLookUpDTO ddLookup = new DDLookUpDTO();

            ViewData["RD_Code"] = await _formN1Service.GetRoadCodesByRMU("");
            //ViewData["RMU"] = await _formN1Service.GetRMU();

            ddLookup.Type = "ACT-CWF";
            ViewData["Activity"] = await _ddLookupService.GetLookUpCodeTextConcat(ddLookup);

            LoadLookupService("User", "RMU");

            ddLookup.Type = "Week No";
            ViewData["WeekNo"] = await _ddLookupService.GetDdDescValue(ddLookup);

            FormASearchDropdown ddl = _formJService.GetDropdown(new RequestDropdownFormA { });

            ViewData["SectionCode"] = ddl.Section.Select(s => new SelectListItem { Text = s.Text, Value = s.Value }).ToArray();

            ddLookup.Type = "Year";
            var year = await _ddLookupService.GetDdLookup(ddLookup);
            ViewData["Year"] = year.Select(l => new SelectListItem { Selected = (l.Value == DateTime.Today.Year.ToString()), Text = l.Text, Value = l.Value });

            ddLookup.Type = "Day";
            var day = await _ddLookupService.GetDdLookup(ddLookup);
            ViewData["Day"] = day.Select(l => new SelectListItem { Selected = (l.Value == DateTime.Now.DayOfWeek.ToString()), Text = l.Text, Value = l.Value });

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            var weekno = cal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule,
                                         dfi.FirstDayOfWeek);

            var weekNoLst = _dDLookupBO.GetWeekNo();
            ViewData["WeekNo"] = weekNoLst.Select(l => new SelectListItem { Selected = (l.Value == weekno.ToString()), Text = l.Text, Value = l.Value });

            ViewData["YESNO"] = DDLYESNO();

            ViewData["WELLNO"] = DDLWELLNO();

            ViewData["ADENO"] = DDLADENO();

            ViewData["STHD"] = DDLSTHD();

        }

        public async Task<IActionResult> QA1()
        {
            await LoadDropDown();
            return View("~/Views/MAM/FormQa1/FormQa1.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> LoadFormQa1List(DataTableAjaxPostModel<FormQa1SearchGridDTO> formQa1Filter)
        {

            if (Request.Form.ContainsKey("columns[0][search][value]"))
            {
                formQa1Filter.filterData.SmartInputValue = Request.Form["columns[0][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[1][search][value]"))
            {
                formQa1Filter.filterData.RMU = Request.Form["columns[1][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[2][search][value]"))
            {
                formQa1Filter.filterData.Section = Request.Form["columns[2][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[3][search][value]"))
            {
                formQa1Filter.filterData.Crew = Request.Form["columns[3][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[4][search][value]"))
            {
                formQa1Filter.filterData.ActivityCode = Request.Form["columns[4][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[5][search][value]"))
            {
                formQa1Filter.filterData.ByFromdate = Request.Form["columns[5][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[6][search][value]"))
            {
                formQa1Filter.filterData.ByTodate = Request.Form["columns[6][search][value]"].ToString();
            }
            if (Request.Form.ContainsKey("columns[7][search][value]"))
            {
                formQa1Filter.filterData.RoadCode = Request.Form["columns[7][search][value]"].ToString();
            }

            FilteredPagingDefinition<FormQa1SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormQa1SearchGridDTO>();
            filteredPagingDefinition.Filters = formQa1Filter.filterData;
            filteredPagingDefinition.RecordsPerPage = formQa1Filter.length;
            filteredPagingDefinition.StartPageNo = formQa1Filter.start;

            if (formQa1Filter.order != null)
            {
                filteredPagingDefinition.ColumnIndex = formQa1Filter.order[0].column;
                filteredPagingDefinition.sortOrder = formQa1Filter.order[0].SortOrder == SortDirection.Asc ? SortOrder.Ascending : SortOrder.Descending;
            }

            var result = await _formQa1Service.GetFilteredFormQa1Grid(filteredPagingDefinition).ConfigureAwait(false);

            return Json(new { draw = formQa1Filter.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });
        }


        [HttpPost]
        public async Task<IActionResult> HeaderListFormQa1Delete(int pkId)
        {
            int rowsAffected = 0;
            rowsAffected = await _formQa1Service.DeleteFormQA1(pkId);
            return Json(rowsAffected);

        }


        public async Task<IActionResult> EditFormQa1(int id)
        {
            await LoadDropDown();
            if (id == 0)
            {
                _formQa1Model.SaveFormQa1Model = new FormQa1HeaderDTO();
                _formQa1Model.SaveFormQa1Model.Lab = new FormQa1LabDTO();
                _formQa1Model.SaveFormQa1Model.Wcq = new FormQa1WcqDTO();
                _formQa1Model.SaveFormQa1Model.We = new FormQa1WeDTO();
                _formQa1Model.SaveFormQa1Model.EqVh = new List<FormQa1EqVhDTO>();
                _formQa1Model.SaveFormQa1Model.Mat = new List<FormQa1MatDTO>();
                _formQa1Model.SaveFormQa1Model.Ssc = new FormQa1SscDTO();
                _formQa1Model.SaveFormQa1Model.Gc = new FormQa1GCDTO();
                _formQa1Model.SaveFormQa1Model.Gen = new List<FormQa1GenDTO>();
                _formQa1Model.SaveFormQa1Model.Tes = new FormQa1TesDTO();
                _formQa1Model.SaveFormQa1Model.Tes.RmFormQa1Image = new List<FormQa1AttachmentDTO>();
            }
            else
            {
                _formQa1Model.SaveFormQa1Model = await _formQa1Service.GetFormQA1(id);
                if (_formQa1Model.SaveFormQa1Model.SubmitSts)
                    _formQa1Model.viewm = "1";

                if (_formQa1Model.SaveFormQa1Model.Tes != null)
                {
                    var imges = await _formQa1Service.GetImages(_formQa1Model.SaveFormQa1Model.Tes.PkRefNo);
                    _formQa1Model.SaveFormQa1Model.Tes.RmFormQa1Image = imges;
                }

            }

            return View("~/Views/MAM/FormQa1/_AddFormQA1.cshtml", _formQa1Model);
        }

        [HttpPost]
        public IActionResult GetRoadCodeBySection(int secCode)
        {
            var obj = _roadMasterService.GetRoadCodeBySectionCode(secCode).Result;
            return Json(obj);
        }

        [HttpPost]
        public async Task<JsonResult> FindDetails(FormQa1Model header, bool create = false)
        {
            FormQa1HeaderDTO formQa1 = new FormQa1HeaderDTO();
            FormQa1HeaderDTO formQa1Res = new FormQa1HeaderDTO();

            formQa1 = header.SaveFormQa1Model;
            formQa1.Dt = header.FormQa1Date;

            formQa1Res = await _formQa1Service.FindQa1Details(formQa1);
            //Qa1 Not Exisit
            if (formQa1Res == null)
            {
                formQa1.Dt = header.FormQa1Date;
                formQa1.UseridAssgn = _security.UserID;
                formQa1.UsernameAssgn = _security.UserName;
                formQa1.DtAssgn = DateTime.Today;
                formQa1.CrBy = _security.UserID;
                formQa1.ModBy = _security.UserID;
                formQa1.ModDt = DateTime.Now;
                formQa1.CrDt = DateTime.Now;
                formQa1Res = await _formQa1Service.FindAndSaveFormQA1Hdr(formQa1, false);
                formQa1Res.Tes = await _formQa1Service.GetTes(formQa1Res.PkRefNo);
            }
            return Json(formQa1Res, JsonOption());
        }

        [HttpPost]
        public async Task<IActionResult> EditFormQa1Gen(int id)
        {
            var _formQa1GenModel = new FormQa1GenDTO();
            if (id > 0)
            {
                _formQa1GenModel = await _formQa1Service.GetGenDetails(id);
            }

            return PartialView("~/Views/MAM/FormQa1/_GeneralAdd.cshtml", _formQa1GenModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFormMaterial(int id)
        {
            var _formQa1MaterialModel = new FormQa1MatDTO();

            DDLookUpDTO ddLookup = new DDLookUpDTO();
            ddLookup.Type = "MaterialUnit";
            ViewData["Unit"] = await _ddLookupService.GetDdLookup(ddLookup);
            ViewData["YESNO"] = DDLYESNO();
            if (id > 0)
            {
                _formQa1MaterialModel = await _formQa1Service.GetMatDetails(id);
            }

            return PartialView("~/Views/MAM/FormQa1/_MaterialAdd.cshtml", _formQa1MaterialModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFormEquipment(int id)
        {
            var _formQa1EqpModel = new FormQa1EqVhDTO();
            ViewData["EQPList"] = await _formV2Service.GetEquipmentCode();
            ViewData["GOODFAIRINADE"] = DDLGOODFAIRINADE();
            if (id > 0)
            {
                _formQa1EqpModel = await _formQa1Service.GetEquipDetails(id);
            }

            return PartialView("~/Views/MAM/FormQa1/_EquipAdd.cshtml", _formQa1EqpModel);
        }

        [HttpPost]
        public async Task<IActionResult> LoadFormQa1EquipmentList(DataTableAjaxPostModel<FormQa1SearchGridDTO> formQa1Filter, int id)
        {

            FilteredPagingDefinition<FormQa1SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormQa1SearchGridDTO>();
            filteredPagingDefinition.RecordsPerPage = formQa1Filter.length;
            filteredPagingDefinition.StartPageNo = formQa1Filter.start;

            var result = await _formQa1Service.GetEquipmentFormQa1Grid(filteredPagingDefinition, id).ConfigureAwait(false);

            return Json(new { draw = formQa1Filter.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });
        }

        [HttpPost]
        public async Task<IActionResult> LoadFormQa1GenList(DataTableAjaxPostModel<FormQa1SearchGridDTO> formQa1Filter, int id)
        {

            FilteredPagingDefinition<FormQa1SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormQa1SearchGridDTO>();
            filteredPagingDefinition.RecordsPerPage = formQa1Filter.length;
            filteredPagingDefinition.StartPageNo = formQa1Filter.start;

            var result = await _formQa1Service.GetGeneralFormQa1Grid(filteredPagingDefinition, id).ConfigureAwait(false);

            return Json(new { draw = formQa1Filter.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });
        }

        [HttpPost]
        public async Task<IActionResult> LoadFormQa1MaterialList(DataTableAjaxPostModel<FormQa1SearchGridDTO> formQa1Filter, int id)
        {

            FilteredPagingDefinition<FormQa1SearchGridDTO> filteredPagingDefinition = new FilteredPagingDefinition<FormQa1SearchGridDTO>();
            filteredPagingDefinition.RecordsPerPage = formQa1Filter.length;
            filteredPagingDefinition.StartPageNo = formQa1Filter.start;

            var result = await _formQa1Service.GetMaterialFormQa1Grid(filteredPagingDefinition, id).ConfigureAwait(false);

            return Json(new { draw = formQa1Filter.draw, recordsFiltered = result.TotalRecords, recordsTotal = result.TotalRecords, data = result.PageResult });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEquipment(FormQa1EqVhDTO formQa1Eq)
        {
            var ret = _formQa1Service.SaveEquipment(formQa1Eq);
            return Json(new { ret });
        }

        [HttpPost]
        public async Task<IActionResult> SaveMaterial(FormQa1MatDTO formQa1Mat)
        {
            var ret = _formQa1Service.SaveMaterial(formQa1Mat);
            return Json(new { ret });
        }

        [HttpPost]
        public async Task<IActionResult> SaveGeneral(FormQa1GenDTO formQa1Gen)
        {
            var ret = _formQa1Service.SaveGeneral(formQa1Gen);
            return Json(new { ret });
        }

        [HttpPost]
        public async Task<IActionResult> SaveHeader(FormQa1HeaderDTO formQa1, bool submitStatus)
        {
            formQa1.ModBy = _security.UserID;
            formQa1.ModDt = DateTime.Today;
            var ret = _formQa1Service.SaveFormQA1(formQa1, submitStatus);
            return Json(new { ret });
        }

        [HttpPost]
        public async Task<IActionResult> ImageUploadFormQA1(IList<IFormFile> formFile, string PkRefNo, int Qa1PkRefNo, int row)
        {
            try
            {
                bool successFullyUploaded = false;
                string wwwPath = this._webHostEnvironment.WebRootPath;
                string contentPath = this._webHostEnvironment.ContentRootPath;
                string _id = Regex.Replace(PkRefNo, @"[^0-9a-zA-Z]+", "");
                string fileName = "";
                int j = 0;
                string source = "";

                switch (row)
                {
                    case 1:
                        source = "Compaction_Test";
                        break;
                    case 2:
                        source = "Density_Test";
                        break;
                    case 3:
                        source = "Material_Grading_Test";
                        break;
                    case 4:
                        source = "CBR";
                        break;
                    case 5:
                        source = "Other_Tests";
                        break;
                }

                FormQa1AttachmentDTO _rmAssetImageDtl = new FormQa1AttachmentDTO();
                foreach (IFormFile postedFile in formFile)
                {
                    List<FormQa1AttachmentDTO> uploadedFiles = new List<FormQa1AttachmentDTO>();


                    string subPath = Path.Combine(@"Uploads/FormQA1/", Qa1PkRefNo.ToString());
                    string path = Path.Combine(wwwPath, Path.Combine(@"Uploads\FormQA1\", Qa1PkRefNo.ToString()));
                    string ext = Path.GetExtension(postedFile.FileName);
                    fileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string fileRename = Qa1PkRefNo.ToString() + "_" + source + "_" + fileName + "_" + "0" + row + ext;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream stream = new FileStream(Path.Combine(path, fileRename), FileMode.Create))
                    {
                        _rmAssetImageDtl.Fqa1TesPkRefNo = PkRefNo;
                        _rmAssetImageDtl.Fqa1PkRefNo = Qa1PkRefNo;
                        _rmAssetImageDtl.ImageTypeCode = fileName;
                        _rmAssetImageDtl.ImageUserFilePath = postedFile.FileName;
                        _rmAssetImageDtl.ImageSrno = row;
                        _rmAssetImageDtl.Source = source;

                        _rmAssetImageDtl.ActiveYn = true;
                        _rmAssetImageDtl.ImageFilenameSys = Qa1PkRefNo.ToString() + "_" + source + "_" + "0" + row;

                        _rmAssetImageDtl.ImageFilenameUpload = $"{subPath}/{fileRename}";
                        postedFile.CopyTo(stream);
                    }
                    uploadedFiles.Add(_rmAssetImageDtl);
                    if (uploadedFiles.Count() > 0)
                    {
                        await _formQa1Service.SaveImage(uploadedFiles);
                        successFullyUploaded = true;
                    }
                    else
                    {
                        successFullyUploaded = false;
                    }

                    j = j + 1;
                }

                return Json(new { status = successFullyUploaded, row, filename = fileName, filepath = _rmAssetImageDtl.ImageFilenameUpload });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(int pKRefNo)
        {
            var ret = await _formQa1Service.DeActivateImage(pKRefNo);
            return Json(new { ret });
        }

        [HttpPost]
        public async Task<IActionResult> GetImages(int tesPKRefNo, int row = 0)
        {
            var ret = await _formQa1Service.GetImages(tesPKRefNo, row);
            return Json(new { ret });
        }

        [HttpPost]
        public IActionResult GetAttachment()
        {
            return PartialView("~/Views/MAM/FormQa1/_Attachment.cshtml");
        }

    }
}