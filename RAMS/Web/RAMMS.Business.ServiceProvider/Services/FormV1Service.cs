﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace RAMMS.Business.ServiceProvider.Services
{

    public class FormV1Service : IFormV1Service
    {
        private readonly IFormV1Repository _repo;
        private readonly IRepositoryUnit _repoUnit;
        private readonly IMapper _mapper;
        private readonly ISecurity _security;
        private readonly IProcessService processService;

        #region FormV1
        public FormV1Service(IRepositoryUnit repoUnit, IFormV1Repository repo, IMapper mapper, ISecurity security, IProcessService process)
        {
            _repoUnit = repoUnit ?? throw new ArgumentNullException(nameof(repoUnit));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _security = security;
            processService = process;
            _repo = repo;
        }


        public async Task<PagingResult<FormV1ResponseDTO>> GetFilteredFormV1Grid(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            PagingResult<FormV1ResponseDTO> result = new PagingResult<FormV1ResponseDTO>();

            List<FormV1ResponseDTO> formDList = new List<FormV1ResponseDTO>();
            try
            {
                var filteredRecords = await _repoUnit.FormV1Repository.GetFilteredRecordList(filterOptions);

                result.TotalRecords = await _repoUnit.FormV1Repository.GetFilteredRecordCount(filterOptions).ConfigureAwait(false);

                foreach (var listData in filteredRecords)
                {
                    var _ = _mapper.Map<FormV1ResponseDTO>(listData);
                    // _.ProcessStatus = listData.FV1hStatus;

                    formDList.Add(_);
                }

                result.PageResult = formDList;

                result.PageNo = filterOptions.StartPageNo;
                result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
            return result;
        }


        public async Task<PagingResult<FormV1WorkScheduleGridDTO>> GetFormV1WorkScheduleGridList(FilteredPagingDefinition<FormV1WorkScheduleGridDTO> filterOptions, int V1PkRefNo)
        {
            PagingResult<FormV1WorkScheduleGridDTO> result = new PagingResult<FormV1WorkScheduleGridDTO>();

            List<FormV1WorkScheduleGridDTO> formV1WorkScheduleList = new List<FormV1WorkScheduleGridDTO>();
            try
            {
                var filteredRecords = await _repoUnit.FormV1Repository.GetFormV1WorkScheduleGridList(filterOptions, V1PkRefNo);

                result.TotalRecords = filteredRecords.Count();  // await _repoUnit.FormDRepository.GetFilteredRecordCount(filterOptions).ConfigureAwait(false);

                foreach (var listData in filteredRecords)
                {
                    FormV1WorkScheduleGridDTO obj = new FormV1WorkScheduleGridDTO();

                    obj.Fv1hPkRefNo = listData.Fv1dFv1hPkRefNo;
                    obj.Fs1dPkRefNo = listData.Fv1dS1dPkRefNo;
                    obj.Chainage = Convert.ToString(listData.Fv1dFrmChDeci);
                    obj.ChainageFrom = Convert.ToString(listData.Fv1dFrmCh);
                    obj.ChainageFromDec = Convert.ToString(listData.Fv1dFrmChDeci);
                    obj.ChainageTo = Convert.ToString(listData.Fv1dToCh);
                    obj.ChainageToDec = Convert.ToString(listData.Fv1dToChDeci);
                    obj.PkRefNo = listData.Fv1dPkRefNo;
                    obj.Remarks = listData.Fv1dRemarks;
                    obj.RoadCode = listData.Fv1dRoadCode;
                    obj.RoadName = listData.Fv1dRoadName;
                    obj.SiteRef = listData.Fv1dSiteRef;
                    obj.StartTime = listData.Fv1dStartTime;

                    formV1WorkScheduleList.Add(obj);
                }

                result.PageResult = formV1WorkScheduleList;

                result.PageNo = filterOptions.StartPageNo;
                result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
            return result;
        }



        public async Task<FormV1ResponseDTO> SaveFormV1(FormV1ResponseDTO FormV1)
        {
            try
            {
                var domainModelFormV1 = _mapper.Map<RmFormV1Hdr>(FormV1);
                domainModelFormV1.Fv1hPkRefNo = 0;

                var obj = _repoUnit.FormV1Repository.FindAsync(x => x.Fv1hRmu == domainModelFormV1.Fv1hRmu && x.Fv1hActCode == domainModelFormV1.Fv1hActCode && x.Fv1hSecCode == domainModelFormV1.Fv1hSecCode && x.Fv1hCrew == domainModelFormV1.Fv1hCrew && x.Fv1hDt == domainModelFormV1.Fv1hDt && x.Fv1hActiveYn == true).Result;
                if (obj != null)
                    return _mapper.Map<FormV1ResponseDTO>(obj);

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYYMMDD", Utility.ToString(DateTime.Today.ToString("yyyyMMdd")));
                lstData.Add("Crew", domainModelFormV1.Fv1hCrew.ToString());
                lstData.Add("ActivityCode", domainModelFormV1.Fv1hActCode);
                domainModelFormV1.Fv1hRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormV1Header, lstData);

                var entity = _repoUnit.FormV1Repository.CreateReturnEntity(domainModelFormV1);
                FormV1.PkRefNo = _mapper.Map<FormV1ResponseDTO>(entity).PkRefNo;
                FormV1.RefId = domainModelFormV1.Fv1hRefId;
                FormV1.Status = domainModelFormV1.Fv1hStatus;

                return FormV1;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public int? SaveFormV1WorkSchedule(FormV1DtlResponseDTO FormV1Dtl)
        {

            try
            {
                var model = _mapper.Map<RmFormV1Dtl>(FormV1Dtl);
                model.Fv1dPkRefNo = 0;
                return _repo.SaveFormV1WorkSchedule(model);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public int? UpdateFormV1WorkSchedule(FormV1DtlResponseDTO FormV1Dtl)
        {

            try
            {
                int? Fv1hPkRefNo = FormV1Dtl.Fv1hPkRefNo;
                var model = _mapper.Map<RmFormV1Dtl>(FormV1Dtl);
                model.Fv1dPkRefNo = 0;
                model.Fv1dFv1hPkRefNo = Fv1hPkRefNo;
                return _repo.UpdateFormV1WorkSchedule(model);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public int? DeleteFormV1(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormV1(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        public int? DeleteFormV1WorkSchedule(int id)
        {
            int? rowsAffected;
            try
            {
                rowsAffected = _repo.DeleteFormV1WorkSchedule(id);
            }
            catch (Exception ex)
            {
                _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }


        public async Task<int> Update(FormV1ResponseDTO FormV1)
        {
            int rowsAffected;
            try
            {
                int PkRefNo = FormV1.PkRefNo;

                var domainModelFormV1 = _mapper.Map<RmFormV1Hdr>(FormV1);
                domainModelFormV1.Fv1hPkRefNo = PkRefNo;
                domainModelFormV1.Fv1hActiveYn = true;
                domainModelFormV1 = UpdateStatus(domainModelFormV1);
                _repoUnit.FormV1Repository.Update(domainModelFormV1);
                rowsAffected = await _repoUnit.CommitAsync();
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }

        public RmFormV1Hdr UpdateStatus(RmFormV1Hdr form)
        {
            if (form.Fv1hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormV1Repository._context.RmFormV1Hdr.Where(x => x.Fv1hPkRefNo == form.Fv1hPkRefNo).Select(x => new { Status = x.Fv1hStatus, Log = x.Fv1hAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.Fv1hAuditLog = existsObj.Log;
                   // form.Fv1hStatus = existsObj.Status;

                }

            }
            if (form.Fv1hSubmitSts && form.Fv1hStatus == "Saved")
            {
                form.Fv1hStatus = Common.StatusList.FormW2Submitted;
                form.Fv1hAuditLog = Utility.ProcessLog(form.Fv1hAuditLog, "Submitted By", "Submitted", form.Fv1hUsernameSch, string.Empty, form.Fv1hDtSch, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Recorded By:" + form.Fv1hUsernameSch + " - Form WN (" + form.Fv1hPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/MAM/EditFormV1?id=" + form.Fv1hPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }
 
        public async Task<FormV1ResponseDTO> FindFormV1ByID(int id)
        {
            RmFormV1Hdr formV1 = await _repo.FindFormV1ByID(id);
            return _mapper.Map<FormV1ResponseDTO>(formV1);

        }

        public List<SelectListItem> FindRefNoFromS1(FormV1ResponseDTO FormV1)
        {
            return _repo.FindRefNoFromS1(FormV1);
        }

        public int LoadS1Data(int PKRefNo, int S1PKRefNo, string ActCode)
        {
            return _repo.LoadS1Data(PKRefNo, S1PKRefNo, ActCode);
        }


        public int PullS1Data(int PKRefNo, int S1PKRefNo, string ActCode)
        {
            return _repo.PullS1Data(PKRefNo, S1PKRefNo, ActCode);
        }

        #endregion

        #region FormV3


        public async Task<PagingResult<FormV3ResponseDTO>> GetFilteredFormV3Grid(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            PagingResult<FormV3ResponseDTO> result = new PagingResult<FormV3ResponseDTO>();

            List<FormV3ResponseDTO> formDList = new List<FormV3ResponseDTO>();
            try
            {
                var filteredRecords = await _repoUnit.FormV1Repository.GetFilteredRecordList(filterOptions);

                result.TotalRecords = await _repoUnit.FormV1Repository.GetFilteredRecordCount(filterOptions).ConfigureAwait(false);

                foreach (var listData in filteredRecords)
                {
                    var _ = _mapper.Map<FormV3ResponseDTO>(listData);
                   
                    formDList.Add(_);
                }

                result.PageResult = formDList;

                result.PageNo = filterOptions.StartPageNo;
                result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
            return result;
        }



        public async Task<PagingResult<FormV3DtlGridDTO>> GetFormV3DtlGridList(FilteredPagingDefinition<FormV3DtlGridDTO> filterOptions, int V3PkRefNo)
        {
            PagingResult<FormV3DtlGridDTO> result = new PagingResult<FormV3DtlGridDTO>();

            List<FormV3DtlGridDTO> formV3DtlList = new List<FormV3DtlGridDTO>();
            try
            {
                var filteredRecords = await _repoUnit.FormV1Repository.GetFormv3DtlGridList(filterOptions, V3PkRefNo);

                result.TotalRecords = filteredRecords.Count();  // await _repoUnit.FormDRepository.GetFilteredRecordCount(filterOptions).ConfigureAwait(false);

                foreach (var listData in filteredRecords)
                {
                    FormV3DtlGridDTO obj = new FormV3DtlGridDTO();

                    obj.Fv3hPkRefNo = listData.Fv3dFv3hPkRefNo;
                    obj.PkRefNo = listData.Fv3dPkRefNo;
                    obj.FrmCh=  listData.Fv3dFrmCh;
                    obj.ToCh = listData.Fv3dToCh;
                    obj.RoadCode = listData.Fv3dRoadCode;
                    obj.RoadName = listData.Fv3dRoadName;
                    obj.Length = listData.Fv3dLength;
                    obj.Width = listData.Fv3dWidth;
                    obj.TimetakenFrm = listData.Fv3dTimetakenFrm;
                    obj.TimeTakenTo = listData.Fv3dTimeTakenTo;
                    obj.TimeTakenTotal = listData.Fv3dTimeTakenTotal;
                    obj.Adp = listData.Fv3dAdp;
                    obj.TransitTimeFrm = listData.Fv3dTransitTimeFrm;
                    obj.TransitTimeTo = listData.Fv3dTransitTimeTo;
                    obj.TransitTimeTotal = listData.Fv3dTransitTimeTotal;

                    formV3DtlList.Add(obj);
                }

                result.PageResult = formV3DtlList;

                result.PageNo = filterOptions.StartPageNo;
                result.FilteredRecords = result.PageResult != null ? result.PageResult.Count : 0;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
            return result;
        }

        public async Task<FormV3ResponseDTO> FindFormV3ByID(int id)
        {
            RmFormV3Hdr formV3 = await _repo.FindFormV3ByID(id);
            return _mapper.Map<FormV3ResponseDTO>(formV3);

        }

        public async Task<FormV3ResponseDTO> SaveFormV3(FormV3ResponseDTO Formv3)
        {
            try
            {
                var domainModelFormv3 = _mapper.Map<RmFormV3Hdr>(Formv3);
                domainModelFormv3.Fv3hPkRefNo = 0;

                var obj = _repoUnit.FormV1Repository.FindAsync(x => x.Fv1hRmu == domainModelFormv3.Fv3hRmu && x.Fv3hActCode == domainModelFormv3.Fv3hActCode && x.Fv3hSecCode == domainModelFormv3.Fv3hSecCode && x.Fv3hCrew == domainModelFormv3.Fv3hCrew && x.Fv3hDt == domainModelFormv3.Fv3hDt && x.Fv3hActiveYn == true).Result;
                if (obj != null)
                    return _mapper.Map<Formv3ResponseDTO>(obj);

                IDictionary<string, string> lstData = new Dictionary<string, string>();
                lstData.Add("YYYYMMDD", Utility.ToString(DateTime.Today.ToString("yyyyMMdd")));
                lstData.Add("Crew", domainModelFormv3.Fv3hCrew.ToString());
                lstData.Add("ActivityCode", domainModelFormv3.Fv3hActCode);
                domainModelFormv3.Fv3hRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.Formv3Header, lstData);

                var entity = _repoUnit.FormV1Repository.CreateReturnEntity(domainModelFormv3);
                Formv3.PkRefNo = _mapper.Map<Formv3ResponseDTO>(entity).PkRefNo;
                Formv3.RefId = domainModelFormv3.Fv3hRefId;
                Formv3.Status = domainModelFormv3.Fv3hStatus;

                return Formv3;
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }
        }

        public async Task<int> UpdateV3(FormV3ResponseDTO FormV3)
        {
            int rowsAffected;
            try
            {
                int PkRefNo = FormV3.PkRefNo;

                var domainModelFormV3 = _mapper.Map<RmFormV3Hdr>(FormV3);
                domainModelFormV3.Fv3hPkRefNo = PkRefNo;
                domainModelFormV3.Fv3hActiveYn = true;
                domainModelFormV3 = UpdateStatus(domainModelFormV3);
                _repoUnit.FormV1Repository.Update(domainModelFormV3);
                rowsAffected = await _repoUnit.CommitAsync();
            }
            catch (Exception ex)
            {
                await _repoUnit.RollbackAsync();
                throw ex;
            }

            return rowsAffected;
        }

        public RmFormV1Hdr UpdateV3Status(RmFormV1Hdr form)
        {
            if (form.Fv1hPkRefNo > 0)
            {
                var existsObj = _repoUnit.FormV1Repository._context.RmFormV1Hdr.Where(x => x.Fv1hPkRefNo == form.Fv1hPkRefNo).Select(x => new { Status = x.Fv1hStatus, Log = x.Fv1hAuditLog }).FirstOrDefault();
                if (existsObj != null)
                {
                    form.Fv1hAuditLog = existsObj.Log;
                    // form.Fv1hStatus = existsObj.Status;

                }

            }
            if (form.Fv1hSubmitSts && form.Fv1hStatus == "Saved")
            {
                form.Fv1hStatus = Common.StatusList.FormW2Submitted;
                form.Fv1hAuditLog = Utility.ProcessLog(form.Fv1hAuditLog, "Submitted By", "Submitted", form.Fv1hUsernameSch, string.Empty, form.Fv1hDtSch, _security.UserName);
                processService.SaveNotification(new RmUserNotification()
                {
                    RmNotCrBy = _security.UserName,
                    RmNotGroup = GroupNames.OperationsExecutive,
                    RmNotMessage = "Recorded By:" + form.Fv1hUsernameSch + " - Form WN (" + form.Fv1hPkRefNo + ")",
                    RmNotOn = DateTime.Now,
                    RmNotUrl = "/MAM/EditFormV1?id=" + form.Fv1hPkRefNo.ToString(),
                    RmNotUserId = "",
                    RmNotViewed = ""
                }, true);
            }

            return form;
        }

        #endregion

    }
}
