using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAMMS.WebAPI.Controllers
{
    public class FormUCUAController : Controller
    {

        private readonly IFormUCUAService  _formUCUAService;
        private readonly IDDLookUpService _ddLookupService;
        private readonly ISecurity _security;
        public FormUCUAController(IFormUCUAService _formUCUAService, ISecurity security, IDDLookUpService dDLookUpService)
        {
            this._formUCUAService = _formUCUAService;
            this._ddLookupService = dDLookUpService;
            this._security = security;
        }

        [Authorize]
        [Route("api/getUCUAGridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormUCUAGrid([FromBody] object landingGrid)
        {

            FilteredPagingDefinition<FormUCUASearchGridDTO> requestDtl = JsonConvert.DeserializeObject<FilteredPagingDefinition<FormUCUASearchGridDTO>>(landingGrid.ToString());

            PagingResult<FormUCUAHeaderRequestDTO> response = await _formUCUAService.GetHeaderList(requestDtl);
            return RAMMSApiSuccessResponse(response);
        }

        [Authorize]
        [Route("api/deleteUCUA")]
        [HttpPost]
        public IActionResult DeActivateUCUA(int id)
        {
            return RAMMSApiSuccessResponse(_formUCUAService.DeleteFormUCUA(id));            
        }

        [Authorize]
        [Route("api/updateUCUA")]
        [HttpPost]
        public async Task<IActionResult> UpdateUCUA([FromBody] object frmUCUA)
        {
            FormUCUAResponseDTO requestDtl = JsonConvert.DeserializeObject<FormUCUAResponseDTO>(frmUCUA.ToString());
           
            var result = await _formUCUAService.SaveFormUCUA(requestDtl);
            return RAMMSApiSuccessResponse(result.PkRefNo);
        }

        [Authorize]
        [Route("api/getUCUAById")]
        [HttpPost]
        public async Task<IActionResult> GetUCUAById(int id)
        {
            return RAMMSApiSuccessResponse(await _formUCUAService.GetHeaderById(id));
        }

        
    }
}
