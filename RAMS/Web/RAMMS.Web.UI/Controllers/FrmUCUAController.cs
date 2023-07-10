using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Business.ServiceProvider.Services;
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
using System.Text.RegularExpressions;
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
        private readonly IFormW1Service _formW1Service;
        private readonly IFormWCService _formWCService;
        private readonly IFormWGService _formWGService;
        private readonly IFormWDService _formWDService;
        private readonly IFormWNService _formWNService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FrmUCUAController(
            IFormUCUAService ucuaservice,
            IFormTService service,
            ISecurity security,
            IUserService userService,
            IWebHostEnvironment webhostenvironment,
            IRoadMasterService roadMasterService,
             IFormJServices formJServices, IFormW1Service formW1Service, IFormWCService formWCService, IFormWGService formWGService
            , IFormWDService formWDService, IFormWNService formWNService)
        {
            _formUCUAService = ucuaservice;
            _userService = userService;
            _formTService = service;
            _security = security;
            _roadMasterService = roadMasterService;
            _formJService = formJServices ?? throw new ArgumentNullException(nameof(formJServices));
            _formW1Service = formW1Service ?? throw new ArgumentNullException(nameof(formW1Service));
            _formWCService = formWCService ?? throw new ArgumentNullException(nameof(formWCService));
            _formWGService = formWGService ?? throw new ArgumentNullException(nameof(formWGService));
            _formWDService = formWDService ?? throw new ArgumentNullException(nameof(formWDService));
            _formWNService = formWNService ?? throw new ArgumentNullException(nameof(formWNService));
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

            //_model.FormUCUADtl = new FormTDtlResponseDTO();
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
                var content1 = await _formUCUAService.FormDownload("FormUCUA", id, _environment.WebRootPath + "/Templates/FormUCUA.xlsx");
                string contentType1 = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(content1, contentType1, "FormUCUA" + ".xlsx");
            }
            catch (Exception ex)
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

        [HttpPost]
        public async Task<IActionResult> GetUCUAImageList(int Id, string assetgroup, string form)
        {

            //DDLookUpDTO ddLookup = new DDLookUpDTO();
            //FormIWImageModel assetsModel = new FormIWImageModel();
            //assetsModel.ImageList = new List<FormIWImageResponseDTO>();
            //assetsModel.ImageTypeList = new List<string>();
            //ddLookup.Type = "Photo Type";
            //ddLookup.TypeCode = "IW";
            ////assetsModel.PhotoType = await _ddLookupService.GetDdLookup(ddLookup);
            ////if (assetsModel.PhotoType.Count() == 0)
            ////{
            ////    assetsModel.PhotoType = new[]{ new SelectListItem
            ////    {
            ////        Text = "Others",
            ////        Value = "Others"
            ////    }};
            ////}
            ////ViewBag.PhotoTypeList = await _ddLookupService.GetDdLookup(ddLookup);
            //assetsModel.ImageList = await _formW1Service.GetImageList(Id);
            //assetsModel.IwRefNo = Id;


            //List<SelectListItem> newDdl = new List<SelectListItem>();
            //if (form == "FormWCWG")
            //{
            //assetsModel.FormName = "Form";
            ////var items = await _ddLookupService.GetDdLookup(ddLookup);
            //var _formWC = await _formWCService.FindWCByW1ID(int.Parse(Id));
            //var _formWG = await _formWGService.FindWGByW1ID(int.Parse(Id));
            //_formWC = _formWC == null ? new FormWCResponseDTO() : _formWC;
            //_formWG = _formWG == null ? new FormWGResponseDTO() : _formWG;

            //if (!_formWC.SubmitSts)
            //{
            //newDdl.Add(new SelectListItem("Form WC", "Form WC"));
            //assetsModel.FormName = assetsModel.FormName + "WC";
            //}
            //if (!_formWG.SubmitSts)
            //{
            //newDdl.Add(new SelectListItem("Form WG", "Form WG"));
            //assetsModel.FormName = assetsModel.FormName + "WG";
            //}
            //ViewData["FormType"] = (IEnumerable<SelectListItem>)newDdl;
            //assetsModel.IsSubmittedWC = _formWC.SubmitSts;
            //assetsModel.IsSubmittedWG = _formWG.SubmitSts;
            //}
            //else if (form == "FormWDWN")
            //{
            //assetsModel.FormName = "Form";
            ////var items = await _ddLookupService.GetDdLookup(ddLookup);
            //var _formWD = await _formWDService.FindWDByW1ID(int.Parse(Id));
            //var _formWN = await _formWNService.FindWNByW1ID(int.Parse(Id));

            //_formWD = _formWD == null ? new FormWDResponseDTO() : _formWD;
            //_formWN = _formWN == null ? new FormWNResponseDTO() : _formWN;

            //if (!_formWD.SubmitSts)
            //{
            //newDdl.Add(new SelectListItem("Form WD", "Form WD"));
            //assetsModel.FormName = assetsModel.FormName + "WD";
            //}
            //if (!_formWN.SubmitSts)
            //{
            //newDdl.Add(new SelectListItem("Form WN", "Form WN"));
            //assetsModel.FormName = assetsModel.FormName + "WN";
            //}
            //ViewData["FormType"] = (IEnumerable<SelectListItem>)newDdl;
            //assetsModel.IsSubmittedWD = _formWD.SubmitSts;
            //assetsModel.IsSubmittedWN = _formWN.SubmitSts;
            //}
            //else
            //{
            //assetsModel.FormName = form;

            //ViewData["FormType"] = (IEnumerable<SelectListItem>)newDdl;
            //}
            //assetsModel.ImageTypeList = assetsModel.ImageList.Select(c => c.ImageTypeCode).Distinct().ToList();

            List<FormUCUAImagesDTO> imageList = new List<FormUCUAImagesDTO>();
            imageList = _formUCUAService.ImageListWeb(Id);

            return PartialView("~/Views/FrmUCUA/_PhotoSectionPage.cshtml", imageList);
        }


        [HttpPost]
        public async Task<IActionResult> ImageUploadFormIw(IList<IFormFile> formFile, string PkRefNo, List<string> photoType, string Source = "ALL")
        {
            try
            {
                bool successFullyUploaded = false;
                string wwwPath = this._webHostEnvironment.WebRootPath;
                string contentPath = this._webHostEnvironment.ContentRootPath;
                string _id = Regex.Replace(PkRefNo, @"[^0-9a-zA-Z]+", "");

                int j = 0;
                foreach (IFormFile postedFile in formFile)
                {
                    List<FormUCUAImageResponseDTO> uploadedFiles = new List<FormUCUAImageResponseDTO>();
                    FormUCUAImageResponseDTO _rmAssetImageDtl = new FormUCUAImageResponseDTO();


                    string photo_Type = Regex.Replace(photoType[j], @"[^a-zA-Z]", "");
                    string subPath = Path.Combine(@"Uploads/FormW1/", _id, photo_Type);
                    string path = Path.Combine(wwwPath, Path.Combine(@"Uploads\FormW1\", _id, photo_Type));
                    int i = await _formUCUAService.LastInsertedIMAGENO(PkRefNo, photo_Type);
                    i++;
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string fileRename = i + "_" + photo_Type + "_" + fileName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream stream = new FileStream(Path.Combine(path, fileRename), FileMode.Create))
                    {
                        _rmAssetImageDtl.UCUARefNo = PkRefNo;
                        _rmAssetImageDtl.ImageTypeCode = photoType[j];
                        _rmAssetImageDtl.ImageUserFilePath = postedFile.FileName;
                        _rmAssetImageDtl.ImageSrno = i;
                        _rmAssetImageDtl.Source = Source;

                        _rmAssetImageDtl.ActiveYn = true;
                        if (i < 10)
                        {
                            _rmAssetImageDtl.ImageFilenameSys = _id + "_" + photo_Type + "_" + "00" + i;
                        }
                        else if (i >= 10 && i < 100)
                        {
                            _rmAssetImageDtl.ImageFilenameSys = _id + "_" + photo_Type + "_" + "0" + i;
                        }
                        else
                        {
                            _rmAssetImageDtl.ImageFilenameSys = _id + "_" + photo_Type + "_" + i;
                        }
                        _rmAssetImageDtl.ImageFilenameUpload = $"{subPath}/{fileRename}";


                        postedFile.CopyTo(stream);


                    }
                    uploadedFiles.Add(_rmAssetImageDtl);
                    if (uploadedFiles.Count() > 0)
                    {
                        await _formUCUAService.SaveImage(uploadedFiles);
                        successFullyUploaded = true;
                    }
                    else
                    {
                        successFullyUploaded = false;
                    }

                    j = j + 1;
                }

                return Json(successFullyUploaded);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<int> ImageUploadedTab(IFormCollection filesCollection, int headerId, string Id, string photoType)
        {
            IFormCollection files = Request.ReadFormAsync().Result;
            if (files != null && files.Count > 0)
            {
                List<FormUCUAImagesDTO> lstImages = new List<FormUCUAImagesDTO>();
                string photo_Type = Regex.Replace(photoType, @"[^a-zA-Z]", "");
                photoType = photoType.Replace(" ", "");

                var objExistsPhotoType = _formUCUAService.GetExitingPhotoType(headerId).Result;
                if (objExistsPhotoType == null) { objExistsPhotoType = new List<FormUCUAPhotoTypeDTO>(); }

                string InspRefNum = Regex.Replace(Id, @"[^0-9a-zA-Z]+", "");
                string wwwPath = this._webHostEnvironment.WebRootPath;

                foreach (var file in files.Files)
                {
                    var objSNo = objExistsPhotoType.Where(x => x.Type == photo_Type).FirstOrDefault();
                    if (objSNo == null) { objSNo = new FormUCUAPhotoTypeDTO() { SNO = 1, Type = photo_Type }; objExistsPhotoType.Add(objSNo); }
                    else { objSNo.SNO = objSNo.SNO + 1; }

                    string fileName = Path.GetFileName(file.FileName.Replace(" ", "_"));
                    string strFileUploadDir = Path.Combine("FormUCUA", InspRefNum, photoType);
                    string strSaveDir = Path.Combine(wwwPath, "Uploads", strFileUploadDir);
                    string strSysFileName = InspRefNum + "_" + photoType + "_" + objSNo.SNO.ToString("000");
                    string strUploadFileName = objSNo.SNO.ToString() + "_" + photoType + "_" + fileName;
                    if (!Directory.Exists(strSaveDir)) { Directory.CreateDirectory(strSaveDir); }
                    using (FileStream stream = new FileStream(Path.Combine(strSaveDir, strUploadFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    lstImages.Add(new FormUCUAImagesDTO()
                    {
                        ActiveYn = true,
                        CrBy = _security.UserID,
                        ModBy = _security.UserID,
                        CrDt = DateTime.UtcNow,
                        ModDt = DateTime.UtcNow,
                        RmmhPkRefNo = headerId,
                        ImageFilenameSys = strSysFileName,
                        ImageFilenameUpload = strUploadFileName,
                        ImageSrno = objSNo.SNO,
                        ImageTypeCode = photo_Type,
                        ImageUserFilePath = strFileUploadDir,
                        SubmitSts = true
                    });

                }
                if (lstImages.Count > 0)
                {
                    var a = await _formUCUAService.AddMultiImage(lstImages);
                }
            }
            else
            {
                return -1;
            }
            return 1;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUCUAWebImage(int pkId)
        {
            int response = await _formUCUAService.DeleteUCUAWebImage(pkId);
            return Json(response);
        }
        [HttpPost]
        public async Task<int> ImageUploadedTabWeb(IFormCollection filesCollection, int headerId, string Id, string photoType)
        {
            IFormCollection files = Request.ReadFormAsync().Result;
            if (files != null && files.Count > 0)
            {
                List<FormUCUAImagesDTO> lstImages = new List<FormUCUAImagesDTO>();
                string photo_Type = Regex.Replace(photoType, @"[^a-zA-Z]", "");
                photoType = photoType.Replace(" ", "");
                var objExistsPhotoType = _formUCUAService.GetExitingPhotoType(headerId).Result;
                if (objExistsPhotoType == null) { objExistsPhotoType = new List<FormUCUAPhotoTypeDTO>(); }

                string InspRefNum = Regex.Replace(Id, @"[^0-9a-zA-Z]+", "");
                string wwwPath = this._webHostEnvironment.WebRootPath;

                foreach (var file in files.Files)
                {
                    var objSNo = objExistsPhotoType.Where(x => x.Type == photo_Type).FirstOrDefault();
                    if (objSNo == null) { objSNo = new FormUCUAPhotoTypeDTO() { SNO = 1, Type = photo_Type }; objExistsPhotoType.Add(objSNo); }
                    else { objSNo.SNO = objSNo.SNO + 1; }

                    string fileName = Path.GetFileName(file.FileName.Replace(" ", "_"));
                    string strFileUploadDir = Path.Combine("FormUCUA", InspRefNum, photoType);
                    string strSaveDir = Path.Combine(wwwPath, "Uploads", strFileUploadDir);
                    string strSysFileName = InspRefNum + "_" + photoType + "_" + objSNo.SNO.ToString("000");
                    string strUploadFileName = objSNo.SNO.ToString() + "_" + photoType + "_" + fileName;
                    if (!Directory.Exists(strSaveDir)) { Directory.CreateDirectory(strSaveDir); }
                    using (FileStream stream = new FileStream(Path.Combine(strSaveDir, strUploadFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    lstImages.Add(new FormUCUAImagesDTO()
                    {
                        ActiveYn = true,
                        CrBy = _security.UserID,
                        ModBy = _security.UserID,
                        CrDt = DateTime.UtcNow,
                        ModDt = DateTime.UtcNow,
                        RmmhPkRefNo = headerId,
                        ImageFilenameSys = strSysFileName,
                        ImageFilenameUpload = strUploadFileName,
                        ImageSrno = objSNo.SNO,
                        ImageTypeCode = photo_Type,
                        ImageUserFilePath = strFileUploadDir,
                        SubmitSts = true
                    });

                }
                if (lstImages.Count > 0)
                {
                    var a = await _formUCUAService.AddMultiImageWeb(lstImages);
                }
            }
            else
            {
                return -1;
            }
            return 1;
        }

    }
}
