using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Common;
using RAMMS.Common.RefNumber;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormUCUAService: IFormUCUAService
    {
        private readonly IFormUCUARepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;
        private IWebHostEnvironment Environment;
        public FormUCUAService(IRepositoryUnit repoUnit, IFormUCUARepository repo, IMapper mapper, ISecurity security, IProcessService process,
            IWebHostEnvironment _environment)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
            this.Environment = _environment;
        }

        public async Task<FormUCUAResponseDTO> GetHeaderById(int id)
        {
            var header = await _repoUnit.FormucuaRepository.FindAsync(s => s.RmmhPkRefNo == id);
            if (header == null)
            {
                return null;
            }
            return _mapper.Map<FormUCUAResponseDTO>(header);
        }
        
        public async Task<FormUCUAResponseDTO> SaveFormUCUA(FormUCUAResponseDTO FormUCUA)
        {
            try
            {
                var domainModelFormUCUA = _mapper.Map<RmUcua>(FormUCUA);
                domainModelFormUCUA.RmmhPkRefNo = 0;

                var obj = _repoUnit.FormucuaRepository.FindAsync(x => x.RmmhPkRefNo == domainModelFormUCUA.RmmhPkRefNo && x.RmmhRefId == domainModelFormUCUA.RmmhRefId && x.RmmhDateReceived == domainModelFormUCUA.RmmhDateReceived && x.RmmhActiveYn == true).Result;
                //var obj = _repoUnit.FormucuaRepository.FindAsync(x => x.RmmhRefId == domainModelFormUCUA.RmmhRefId && x.RmmhActiveYn == true).Result;
                var MaxPkrefNo = _repoUnit.FormucuaRepository._context.RmUcua.Select(x => x.RmmhPkRefNo).ToList();
                int LatestPKNo = 0;


                if (obj != null)
                {
                    var res = _mapper.Map<FormUCUAResponseDTO>(obj);
                    res.FormExist = true;
                    return res;
                }

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                if (MaxPkrefNo.Count != 0)
                {
                     LatestPKNo = MaxPkrefNo.Max();
                    LatestPKNo = LatestPKNo + 1;
                }
                else
                {
                    LatestPKNo = LatestPKNo + 1;
                }
                lstData.Add("RefNo", LatestPKNo.ToString());
                lstData.Add("YYYYMMDD", Utility.ToString(Convert.ToDateTime(FormUCUA.DateReceived).ToString("yyyyMMdd")));
                domainModelFormUCUA.RmmhRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormUCUA, lstData);
                domainModelFormUCUA.RmmhStatus = "Initialize";
                domainModelFormUCUA.RmmhUnsafeAct = FormUCUA.hdnUnsafeAct;
                domainModelFormUCUA.RmmhUnsafeCondition = FormUCUA.hdnUnsafeCondition;

                var entity = _repoUnit.FormucuaRepository.CreateReturnEntity(domainModelFormUCUA);
                FormUCUA.PkRefNo = _mapper.Map<FormUCUAResponseDTO>(entity).PkRefNo;
                FormUCUA.RefId = domainModelFormUCUA.RmmhRefId;
                FormUCUA.Status = domainModelFormUCUA.RmmhStatus;

                return FormUCUA;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> Update(FormUCUAResponseDTO FormUCUA)
        {
            int rowsAffected;
            
            try
            {
                int PkRefNo = FormUCUA.PkRefNo;
                int? Fw1PkRefNo = FormUCUA.PkRefNo;

                var domainModelformUcua = _mapper.Map<RmUcua>(FormUCUA);
                domainModelformUcua.RmmhPkRefNo = PkRefNo;

                domainModelformUcua.RmmhActiveYn = true;
                domainModelformUcua.RmmhUnsafeAct = FormUCUA.hdnUnsafeAct;
                domainModelformUcua.RmmhUnsafeCondition = FormUCUA.hdnUnsafeCondition;
                domainModelformUcua.RmmhActionTakenBy = FormUCUA.hdnActionTakenBy;
                domainModelformUcua.RmmhEffectivenessActionTakenBy = FormUCUA.hdnEffectivenessActionTakenBy;
                domainModelformUcua = UpdateStatus(domainModelformUcua);
                _repoUnit.FormucuaRepository.Update(domainModelformUcua);
                rowsAffected = await _repoUnit.CommitAsync();

            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }
        public RmUcua UpdateStatus(RmUcua form)
        {
            if (form.RmmhPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormucuaRepository._context.RmUcua.Where(x => x.RmmhPkRefNo == form.RmmhPkRefNo).Select(x => new { Status = x.RmmhStatus, Log = x.RmmhAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.RmmhAuditLog = existsObj.Log;
                    form.RmmhStatus = existsObj.Status;

                }

            }
            if (form.RmmhSubmitYn && (form.RmmhStatus == "Saved" || form.RmmhStatus == "Initialize"))
            {
                form.RmmhStatus = Common.StatusList.FormUcuaSubmitted;
                form.RmmhAuditLog = Utility.ProcessLog(form.RmmhAuditLog, "Submitted By", "Submitted", string.Empty, string.Empty, form.RmmhDateReceived, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Recorded By:" + form.RmmhReportingName + " - Form Ucua (" + form.RmmhPkRefNo + ")",
                 //   RmNotMessage = "Recorded By:" + " - Form Ucua (" + form.RmmhPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/InstructedWorks/EditFormT?id=" + form.RmmhPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }
        public async Task<byte[]> FormDownload(string formname, int id, string filepath)
        {
            string Oldfilename = "";
            string filename = "";
            string cachefile = "";
            if (!filepath.Contains(".xlsx"))
            {
                Oldfilename = filepath + filename + ".xlsx";
                filename = formname + DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString();
                cachefile = filepath + filename + ".xlsx";
            }
            else
            {
                Oldfilename = filepath;
                filename = filepath.Replace(".xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfffffff").ToString() + ".xlsx");
                cachefile = filename;
            }

            try
            {
                FormUCUARpt rpt = await this.GetReportData(id);
                System.IO.File.Copy(Oldfilename, cachefile, true);
                string wwwPath = this.Environment.WebRootPath;
                string trueImage = wwwPath + "/Images/True.png";
                string FalseImage = wwwPath + "/Images/False.png";

                using (var workbook = new XLWorkbook(cachefile))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    IXLWorksheet image = workbook.Worksheet(1);
                    byte[] buffTrue = File.ReadAllBytes($"{trueImage}"); 
                    System.IO.MemoryStream strTrue = new System.IO.MemoryStream(buffTrue);

                    byte[] buffFalse = File.ReadAllBytes($"{FalseImage}");
                    System.IO.MemoryStream strFalse = new System.IO.MemoryStream(buffFalse);
                    //image.AddPicture(strTrue).MoveTo(image.Cell(11, 2)).WithSize(360, 170);
                    if ( rpt.UnsafeAct == true)
                    {
                        image.AddPicture(strTrue).MoveTo(image.Cell(11, 2)).WithSize(40, 40);
                    }
                    else
                    {
                        image.AddPicture(strFalse).MoveTo(image.Cell(11, 2)).WithSize(40, 40);
                    }
                    if (rpt.UnsafeCondition == true)
                    {
                        image.AddPicture(strTrue).MoveTo(image.Cell(11, 9)).WithSize(40, 40);
                    }
                    else
                    {
                        image.AddPicture(strFalse).MoveTo(image.Cell(11, 9)).WithSize(40, 40);
                    }


                    if (worksheet != null)
                    {
                        worksheet.Cell(2, 6).Value = rpt.ReportingName;
                        worksheet.Cell(5, 6).Value = rpt.Location;
                        worksheet.Cell(7, 6).Value = rpt.WorkScope;

                       // worksheet.Cell(11, 2).Value = rpt.UnsafeAct ;
                        worksheet.Cell(12, 2).Value = rpt.UnsafeActDescription;
                       // worksheet.Cell(11, 9).Value = rpt.UnsafeCondition;
                        worksheet.Cell(12, 9).Value = rpt.UnsafeConditionDescription;
                        worksheet.Cell(15, 2).Value = rpt.ImprovementRecommendation;
                        worksheet.Cell(19, 5).Value = rpt.DateReceived.HasValue ? rpt.DateReceived.Value.ToString("dd-MM-yyyy") : "";

                        worksheet.Cell(19, 14).Value = rpt.DateCommitteeReview.HasValue ? rpt.DateCommitteeReview.Value.ToString("dd-MM-yyyy") : "";
                        worksheet.Cell(22, 2).Value = rpt.CommentsOfficeUse;
                        worksheet.Cell(23, 2).Value = rpt.HseSection;
                        worksheet.Cell(23, 8).Value = rpt.SafteyCommitteeChairman;
                        worksheet.Cell(23, 12).Value = rpt.ImsRep;
                        worksheet.Cell(27, 6).Value = rpt.DateActionTaken.HasValue ? rpt.DateActionTaken.Value.ToString("dd-MM-yyyy") : "";

                        worksheet.Cell(27, 12).Value = rpt.ActionTakenBy;

                        worksheet.Cell(28, 2).Value = rpt.ActionDescription;

                        worksheet.Cell(31, 4).Value = rpt.DateEffectivenessActionTaken.HasValue ? rpt.DateEffectivenessActionTaken.Value.ToString("dd-MM-yyyy") : "";
                        worksheet.Cell(31, 12).Value = rpt.EffectivenessActionTakenBy;
                        worksheet.Cell(32, 2).Value = rpt.EffectivenessActionDescription;


                        //worksheet.Cell(67, 33).Value = rpt.Status;
                        //worksheet.Cell(68, 33).Value = rpt.ActiveYn;
                        //worksheet.Cell(69, 33).Value = rpt.SubmitYn;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        System.IO.File.Delete(cachefile);
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Copy(Oldfilename, cachefile, true);
                using (var workbook = new XLWorkbook(cachefile))
                {
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        System.IO.File.Delete(cachefile);
                        return content;
                    }
                }

            }
        }
        public async Task<FormUCUARpt> GetReportData(int headerid)
        {
            return await _repo.GetReportData(headerid);
        }
        //public async Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions)
        //{
        //    PagingResult<FormUCUAHeaderRequestDTO> result = new PagingResult<FormUCUAHeaderRequestDTO>();
        //    List<FormUCUAHeaderRequestDTO> formAlist = new List<FormUCUAHeaderRequestDTO>();
        //    result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
        //    result.TotalRecords = result.PageResult.Count();
        //    result.PageNo = filterOptions.StartPageNo;
        //    result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
        //    return result;
        //}
        public async Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions)
        {
            PagingResult<FormUCUAHeaderRequestDTO> result = new PagingResult<FormUCUAHeaderRequestDTO>();
            List<FormUCUAHeaderRequestDTO> formAlist = new List<FormUCUAHeaderRequestDTO>();
            result.PageResult = await _repo.GetFilteredRecordList(filterOptions);
            result.TotalRecords = result.PageResult.Count();
            result.PageNo = filterOptions.StartPageNo;
            result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            return result;
        }
        public async Task<int> DeActivateFormT(int formNo)
        {
            int rowsAffected;
            try
            {
                var domainModelFormT = await _repoUnit.FormTRepository.GetByIdAsync(formNo);
                domainModelFormT.FmtActiveYn = false;
                _repoUnit.FormTRepository.Update(domainModelFormT);
                rowsAffected = await _repoUnit.CommitAsync();
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }
        public int? DeleteFormUCUA(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormUCUA(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }

        public async Task<int> LastInsertedIMAGENO(string hederId, string type)
        {
            int imageCt = await _repoUnit.FormW1Repository.GetImageId(hederId, type);
            return imageCt;
        }

        public async Task<int> SaveImage(List<FormUCUAImageResponseDTO> image)
        {
            int rowsAffected;
            try
            {
                var domainModelFormW1 = new List<RmIwformImage>();

                foreach (var list in image)
                {
                    domainModelFormW1.Add(_mapper.Map<RmIwformImage>(list));
                }
                _repoUnit.FormW1Repository.SaveImage(domainModelFormW1);
                rowsAffected = await _repoUnit.CommitAsync();

            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }

        

        public async Task<List<FormUCUAPhotoTypeDTO>> GetExitingPhotoType(int headerId)
        {
            return await _repo.GetExitingPhotoType(headerId);
        }
        //public async Task<RmIwformImage> AddImage(FormUCUAImagesDTO imageDTO)
        //{
        //    RmIwformImage image = _mapper.Map<RmIwformImage>(imageDTO);
        //    return await _repo.AddImage(image);
        //}
        //public async Task<(IList<RmIwformImage>, int)> AddMultiImage(IList<FormUCUAImagesDTO> imagesDTO)
        //{
        //    IList<RmIwformImage> images = new List<RmIwformImage>();
        //    foreach (var img in imagesDTO)
        //    {
        //        var count = await _repo.ImageCount(img.ImageTypeCode, img.hPkRefNo.Value);
        //        if (count > 2)
        //        {
        //            return (null, -1);
        //        }

        //        images.Add(_mapper.Map<RmIwformImage>(img));
        //    }
        //    return (await _repo.AddMultiImage(images), 1);
        //}

        //public async Task<List<RmUcuaImage>> AddMultiImageTab(List<FormUCUAImagesDTO> imagesDTO)
        //{
        //    List<RmUcuaImage> images = new List<RmUcuaImage>();
        //    foreach (var img in imagesDTO)
        //    {
        //        images.Add(_mapper.Map<RmUcuaImage>(img));
        //    }
        //    return await _repo.AddMultiImage(images);
        //}

        public async Task<(IList<RmUcuaImage>, int)> AddMultiImage(IList<FormUCUAImagesDTO> imagesDTO)
        {
            IList<RmUcuaImage> images = new List<RmUcuaImage>();
            foreach (var img in imagesDTO)
            {
                var count = await _repo.ImageCount(img.ImageTypeCode, img.RmmhPkRefNo.Value);
                if (count > 2)
                {
                    return (null, -1);
                }
                var imgs = _mapper.Map<RmUcuaImage>(img);
                imgs.UcuaPkRefNo = img.PkRefNo;
                imgs.UcuaRmmhPkRefNo = img.RmmhPkRefNo;
                images.Add(imgs);
            }
            return (await _repo.AddMultiImage(images), 1);
        }
        public List<FormUCUAImagesDTO> ImageList(int headerId)
        {
            List<RmUcuaImage> lstImages = _repo.ImageList(headerId).Result;
            List<FormUCUAImagesDTO> lstResult = new List<FormUCUAImagesDTO>();
            if (lstImages != null && lstImages.Count > 0)
            {
                lstImages.ForEach((RmUcuaImage img) =>
                {
                    lstResult.Add(_mapper.Map<FormUCUAImagesDTO>(img));
                });
            }
            return lstResult;
        }
        public async Task<int> DeleteImage(int headerId, int imgId)
        {
            RmUcuaImage img = new RmUcuaImage();
            img.UcuaPkRefNo = imgId;
            img.UcuaRmmhPkRefNo = headerId;
            img.UcuaActiveYn = false;
            return await _repo.DeleteImage(img);
        }
    }

}
