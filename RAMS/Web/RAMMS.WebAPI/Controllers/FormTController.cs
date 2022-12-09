using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Business.ServiceProvider.Services;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.SearchBO;
using RAMMS.DTO.Wrappers;
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
        [Route("api/getFormTGridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormTGrid([FromBody] object landingGrid)
        {

            FilteredPagingDefinition<FormTSearchGridDTO> requestDtl = JsonConvert.DeserializeObject<FilteredPagingDefinition<FormTSearchGridDTO>>(landingGrid.ToString());

            PagingResult<FormTHeaderRequestDTO> response = await _FormTService.GetHeaderList(requestDtl);
            return RAMMSApiSuccessResponse(response);
        }

        [Authorize]
        [Route("api/getFormTDetailsGridData")]
        [HttpPost]
        public async Task<IActionResult> GetFilteredFormTDetailGrid([FromBody] object landingGrid)
        {

            FilteredPagingDefinition<FormTDtlResponseDTO> requestDtl = JsonConvert.DeserializeObject<FilteredPagingDefinition<FormTDtlResponseDTO>>(landingGrid.ToString());

            PagingResult<FormTDtlGridDTO> response = await _FormTService.GetDetailList(requestDtl);
            return RAMMSApiSuccessResponse(response);
        }

        [Authorize]
        [Route("api/deleteFormT")]
        [HttpPost]
        public IActionResult DeActivateM(int id)
        {
            return RAMMSApiSuccessResponse(_FormTService.DeActivateFormT(id));            
        }

        [Authorize]
        [Route("api/deleteFormTDetail")]
        [HttpPost]
        public IActionResult DeActivateFormTDtl(int id)
        {
            return RAMMSApiSuccessResponse(_FormTService.DeleteFormTDtl(id));
        }


        [Authorize]
        [Route("api/findDetailsFormT")]
        [HttpPost]
        public async Task<IActionResult> FindDetailsT([FromBody] object frmT)
        {
            FormTResponseDTO requestDtl = JsonConvert.DeserializeObject<FormTResponseDTO>(frmT.ToString());
            return RAMMSApiSuccessResponse(await _FormTService.SaveFormT(requestDtl));
        }

        [Authorize]
        [Route("api/updateFormT")]
        [HttpPost]
        public async Task<IActionResult> UpdateFormT([FromBody] object frmM)
        {
            FormTResponseDTO  requestDtl = JsonConvert.DeserializeObject<FormTResponseDTO>(frmM.ToString());
            requestDtl.ModBy = _security.UserID;
            requestDtl.ModDt = DateTime.UtcNow;
            var result = await _FormTService.Update(requestDtl);
            return RAMMSApiSuccessResponse(requestDtl.PkRefNo);
        }


        [Authorize]
        [Route("api/saveFormTdetails")]
        [HttpPost]
        public async Task<IActionResult> SaveFormTDetail([FromBody] object frmTdtl)
        {
            FormTDtlResponseDTO requestDtl = JsonConvert.DeserializeObject<FormTDtlResponseDTO>(frmTdtl.ToString());
 
            var result =  _FormTService.SaveFormTDtl(requestDtl);
            return RAMMSApiSuccessResponse(result);
        }

        [Authorize]
        [Route("api/updateFormTdetails")]
        [HttpPost]
        public async Task<IActionResult> UpdateFormTDtl([FromBody] object frmTdtl)
        {
            FormTDtlResponseDTO requestDtl = JsonConvert.DeserializeObject<FormTDtlResponseDTO>(frmTdtl.ToString());

            var result = _FormTService.UpdateFormTDtl(requestDtl);
            return RAMMSApiSuccessResponse(requestDtl.PkRefNo);
        }


        [Authorize]
        [Route("api/getFormTbyId")]
        [HttpPost]
        public async Task<IActionResult> GetFormTById(int id)
        {
            return RAMMSApiSuccessResponse(await _FormTService.GetHeaderById(id));
        }

        [Authorize]
        [Route("api/getFormTdetailsById")]
        [HttpPost]
        public async Task<IActionResult> GetFormTDetailsById(int id)
        {
            return RAMMSApiSuccessResponse(await _FormTService.GetFormTDtlById(id));
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
