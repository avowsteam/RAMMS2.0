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
    public class FormG1G2Controller : Controller
    {

        private readonly IFormG1G2Service _formG1G2Service;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        public FormG1G2Controller(IFormG1G2Service _formG1G2Service, ISecurity security, IDDLookUpService dDLookUpService)
        {
            this._formG1G2Service = _formG1G2Service;
            this._ddLookupService = dDLookUpService;
            this._security = security;
        }

        [Authorize]
        [Route("api/getG1G2GridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormG1G2Grid([FromBody] object landingGrid)
        {
            DataTableAjaxPostModel requestDtl = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(landingGrid.ToString());
            if (requestDtl.order != null && requestDtl.order.Count > 0)
            {
                requestDtl.order = requestDtl.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            var respons = await _formG1G2Service.GetHeaderGrid(requestDtl);
            return RAMMSApiSuccessResponse(respons);
        }

        [Authorize]
        [Route("api/deleteG1G2")]
        [HttpPost]
        public IActionResult DeActivateG1G2(int id)
        {
            return RAMMSApiSuccessResponse(_formG1G2Service.Delete(id));            
        }

        [Authorize]
        [Route("api/findDetailsG1G2")]
        [HttpPost]
        public async Task<IActionResult> FindDetailsG1G2([FromBody] object frmG1G2)
        {
            FormG1DTO requestDtl = JsonConvert.DeserializeObject<FormG1DTO>(frmG1G2.ToString());
            return RAMMSApiSuccessResponse(await _formG1G2Service.FindDetails(requestDtl, _security.UserID));
        }

        [Authorize]
        [Route("api/updateG1G2")]
        [HttpPost]
        public async Task<IActionResult> UpdateG1G2([FromBody] object frmG1G2)
        {
            FormG1DTO requestDtl = JsonConvert.DeserializeObject<FormG1DTO>(frmG1G2.ToString());
            requestDtl.ModBy = _security.UserID;
            requestDtl.ModDt = DateTime.UtcNow;
            var result = await _formG1G2Service.Save(requestDtl, requestDtl.SubmitSts);
            return RAMMSApiSuccessResponse(result.PkRefNo);
        }

        [Authorize]
        [Route("api/getG1G2ById")]
        [HttpPost]
        public async Task<IActionResult> GetG1G2ById(int id)
        {
            return RAMMSApiSuccessResponse(await _formG1G2Service.FindByHeaderID(id));
        }

        //[Authorize]
        //[Route("api/getG1G2List")]
        //[HttpPost]
        //public async Task<IActionResult> GetG1G2([FromBody] object request)
        //{
        //    AssetDDLRequestDTO requestDtl = JsonConvert.DeserializeObject<AssetDDLRequestDTO>(request.ToString());
        //    IEnumerable<SelectListItem> listItems = await _formG1G2Service.GetCVIds(requestDtl);
        //    return RAMMSApiSuccessResponse(listItems);
        //}

        [Authorize]
        [Route("api/getG1G2ImageList")]
        [HttpPost]
        public IActionResult GetG1G2Image(int id)
        {
            List<FormGImagesDTO > listItems = _formG1G2Service.ImageList(id);
            return RAMMSApiSuccessResponse(listItems);
        }

        [Authorize]
        [Route("api/deleteG1G2Image")]
        [HttpPost]
        public async Task<IActionResult> DeleteG1G2Image(int headerid, int imgId)
        {
            int response = await _formG1G2Service.DeleteImage(headerid, imgId);
            return RAMMSApiSuccessResponse(response);
        }
    }
}
