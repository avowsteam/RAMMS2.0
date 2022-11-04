using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAMMS.WebAPI.Controllers
{
    public class FormMController : Controller
    {

        private readonly IFormMService _formMService;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        public FormMController(IFormMService _formMService, ISecurity security, IDDLookUpService dDLookUpService)
        {
            this._formMService = _formMService;
            this._ddLookupService = dDLookUpService;
            this._security = security;
        }

        [Authorize]
        [Route("api/getMGridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormMGrid([FromBody] object landingGrid)
        {
            DataTableAjaxPostModel requestDtl = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(landingGrid.ToString());
            if (requestDtl.order != null && requestDtl.order.Count > 0)
            {
                requestDtl.order = requestDtl.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            var respons = await _formMService.GetHeaderGrid(requestDtl);
            return RAMMSApiSuccessResponse(respons);
        }

        [Authorize]
        [Route("api/deleteM")]
        [HttpPost]
        public IActionResult DeActivateM(int id)
        {
            return RAMMSApiSuccessResponse(_formMService.Delete(id));            
        }

        [Authorize]
        [Route("api/findDetailsM")]
        [HttpPost]
        public async Task<IActionResult> FindDetailsM([FromBody] object frmM)
        {
            FormMDTO requestDtl = JsonConvert.DeserializeObject<FormMDTO>(frmM.ToString());
            return RAMMSApiSuccessResponse(await _formMService.FindDetails(requestDtl, _security.UserID));
        }

        [Authorize]
        [Route("api/updateM")]
        [HttpPost]
        public async Task<IActionResult> UpdateM([FromBody] object frmM)
        {
            FormMDTO requestDtl = JsonConvert.DeserializeObject<FormMDTO>(frmM.ToString());
            requestDtl.ModBy = _security.UserID;
            requestDtl.ModDt = DateTime.UtcNow;
            var result = await _formMService.Save(requestDtl, requestDtl.SubmitSts);
            return RAMMSApiSuccessResponse(result.PkRefNo);
        }


        [Authorize]
        [Route("api/getMById")]
        [HttpPost]
        public async Task<IActionResult> GetMById(int id)
        {
            return RAMMSApiSuccessResponse(await _formMService.FindByHeaderID(id));
        }

        //[Authorize]
        //[Route("api/getMlist")]
        //[HttpPost]
        //public async Task<IActionResult> GetM([FromBody] object request)
        //{
        //    AssetDDLRequestDTO requestDtl = JsonConvert.DeserializeObject<AssetDDLRequestDTO>(request.ToString());
        //    IEnumerable<SelectListItem> listItems = await _formMService.GetCVIds(requestDtl);
        //    return RAMMSApiSuccessResponse(listItems);
        //}

        //[Authorize]
        //[Route("api/getMImageList")]
        //[HttpPost]
        //public IActionResult GetMImage(int id)
        //{
        //    List<FormRImagesDTO > listItems = _formMService.ImageList(id);
        //    return RAMMSApiSuccessResponse(listItems);
        //}

        //[Authorize]
        //[Route("api/deleteMImage")]
        //[HttpPost]
        //public async Task<IActionResult> DeleteMImage(int headerid, int imgId)
        //{
        //    int response = await _formMService.DeleteImage(headerid, imgId);
        //    return RAMMSApiSuccessResponse(response);
        //}

        
    }
}
