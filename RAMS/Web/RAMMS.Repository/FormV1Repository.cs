﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAMMS.Common;
using RAMMS.Common.Extensions;
using RAMMS.Common.RefNumber;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace RAMMS.Repository
{
    public class FormV1Repository : RepositoryBase<RmFormV1Hdr>, IFormV1Repository
    {
        private readonly IMapper _mapper;


        public FormV1Repository(RAMMSContext context, IMapper mapper = null) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        #region FormV1
        public async Task<List<RmFormV1Hdr>> GetFilteredRecordList(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV1Hdr> result = new List<RmFormV1Hdr>();
            var query = (from x in _context.RmFormV1Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv1hRmu || s.DdlTypeDesc == x.Fv1hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv1hSecCode || s.DdlTypeCode == x.Fv1hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv1hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv1hActiveYn == true).OrderByDescending(x => x.x.Fv1hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv1hCrewname == filterOptions.Filters.Crew || (x.x.Fv1hCrew.HasValue ? x.x.Fv1hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv1hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv1hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv1hDt.HasValue ? (x.x.Fv1hDt.Value.Year == dt.Year && x.x.Fv1hDt.Value.Month == dt.Month && x.x.Fv1hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv1hDt.HasValue ? (x.x.Fv1hDt.Value.Year == dt.Year && x.x.Fv1hDt.Value.Month == dt.Month && x.x.Fv1hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv1hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv1hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv1hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameSch.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameAck.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv1hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {

                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.Fv1hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.Fv1hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.Fv1hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.Fv1hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(x => x.x.Fv1hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(x => x.x.Fv1hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(x => x.x.Fv1hUsernameAck);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderBy(x => x.x.Fv1hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderBy(x => x.x.Fv1hUsernameSch);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderBy(x => x.x.Fv1hSubmitSts);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.Fv1hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.Fv1hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.Fv1hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.Fv1hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(x => x.x.Fv1hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(x => x.x.Fv1hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(x => x.x.Fv1hUsernameAck);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderByDescending(x => x.x.Fv1hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderByDescending(x => x.x.Fv1hUsernameSch);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderByDescending(x => x.x.Fv1hSubmitSts);
            }


            result = await query.Select(s => s.x)
                    .Skip(filterOptions.StartPageNo)
                    .Take(filterOptions.RecordsPerPage)
                    .ToListAsync();
            return result;
        }

        public async Task<int> GetFilteredRecordCount(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV1Hdr> result = new List<RmFormV1Hdr>();
            var query = (from x in _context.RmFormV1Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv1hRmu || s.DdlTypeDesc == x.Fv1hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv1hSecCode || s.DdlTypeCode == x.Fv1hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv1hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv1hActiveYn == true).OrderByDescending(x => x.x.Fv1hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv1hCrewname == filterOptions.Filters.Crew || (x.x.Fv1hCrew.HasValue ? x.x.Fv1hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv1hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv1hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv1hDt.HasValue ? (x.x.Fv1hDt.Value.Year == dt.Year && x.x.Fv1hDt.Value.Month == dt.Month && x.x.Fv1hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv1hDt.HasValue ? (x.x.Fv1hDt.Value.Year == dt.Year && x.x.Fv1hDt.Value.Month == dt.Month && x.x.Fv1hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv1hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv1hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv1hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameSch.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv1hUsernameAck.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv1hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }

            return await query.CountAsync().ConfigureAwait(false);
        }


        public async Task<List<RmFormV1Dtl>> GetFormV1WorkScheduleGridList(FilteredPagingDefinition<FormV1WorkScheduleGridDTO> filterOptions, int V1PkRefNo)
        {
            List<RmFormV1Dtl> result = new List<RmFormV1Dtl>();
            var query = (from x in _context.RmFormV1Dtl
                         where x.Fv1dFv1hPkRefNo == V1PkRefNo && x.Fv1dActiveYn == true
                         select new { x }).OrderByDescending(x => x.x.Fv1dPkRefNo);


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.Fv1dRoadCode);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.Fv1dRoadName);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.x.Fv1dFrmCh);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.Fv1dSiteRef);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.Fv1dStartTime);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(x => x.x.Fv1dRemarks);

            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.Fv1dRoadCode);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.Fv1dRoadName);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.x.Fv1dFrmCh);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.Fv1dSiteRef);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.Fv1dStartTime);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(x => x.x.Fv1dRemarks);
            }


            result = await query.Select(s => s.x)
                    .Skip(filterOptions.StartPageNo)
                    .Take(filterOptions.RecordsPerPage)
                    .ToListAsync();

            return result;
        }


        public async Task<RmFormV1Hdr> FindFormV1ByID(int id)
        {
            return await _context.RmFormV1Hdr.Where(x => x.Fv1hPkRefNo == id && x.Fv1hActiveYn == true).FirstOrDefaultAsync();
        }


        public List<SelectListItem> FindRefNoFromS1(FormV1ResponseDTO FormV1)
        {



            var res = (from hdr in _context.RmFormS1Hdr
                       join dtl in _context.RmFormS1Dtl on hdr.FsihPkRefNo equals dtl.FsidFsihPkRefNo
                       where hdr.FsihDt == FormV1.Dt && hdr.FsihRmu == FormV1.Rmu && Convert.ToInt32(dtl.FsidActCode) == Convert.ToInt32(FormV1.ActCode) && dtl.FsidActiveYn == true && hdr.FsihActiveYn == true
                       orderby hdr.FsihPkRefNo descending
                       select new
                       {
                           hdr.FsihRefId,
                           hdr.FsihPkRefNo
                       }).Distinct();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in res)
            {
                list.Add(new SelectListItem { Text = item.FsihRefId.ToString(), Value = item.FsihPkRefNo.ToString() });
            }

            return list;
        }


        public int LoadS1Data(int PKRefNo, int S1PKRefNo, string ActCode)
        {

            IList<RmFormV1Dtl> del = (from r in _context.RmFormV1Dtl where r.Fv1dFv1hPkRefNo == PKRefNo && r.Fv1dS1dPkRefNo != 0 select r).ToList();

            foreach (var item in del)
            {
                _context.Remove(item);
                _context.SaveChanges();
            }

            if (S1PKRefNo != 0)
            {
                var res = (from dtl in _context.RmFormS1Dtl
                           where dtl.FsidFsihPkRefNo == S1PKRefNo && dtl.FsidActCode == ActCode && dtl.FsidActiveYn == true
                           orderby dtl.FsidPkRefNo descending
                           select new RmFormV1Dtl
                           {
                               Fv1dFv1hPkRefNo = PKRefNo,
                               Fv1dS1dPkRefNo = dtl.FsidPkRefNo,
                               Fv1dActiveYn = true,
                               Fv1dCrDt = dtl.FsidCrDt,
                               Fv1dFrmCh = dtl.FsidFrmChKm,
                               Fv1dRemarks = dtl.FsidRemarks,
                               Fv1dRoadCode = dtl.FsiidRoadCode,
                               Fv1dRoadName = dtl.FsiidRoadName,
                               Fv1dFrmChDeci = dtl.FsidFrmChM == "" ? 0 : Convert.ToInt32(dtl.FsidFrmChM),
                               Fv1dSiteRef = dtl.FsidFormASiteRef,
                               Fv1dStartTime = "",
                               Fv1dToCh = dtl.FsidToChKm,
                               Fv1dToChDeci = dtl.FsidToChM == "" ? 0 : Convert.ToInt32(dtl.FsidToChM),
                           }).ToList();


                foreach (var item in res)
                {
                    _context.RmFormV1Dtl.Add(item);
                    _context.SaveChanges();
                }
            }

            return 1;
        }

        public int PullS1Data(int PKRefNo, int S1PKRefNo, string ActCode)
        {

            var Exist = from r in _context.RmFormV1Dtl where r.Fv1dFv1hPkRefNo == PKRefNo && r.Fv1dS1dPkRefNo != 0 select r.Fv1dS1dPkRefNo;
            var FromS1 = (from dtl in _context.RmFormS1Dtl
                          where dtl.FsidFsihPkRefNo == S1PKRefNo && dtl.FsidActCode == ActCode && dtl.FsidActiveYn == true && !Exist.Contains(dtl.FsidPkRefNo)
                          orderby dtl.FsidPkRefNo descending
                          select new RmFormV1Dtl
                          {
                              Fv1dFv1hPkRefNo = PKRefNo,
                              Fv1dS1dPkRefNo = dtl.FsidPkRefNo,
                              Fv1dActiveYn = true,
                              Fv1dCrDt = dtl.FsidCrDt,
                              Fv1dFrmCh = dtl.FsidFrmChKm,
                              Fv1dRemarks = dtl.FsidRemarks,
                              Fv1dRoadCode = dtl.FsiidRoadCode,
                              Fv1dRoadName = dtl.FsiidRoadName,
                              Fv1dFrmChDeci = dtl.FsidFrmChM == "" ? 0 : Convert.ToInt32(dtl.FsidFrmChM),
                              Fv1dSiteRef = dtl.FsidFormASiteRef,
                              Fv1dStartTime = "",
                              Fv1dToCh = dtl.FsidToChKm,
                              Fv1dToChDeci = dtl.FsidToChM == "" ? 0 : Convert.ToInt32(dtl.FsidToChM),
                          }).ToList();


            foreach (var item in FromS1)
            {
                _context.RmFormV1Dtl.Add(item);
                _context.SaveChanges();
            }

            return 1;
        }


        public int? SaveFormV1WorkSchedule(RmFormV1Dtl FormV1Dtl)
        {
            try
            {
                _context.Entry<RmFormV1Dtl>(FormV1Dtl).State = FormV1Dtl.Fv1dPkRefNo == 0 ? EntityState.Added : EntityState.Modified;
                _context.SaveChanges();

                return FormV1Dtl.Fv1dPkRefNo;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public int? UpdateFormV1WorkSchedule(RmFormV1Dtl FormV1Dtl)
        {
            try
            {
                _context.Set<RmFormV1Dtl>().Attach(FormV1Dtl);
                _context.Entry<RmFormV1Dtl>(FormV1Dtl).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }


        public int? DeleteFormV1(int id)
        {
            try
            {
                var res = _context.Set<RmFormV1Hdr>().FindAsync(id);
                res.Result.Fv1hActiveYn = false;
                _context.Set<RmFormV1Hdr>().Attach(res.Result);
                _context.Entry<RmFormV1Hdr>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }


        public int? DeleteFormV1WorkSchedule(int id)
        {
            try
            {
                var res = _context.Set<RmFormV1Dtl>().FindAsync(id);
                res.Result.Fv1dActiveYn = false;
                _context.Set<RmFormV1Dtl>().Attach(res.Result);
                _context.Entry<RmFormV1Dtl>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        #endregion

        #region FormV3

        public async Task<List<RmFormV3Hdr>> GetFilteredV3RecordList(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV3Hdr> result = new List<RmFormV3Hdr>();
            var query = (from x in _context.RmFormV3Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv3hRmu || s.DdlTypeDesc == x.Fv3hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv3hSecCode || s.DdlTypeCode == x.Fv3hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv3hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv3hActiveYn == true).OrderByDescending(x => x.x.Fv3hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv3hCrewname == filterOptions.Filters.Crew || (x.x.Fv3hCrew.HasValue ? x.x.Fv3hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv3hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv3hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv3hDt.HasValue ? (x.x.Fv3hDt.Value.Year == dt.Year && x.x.Fv3hDt.Value.Month == dt.Month && x.x.Fv3hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv3hDt.HasValue ? (x.x.Fv3hDt.Value.Year == dt.Year && x.x.Fv3hDt.Value.Month == dt.Month && x.x.Fv3hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv3hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv3hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv3hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameRec.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameFac.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv3hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {

                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.Fv3hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.Fv3hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.Fv3hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.Fv3hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(x => x.x.Fv3hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(x => x.x.Fv3hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(x => x.x.Fv3hUsernameRec);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderBy(x => x.x.Fv3hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderBy(x => x.x.Fv3hUsernameFac);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderBy(x => x.x.Fv3hSubmitSts);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.Fv3hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.Fv3hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.Fv3hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.Fv3hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(x => x.x.Fv3hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(x => x.x.Fv3hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(x => x.x.Fv3hUsernameRec);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderByDescending(x => x.x.Fv3hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderByDescending(x => x.x.Fv3hUsernameFac);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderByDescending(x => x.x.Fv3hSubmitSts);
            }


            result = await query.Select(s => s.x)
                    .Skip(filterOptions.StartPageNo)
                    .Take(filterOptions.RecordsPerPage)
                    .ToListAsync();
            return result;
        }

        public async Task<int> GetFilteredV3RecordCount(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV3Hdr> result = new List<RmFormV3Hdr>();
            var query = (from x in _context.RmFormV3Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv3hRmu || s.DdlTypeDesc == x.Fv3hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv3hSecCode || s.DdlTypeCode == x.Fv3hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv3hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv3hActiveYn == true).OrderByDescending(x => x.x.Fv3hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv3hCrewname == filterOptions.Filters.Crew || (x.x.Fv3hCrew.HasValue ? x.x.Fv3hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv3hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv3hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv3hDt.HasValue ? (x.x.Fv3hDt.Value.Year == dt.Year && x.x.Fv3hDt.Value.Month == dt.Month && x.x.Fv3hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv3hDt.HasValue ? (x.x.Fv3hDt.Value.Year == dt.Year && x.x.Fv3hDt.Value.Month == dt.Month && x.x.Fv3hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv3hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv3hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv3hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameRec.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv3hUsernameFac.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv3hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }

            return await query.CountAsync().ConfigureAwait(false);
        }

        public async Task<List<RmFormV3Dtl>> GetFormv3DtlGridList(FilteredPagingDefinition<FormV3DtlGridDTO> filterOptions, int V3PkRefNo)
        {
            List<RmFormV3Dtl> result = new List<RmFormV3Dtl>();
            var query = (from x in _context.RmFormV3Dtl
                         where x.Fv3dFv3hPkRefNo == V3PkRefNo && x.Fv3dActiveYn == true
                         select new { x }).OrderByDescending(x => x.x.Fv3dPkRefNo);


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.Fv3dRoadCode);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.Fv3dRoadName);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.x.Fv3dFrmCh);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.Fv3dToCh);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.Fv3dLength);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(x => x.x.Fv3dWidth);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(x => x.x.Fv3dTimetakenFrm);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(x => x.x.Fv3dTimeTakenTo);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderBy(x => x.x.Fv3dTimeTakenTotal);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderBy(x => x.x.Fv3dAdp);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderBy(x => x.x.Fv3dTransitTimeFrm);
                if (filterOptions.ColumnIndex == 12)
                    query = query.OrderBy(x => x.x.Fv3dTransitTimeTo);
                if (filterOptions.ColumnIndex == 13)
                    query = query.OrderBy(x => x.x.Fv3dTransitTimeTotal);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.Fv3dRoadCode);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.Fv3dRoadName);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.x.Fv3dFrmCh);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.Fv3dToCh);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.Fv3dLength);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(x => x.x.Fv3dWidth);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(x => x.x.Fv3dTimetakenFrm);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(x => x.x.Fv3dTimeTakenTo);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderByDescending(x => x.x.Fv3dTimeTakenTotal);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderByDescending(x => x.x.Fv3dAdp);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderByDescending(x => x.x.Fv3dTransitTimeFrm);
                if (filterOptions.ColumnIndex == 12)
                    query = query.OrderByDescending(x => x.x.Fv3dTransitTimeTo);
                if (filterOptions.ColumnIndex == 13)
                    query = query.OrderByDescending(x => x.x.Fv3dTransitTimeTotal);
            }


            result = await query.Select(s => s.x)
                    .Skip(filterOptions.StartPageNo)
                    .Take(filterOptions.RecordsPerPage)
                    .ToListAsync();

            return result;
        }


        public async Task<RmFormV3Hdr> FindFormV3ByID(int id)
        {
            return await _context.RmFormV3Hdr.Where(x => x.Fv3hPkRefNo == id && x.Fv3hActiveYn == true).FirstOrDefaultAsync();
        }

        public async Task<FormV3ResponseDTO> SaveFormV3(FormV3ResponseDTO Formv3)
        {
            try
            {
                var domainModelFormv3 = _mapper.Map<RmFormV3Hdr>(Formv3);
                domainModelFormv3.Fv3hPkRefNo = 0;

                //var obj = _context.RmFormV3Hdr.Where(x => x.Fv3hRmu == domainModelFormv3.Fv3hRmu && x.Fv3hActCode == domainModelFormv3.Fv3hActCode && x.Fv3hSecCode == domainModelFormv3.Fv3hSecCode && x.Fv3hCrew == domainModelFormv3.Fv3hCrew && x.Fv3hDt == domainModelFormv3.Fv3hDt && x.Fv3hActiveYn == true).ToList();
                var obj = _context.RmFormV3Hdr.Where(x => x.Fv3hRmu == domainModelFormv3.Fv3hRmu && x.Fv3hActCode == domainModelFormv3.Fv3hActCode && x.Fv3hDt == domainModelFormv3.Fv3hDt && x.Fv3hCrew == domainModelFormv3.Fv3hCrew && x.Fv3hActiveYn == true).ToList();
                if (obj.Count > 0)
                {
                    return _mapper.Map<FormV3ResponseDTO>(obj);
                }
                var objV2 = _context.RmFormV3Hdr.Where(x => x.Fv3hRmu == domainModelFormv3.Fv3hRmu && x.Fv3hActCode == domainModelFormv3.Fv3hActCode && x.Fv3hDt == domainModelFormv3.Fv3hDt && x.Fv3hCrew == domainModelFormv3.Fv3hCrew && x.Fv3hActiveYn == true).ToList();
                if (objV2.Count == 0)
                {
                    Formv3.PkRefNo = -2;
                    return Formv3;
                }
                else
                {
                    var objV1 = _context.RmFormV1Hdr.Where(x => x.Fv1hRmu == domainModelFormv3.Fv3hRmu && x.Fv1hActCode == domainModelFormv3.Fv3hActCode && x.Fv1hDt == domainModelFormv3.Fv3hDt && x.Fv1hActiveYn == true).ToList();
                    if (objV1.Count > 0)
                    {
                        domainModelFormv3.Fv3hFv1PkRefNo = objV1.Single().Fv1hPkRefNo;
                        _context.Set<RmFormV3Hdr>().Add(domainModelFormv3);
                        _context.SaveChanges();

                        IDictionary<string, string> lstData = new Dictionary<string, string>();
                        lstData.Add("YYYYMMDD", Utility.ToString(DateTime.Today.ToString("yyyyMMdd")));
                        lstData.Add("Crew", domainModelFormv3.Fv3hCrew.ToString());
                        lstData.Add("ActivityCode", domainModelFormv3.Fv3hActCode);
                        lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(domainModelFormv3.Fv3hPkRefNo));
                        domainModelFormv3.Fv3hRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormV3Header, lstData);
                        _context.SaveChanges();
                        Formv3.PkRefNo = _mapper.Map<FormV3ResponseDTO>(domainModelFormv3).PkRefNo;
                        Formv3.RefId = domainModelFormv3.Fv3hRefId;
                        Formv3.Status = domainModelFormv3.Fv3hStatus;

                        var res = (from dtl in _context.RmFormV1Dtl
                                   where dtl.Fv1dFv1hPkRefNo == domainModelFormv3.Fv3hFv1PkRefNo  && dtl.Fv1dActiveYn == true
                                   orderby dtl.Fv1dPkRefNo descending
                                   select new RmFormV3Dtl
                                   {
                                       Fv3dFv3hPkRefNo = Formv3.PkRefNo,
                                       Fv3dFv1dPkRefNo = dtl.Fv1dPkRefNo, 
                                       Fv3dActiveYn = true,
                                       Fv3dFrmChDeci = dtl.Fv1dFrmChDeci,
                                       Fv3dFrmCh = dtl.Fv1dFrmCh,
                                       Fv3dToChDeci = dtl.Fv1dToCh,
                                       Fv3dToCh = dtl.Fv1dToChDeci,
                                       Fv3dRoadCode = dtl.Fv1dRoadCode,
                                       Fv3dRoadName = dtl.Fv1dRoadName,
                                       
                                   }).ToList();


                        foreach (var item in res)
                        {
                            _context.RmFormV3Dtl.Add(item);
                            _context.SaveChanges();
                        }


                        return Formv3;
                    }
                    else
                    {
                        Formv3.PkRefNo = -1;
                        return Formv3;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                await _context.DisposeAsync();
                throw ex;
            }
        }


        public async Task<int> UpdateFormV3(RmFormV3Hdr FormV3)
        {
            try
            {
                _context.Set<RmFormV3Hdr>().Attach(FormV3);
                _context.Entry<RmFormV3Hdr>(FormV3).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public async Task<int> UpdateFormV3Dtl(RmFormV3Dtl FormV3Dtl)
        {
            try
            {
                _context.Set<RmFormV3Dtl>().Attach(FormV3Dtl);
                _context.Entry<RmFormV3Dtl>(FormV3Dtl).State = EntityState.Modified;
               await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public int? DeleteFormV3(int id)
        {
            try
            {
                var res = _context.Set<RmFormV3Hdr>().FindAsync(id);
                res.Result.Fv3hActiveYn = false;
                _context.Set<RmFormV3Hdr>().Attach(res.Result);
                _context.Entry<RmFormV3Hdr>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }


        public int? DeleteFormV3Dtl(int id)
        {
            try
            {
                var res = _context.Set<RmFormV3Dtl>().FindAsync(id);
                res.Result.Fv3dActiveYn = false;
                _context.Set<RmFormV3Dtl>().Attach(res.Result);
                _context.Entry<RmFormV3Dtl>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        #endregion

        #region FormV4

        public async Task<List<RmFormV4Hdr>> GetFilteredV4RecordList(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV4Hdr> result = new List<RmFormV4Hdr>();
            var query = (from x in _context.RmFormV4Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv4hRmu || s.DdlTypeDesc == x.Fv4hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv4hSecCode || s.DdlTypeCode == x.Fv4hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv4hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv4hActiveYn == true).OrderByDescending(x => x.x.Fv4hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv4hCrewname == filterOptions.Filters.Crew || (x.x.Fv4hCrew.HasValue ? x.x.Fv4hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv4hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv4hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv4hDt.HasValue ? (x.x.Fv4hDt.Value.Year == dt.Year && x.x.Fv4hDt.Value.Month == dt.Month && x.x.Fv4hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv4hDt.HasValue ? (x.x.Fv4hDt.Value.Year == dt.Year && x.x.Fv4hDt.Value.Month == dt.Month && x.x.Fv4hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv4hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv4hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv4hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameRec.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameFac.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv4hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {

                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.Fv4hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.Fv4hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.Fv4hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.Fv4hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(x => x.x.Fv4hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(x => x.x.Fv4hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(x => x.x.Fv4hUsernameRec);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderBy(x => x.x.Fv4hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderBy(x => x.x.Fv4hUsernameFac);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderBy(x => x.x.Fv4hSubmitSts);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.Fv4hPkRefNo);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.Fv4hRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.div.RdsmDivCode);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.Fv4hActCode);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.Fv4hSecCode);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(x => x.x.Fv4hCrewname);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(x => x.x.Fv4hDt);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(x => x.x.Fv4hUsernameRec);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderByDescending(x => x.x.Fv4hUsernameAgr);
                if (filterOptions.ColumnIndex == 10)
                    query = query.OrderByDescending(x => x.x.Fv4hUsernameFac);
                if (filterOptions.ColumnIndex == 11)
                    query = query.OrderByDescending(x => x.x.Fv4hSubmitSts);
            }


            result = await query.Select(s => s.x)
                    .Skip(filterOptions.StartPageNo)
                    .Take(filterOptions.RecordsPerPage)
                    .ToListAsync();
            return result;
        }

        public async Task<int> GetFilteredV4RecordCount(FilteredPagingDefinition<FormV1SearchGridDTO> filterOptions)
        {
            List<RmFormV4Hdr> result = new List<RmFormV4Hdr>();
            var query = (from x in _context.RmFormV4Hdr
                         let rmu = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "RMU" && (s.DdlTypeCode == x.Fv4hRmu || s.DdlTypeDesc == x.Fv4hRmu))
                         let sec = _context.RmDdLookup.FirstOrDefault(s => s.DdlType == "Section Code" && (s.DdlTypeDesc == x.Fv4hSecCode || s.DdlTypeCode == x.Fv4hSecCode))
                         let div = _context.RmDivRmuSecMaster.FirstOrDefault(s => s.RdsmSectionCode == x.Fv4hSecCode)
                         select new { rmu, sec, x, div });



            query = query.Where(x => x.x.Fv4hActiveYn == true).OrderByDescending(x => x.x.Fv4hPkRefNo);
            if (filterOptions.Filters != null)
            {

                if (!string.IsNullOrEmpty(filterOptions.Filters.Section))
                {
                    query = query.Where(x => x.sec.DdlTypeDesc == filterOptions.Filters.Section || x.sec.DdlTypeCode == filterOptions.Filters.Section);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
                {
                    query = query.Where(x => x.rmu.DdlTypeCode == filterOptions.Filters.RMU || x.rmu.DdlTypeDesc == filterOptions.Filters.RMU);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.Crew))
                {
                    query = query.Where(x => x.x.Fv4hCrewname == filterOptions.Filters.Crew || (x.x.Fv4hCrew.HasValue ? x.x.Fv4hCrew.ToString() == filterOptions.Filters.Crew : x.x.Fv4hCrew.ToString() == ""));
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ActivityCode))
                {
                    query = query.Where(x => x.x.Fv4hActCode == filterOptions.Filters.ActivityCode);
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv4hDt.HasValue ? (x.x.Fv4hDt.Value.Year == dt.Year && x.x.Fv4hDt.Value.Month == dt.Month && x.x.Fv4hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (string.IsNullOrEmpty(filterOptions.Filters.ByFromdate) && !string.IsNullOrEmpty(filterOptions.Filters.ByTodate))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(filterOptions.Filters.ByTodate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    {
                        query = query.Where(x => x.x.Fv4hDt.HasValue ? (x.x.Fv4hDt.Value.Year == dt.Year && x.x.Fv4hDt.Value.Month == dt.Month && x.x.Fv4hDt.Value.Day == dt.Day) : false);
                    }
                }

                if (!string.IsNullOrEmpty(filterOptions.Filters.SmartInputValue))
                {
                    query = query.Where(x => x.x.Fv4hRmu.Contains(filterOptions.Filters.SmartInputValue)
                                        || (x.rmu.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || (x.sec.DdlTypeDesc.Contains(filterOptions.Filters.SmartInputValue))
                                        || x.x.Fv4hCrewname.Contains(filterOptions.Filters.SmartInputValue)
                                        || ((bool)x.x.Fv4hSubmitSts ? "Submitted" : "Saved").Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hRefId.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hSecCode.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameRec.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameAgr.Contains(filterOptions.Filters.SmartInputValue)
                                        || x.x.Fv4hUsernameFac.Contains(filterOptions.Filters.SmartInputValue)
                                        || (filterOptions.Filters.SmartInputValue.IsInt() && x.x.Fv4hPkRefNo.Equals(filterOptions.Filters.SmartInputValue.AsInt())));

                }
            }

            return await query.CountAsync().ConfigureAwait(false);
        }

    

        public async Task<RmFormV4Hdr> FindFormV4ByID(int id)
        {
            return await _context.RmFormV4Hdr.Where(x => x.Fv4hPkRefNo == id && x.Fv4hActiveYn == true).FirstOrDefaultAsync();
        }

        public async Task<FormV4ResponseDTO> SaveFormV4(FormV4ResponseDTO Formv4)
        {
            try
            {
                var domainModelFormv4 = _mapper.Map<RmFormV4Hdr>(Formv4);
                domainModelFormv4.Fv4hPkRefNo = 0;

                //var obj = _context.RmFormV4Hdr.Where(x => x.Fv4hRmu == domainModelFormv4.Fv4hRmu && x.Fv4hActCode == domainModelFormv4.Fv4hActCode && x.Fv4hSecCode == domainModelFormv4.Fv4hSecCode && x.Fv4hCrew == domainModelFormv4.Fv4hCrew && x.Fv4hDt == domainModelFormv4.Fv4hDt && x.Fv4hActiveYn == true).ToList();
                var obj = _context.RmFormV4Hdr.Where(x => x.Fv4hRmu == domainModelFormv4.Fv4hRmu && x.Fv4hActCode == domainModelFormv4.Fv4hActCode && x.Fv4hDt == domainModelFormv4.Fv4hDt && x.Fv4hCrew == domainModelFormv4.Fv4hCrew && x.Fv4hActiveYn == true).ToList();
                if (obj.Count > 0)
                {
                    return _mapper.Map<FormV4ResponseDTO>(obj);
                }
                var objV2 = _context.RmFormV4Hdr.Where(x => x.Fv4hRmu == domainModelFormv4.Fv4hRmu && x.Fv4hActCode == domainModelFormv4.Fv4hActCode && x.Fv4hDt == domainModelFormv4.Fv4hDt && x.Fv4hCrew == domainModelFormv4.Fv4hCrew && x.Fv4hActiveYn == true).ToList();
                if (objV2.Count == 0)
                {
                    Formv4.PkRefNo = -2;
                    return Formv4;
                }
                else
                {
                    var objV1 = _context.RmFormV1Hdr.Where(x => x.Fv1hRmu == domainModelFormv4.Fv4hRmu && x.Fv1hActCode == domainModelFormv4.Fv4hActCode && x.Fv1hDt == domainModelFormv4.Fv4hDt && x.Fv1hActiveYn == true).ToList();
                    if (objV1.Count > 0)
                    {
                        domainModelFormv4.Fv4hFv1PkRefNo = objV1.Single().Fv1hPkRefNo;
                        _context.Set<RmFormV4Hdr>().Add(domainModelFormv4);
                        _context.SaveChanges();

                        IDictionary<string, string> lstData = new Dictionary<string, string>();
                        lstData.Add("YYYYMMDD", Utility.ToString(DateTime.Today.ToString("yyyyMMdd")));
                        lstData.Add("Crew", domainModelFormv4.Fv4hCrew.ToString());
                        lstData.Add("ActivityCode", domainModelFormv4.Fv4hActCode);
                        lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(domainModelFormv4.Fv4hPkRefNo));
                        domainModelFormv4.Fv4hRefId = FormRefNumber.GetRefNumber(RAMMS.Common.RefNumber.FormType.FormV4Header, lstData);
                        _context.SaveChanges();
                        Formv4.PkRefNo = _mapper.Map<FormV4ResponseDTO>(domainModelFormv4).PkRefNo;
                        Formv4.RefId = domainModelFormv4.Fv4hRefId;
                        Formv4.Status = domainModelFormv4.Fv4hStatus;

                        var res = (from dtl in _context.RmFormV1Dtl
                                   where dtl.Fv1dFv1hPkRefNo == domainModelFormv4.Fv4hFv1PkRefNo && dtl.Fv1dActiveYn == true
                                   orderby dtl.Fv1dPkRefNo descending
                                   select new RmFormV4Dtl
                                   {
                                       Fv4dFv4hPkRefNo = Formv4.PkRefNo,
                                       Fv4dFv1dPkRefNo = dtl.Fv1dPkRefNo,
                                       Fv4dActiveYn = true,
                                       Fv4dFrmChDeci = dtl.Fv1dFrmChDeci,
                                       Fv4dFrmCh = dtl.Fv1dFrmCh,
                                       Fv4dToChDeci = dtl.Fv1dToCh,
                                       Fv4dToCh = dtl.Fv1dToChDeci,
                                       Fv4dRoadCode = dtl.Fv1dRoadCode,
                                       Fv4dRoadName = dtl.Fv1dRoadName,

                                   }).ToList();


                        foreach (var item in res)
                        {
                            _context.RmFormV4Dtl.Add(item);
                            _context.SaveChanges();
                        }


                        return Formv4;
                    }
                    else
                    {
                        Formv4.PkRefNo = -1;
                        return Formv4;
                    }

                }
            }
            catch (Exception ex)
            {
                await _context.DisposeAsync();
                throw ex;
            }
        }


        public async Task<int> UpdateFormV4(RmFormV4Hdr FormV4)
        {
            try
            {
                _context.Set<RmFormV4Hdr>().Attach(FormV4);
                _context.Entry<RmFormV4Hdr>(FormV4).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public int? DeleteFormV4(int id)
        {
            try
            {
                var res = _context.Set<RmFormV4Hdr>().FindAsync(id);
                res.Result.Fv4hActiveYn = false;
                _context.Set<RmFormV4Hdr>().Attach(res.Result);
                _context.Entry<RmFormV4Hdr>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

 
        #endregion

    }
}
