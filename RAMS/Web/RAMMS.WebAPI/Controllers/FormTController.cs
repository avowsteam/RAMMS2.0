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
    public class FormTController : Controller
    {

        private readonly IFormTService _FormTService;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        public FormTController(IFormTService _FormTService, ISecurity security, IDDLookUpService dDLookUpService)
        {
            this._FormTService = _FormTService;
            this._ddLookupService = dDLookUpService;
            this._security = security;
        }

        [Authorize]
        [Route("api/getTGridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormTGrid([FromBody] object landingGrid)
        {
            DataTableAjaxPostModel requestDtl = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(landingGrid.ToString());
            if (requestDtl.order != null && requestDtl.order.Count > 0)
            {
                requestDtl.order = requestDtl.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            var respons = await _FormTService.GetHeaderList(requestDtl);
            return RAMMSApiSuccessResponse(respons);
        }

        [Authorize]
        [Route("api/deleteT")]
        [HttpPost]
        public IActionResult DeActivateM(int id)
        {
            return RAMMSApiSuccessResponse(_FormTService.DeActivateFormT(id));            
        }

        [Authorize]
        [Route("api/findDetailsT")]
        [HttpPost]
        public async Task<IActionResult> FindDetailsM([FromBody] object frmM)
        {
            FormTResponseDTO requestDtl = JsonConvert.DeserializeObject<FormTResponseDTO>(frmM.ToString());
            return RAMMSApiSuccessResponse(await _FormTService.FindDetails(requestDtl, _security.UserID));
        }

        [Authorize]
        [Route("api/updateT")]
        [HttpPost]
        public async Task<IActionResult> UpdateM([FromBody] object frmM)
        {
            FormTResponseDTO  requestDtl = JsonConvert.DeserializeObject<FormTResponseDTO>(frmM.ToString());
            requestDtl.ModBy = _security.UserID;
            requestDtl.ModDt = DateTime.UtcNow;
            var result = await _FormTService.Save(requestDtl, requestDtl.SubmitSts);
            return RAMMSApiSuccessResponse(result.PkRefNo);
        }


        [Authorize]
        [Route("api/getMById")]
        [HttpPost]
        public async Task<IActionResult> GetMById(int id)
        {
            return RAMMSApiSuccessResponse(await _FormTService.FindByHeaderID(id));
        }

        //[Authorize]
        //[Route("api/getMlist")]
        //[HttpPost]
        //public async Task<IActionResult> GetM([FromBody] object request)
        //{
        //    AssetDDLRequestDTO requestDtl = JsonConvert.DeserializeObject<AssetDDLRequestDTO>(request.ToString());
        //    IEnumerable<SelectListItem> listItems = await _FormTService.GetCVIds(requestDtl);
        //    return RAMMSApiSuccessResponse(listItems);
        //}

        //[Authorize]
        //[Route("api/getMImageList")]
        //[HttpPost]
        //public IActionResult GetMImage(int id)
        //{
        //    List<FormRImagesDTO > listItems = _FormTService.ImageList(id);
        //    return RAMMSApiSuccessResponse(listItems);
        //}

        //[Authorize]
        //[Route("api/deleteMImage")]
        //[HttpPost]
        //public async Task<IActionResult> DeleteMImage(int headerid, int imgId)
        //{
        //    int response = await _FormTService.DeleteImage(headerid, imgId);
        //    return RAMMSApiSuccessResponse(response);
        //}

        
    }
}
