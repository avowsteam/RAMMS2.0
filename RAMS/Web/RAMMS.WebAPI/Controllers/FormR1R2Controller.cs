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
    public class FormR1R2Controller : Controller
    {

        private readonly IFormR1R2Service _formR1R2Service;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        public FormR1R2Controller(IFormR1R2Service _formR1R2Service, ISecurity security, IDDLookUpService dDLookUpService)
        {
            this._formR1R2Service = _formR1R2Service;
            this._ddLookupService = dDLookUpService;
            this._security = security;
        }

        [Authorize]
        [Route("api/getR1R2GridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormR1R2Grid([FromBody] object landingGrid)
        {
            DataTableAjaxPostModel requestDtl = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(landingGrid.ToString());
            if (requestDtl.order != null && requestDtl.order.Count > 0)
            {
                requestDtl.order = requestDtl.order.Select(x => { if (x.column == 4 || x.column == 1 || x.column == 9) { x.column = 16; } return x; }).ToList();
            }
            var respons = await _formR1R2Service.GetHeaderGrid(requestDtl);
            return RAMMSApiSuccessResponse(respons);
        }

        [Authorize]
        [Route("api/deleteR1R2")]
        [HttpPost]
        public IActionResult DeActivateR1R2(int id)
        {
            return RAMMSApiSuccessResponse(_formR1R2Service.Delete(id));            
        }

        [Authorize]
        [Route("api/findDetailsR1R2")]
        [HttpPost]
        public async Task<IActionResult> FindDetailsR1R2([FromBody] object frmR1R2)
        {
            FormR1DTO requestDtl = JsonConvert.DeserializeObject<FormR1DTO>(frmR1R2.ToString());
            return RAMMSApiSuccessResponse(await _formR1R2Service.FindDetails(requestDtl, _security.UserID));
        }

        [Authorize]
        [Route("api/updateR1R2")]
        [HttpPost]
        public async Task<IActionResult> UpdateR1R2([FromBody] object frmR1R2)
        {
            FormR1DTO requestDtl = JsonConvert.DeserializeObject<FormR1DTO>(frmR1R2.ToString());
            requestDtl.ModBy = _security.UserID;
            requestDtl.ModDt = DateTime.UtcNow;
            var result = await _formR1R2Service.Save(requestDtl, requestDtl.SubmitSts);
            return RAMMSApiSuccessResponse(result.PkRefNo);
        }

        [Authorize]
        [Route("api/getR1R2ById")]
        [HttpPost]
        public async Task<IActionResult> GetR1R2ById(int id)
        {
            return RAMMSApiSuccessResponse(await _formR1R2Service.FindByHeaderID(id));
        }

        [Authorize]
        [Route("api/getR1R2list")]
        [HttpPost]
        public async Task<IActionResult> GetR1R2([FromBody] object request)
        {
            AssetDDLRequestDTO requestDtl = JsonConvert.DeserializeObject<AssetDDLRequestDTO>(request.ToString());
            IEnumerable<SelectListItem> listItems = await _formR1R2Service.GetCVIds(requestDtl);
            return RAMMSApiSuccessResponse(listItems);
        }

        [Authorize]
        [Route("api/getR1R2ImageList")]
        [HttpPost]
        public IActionResult GetR1R2Image(int id)
        {
            List<FormRImagesDTO > listItems = _formR1R2Service.ImageList(id);
            return RAMMSApiSuccessResponse(listItems);
        }

        [Authorize]
        [Route("api/deleteR1R2Image")]
        [HttpPost]
        public async Task<IActionResult> DeleteR1R2Image(int headerid, int imgId)
        {
            int response = await _formR1R2Service.DeleteImage(headerid, imgId);
            return RAMMSApiSuccessResponse(response);
        }

        
    }
}
