using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.Repository.Interfaces;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class FormMapService : IFormMapService
    {
        private readonly IFormMapRepository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly IAssetsService _assetsService;
        private readonly IProcessService processService;
        private readonly ISecurity _security;

        public FormMapService(IRepositoryUnit repoUnit, IFormMapRepository repo,
            IAssetsService assetsService, IMapper mapper, IProcessService proService,
            ISecurity security)
        {
            _repo = repo;
            _mapper = mapper;
            _assetsService = assetsService;
            _repoUnit = repoUnit;
            processService = proService;
            _security = security;
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            return await _repo.GetHeaderGrid(searchData);
        }

        public async Task<FormMapHeaderDTO> GetHeaderById(int id, bool view)
        {
            RmMapHeader res = _repo.GetHeaderById(id, view);
            FormMapHeaderDTO FormMap = new FormMapHeaderDTO();
            FormMap = _mapper.Map<FormMapHeaderDTO>(res);
            FormMap.RmMapDetails = _mapper.Map<List<FormMapDetailsDTO>>(res.RmMapDetails);
            return FormMap;
        }

        public int Delete(int id)
        {
            ///if (id > 0 && !_repo.isF1Exist(id))
            if (id > 0)
            {
                id = _repo.DeleteHeader(new RmMapHeader() { RmmhActiveYn = false, RmmhPkRefNo = id });
            }
            else
            {
                return -1;
            }
            return id;
        }

        public async Task<FormMapHeaderDTO> FindDetails(FormMapHeaderDTO frmMap, int createdBy)
        {
            RmMapHeader header = _mapper.Map<RmMapHeader>(frmMap);
            header = await _repo.FindDetails(header);
            if (header != null)
            {
                frmMap = _mapper.Map<FormMapHeaderDTO>(header);
            }
            else
            {
                List<string> lstCVUNChar = Utility.GetAlphabets(1);
                frmMap.Status = "Initialize";

                //frmR1R2.InspectedBy = _security.UserID;
                //frmR1R2.InspectedName = _security.UserName;
                //frmR1R2.InspectedDt = DateTime.Today;
                frmMap.CrBy = frmMap.ModBy = createdBy;
                frmMap.CrDt = frmMap.ModDt = DateTime.UtcNow;

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                if (frmMap.RmuCode.ToUpper() == "MRI")
                    lstData.Add("RMU", "Miri");
                else if (frmMap.RmuCode.ToUpper() == "BTN")
                    lstData.Add("RMU", "Batu Niah");
                else
                    lstData.Add("RMU", frmMap.RmuCode.ToString());
                lstData.Add("YYYY", frmMap.Year.ToString());
                frmMap.RefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormMap, lstData);

                header = _mapper.Map<RmMapHeader>(frmMap);
                header = await _repo.Save(header, false);
                frmMap = _mapper.Map<FormMapHeaderDTO>(header);
            }
            return frmMap;
        }

        public async Task<List<FormDHeaderResponseDTO>> GetForDDetails(string RMU, int Year, int Month)
        {
            List<RmFormDHdr> res = _repo.GetForDDetails(RMU, Year, Month);
            List<FormDHeaderResponseDTO> FormD = new List<FormDHeaderResponseDTO>();
            FormD = _mapper.Map<List<FormDHeaderResponseDTO>>(res);
            return FormD;
        }

        public async Task<List<FormMapDetailsDTO>> GetForMapDetails(int ID)
        {
            List<RmMapDetails> res = _repo.GetForMapDetails(ID);
            List<FormMapDetailsDTO> FormD = new List<FormMapDetailsDTO>();
            FormD = _mapper.Map<List<FormMapDetailsDTO>>(res);
            return FormD;
        }

        public async Task<FormMapHeaderDTO> SaveMap(FormMapHeaderDTO frmmaphdr, List<FormMapDetailsDTO> frmmap, bool updateSubmit)
        {
            RmMapHeader frmb14hdr_1 = this._mapper.Map<RmMapHeader>((object)frmmaphdr);
            frmb14hdr_1 = UpdateStatus(frmb14hdr_1);

            RmMapHeader source = await this._repo.Save(frmb14hdr_1, updateSubmit);

            var domainModelFormB14 = _mapper.Map<List<RmMapDetails>>(frmmap);
            foreach (var list in domainModelFormB14)
            {
                list.RmmdPkRefNoDetails = list.RmmdPkRefNoDetails;
                list.RmmdRmmhPkRefNo = frmmaphdr.PkRefNo;
            }
            await _repo.SaveFormB14(domainModelFormB14);


            frmmaphdr = this._mapper.Map<FormMapHeaderDTO>((object)source);
            return frmmaphdr;
        }

        public RmMapHeader UpdateStatus(RmMapHeader form)
        {
            if (form.RmmhPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormR1Repository._context.RmMapHeader.Where(x => x.RmmhPkRefNo == form.RmmhPkRefNo).Select(x => new { Status = x.RmmhStatus, Log = x.RmmhAuditlog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.RmmhAuditlog = existsObj.Log;
                    form.RmmhStatus = existsObj.Status;
                }

            }


            if (form.RmmhSubmitSts == true && (string.IsNullOrEmpty(form.RmmhStatus) || form.RmmhStatus == Common.StatusList.FormQA1Saved || form.RmmhStatus == Common.StatusList.FormQA1Rejected))
            {
                //form.Fg1hInspectedBy = _security.UserID;
                //form.Fg1hInspectedName = _security.UserName;
                //form.Fg1hInspectedDt = DateTime.Today;
                form.RmmhStatus = Common.StatusList.FormQA1Submitted;
                form.RmmhAuditlog = Utility.ProcessLog(form.RmmhAuditlog, "Submitted", "Submitted", form.RmmhPreparedName, string.Empty, form.RmmhPreparedDate, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Executed By:" + form.RmmhPreparedName + " - Form M (" + form.RmmhPkRefNo + ")",//doubt
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/FormMap/Edit/" + form.RmmhPkRefNo.ToString() + "?view=1",
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }
            else if (string.IsNullOrEmpty(form.RmmhStatus) || form.RmmhStatus == "Initialize")
                form.RmmhStatus = Common.StatusList.FormR1R2Saved;

            return form;
        }
    }
}
