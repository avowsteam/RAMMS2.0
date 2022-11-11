﻿using Microsoft.EntityFrameworkCore;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository
{
    public class FormUCUARepository : RepositoryBase<RmUcua>,IFormUCUARepository
    {
        public FormUCUARepository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public int? SaveFormUCUA(RmUcua FormUCUA)
        {
            try
            {
                _context.Entry<RmUcua>(FormUCUA).State = FormUCUA.RmmhPkRefNo == 0 ? EntityState.Added : EntityState.Modified;
                _context.SaveChanges();


                return FormUCUA.RmmhPkRefNo;
            }
            catch (Exception ex)
            {
                return 500;

            }
        }
        public async Task<FORMTRpt> GetReportData(int headerid)
        {
            int? pkrefno = (from s in _context.RmFormTDailyInspection
                            where s.FmtdiPkRefNo == headerid
                            select s.FmtdiFmtPkRefNo).FirstOrDefault();

            FORMTRpt result = (from s in _context.RmFormTHdr
                               where s.FmtPkRefNo == pkrefno
                               select new FORMTRpt
                               {
                                   InspectedDate = s.FmtInspectionDate,
                                   RMU = (from r in _context.RmDdLookup where r.DdlType == "RMU" && r.DdlTypeCode == s.FmtRmuCode select r.DdlTypeDesc).FirstOrDefault(),
                                   RoadCode = s.FmtRdCode,
                                   RoadName = s.FmtRdName,
                                   HdDate = s.FmtDateHdd,
                                   HdName = s.FmtUsernameHdd,
                                   HdDesg = s.FmtDesignationHdd,
                                   RecDate = s.FmtDateRcd,
                                   RecDesg = s.FmtDesignationRcd,
                                   RecName = s.FmtUsernameRcd,
                                   RefId = s.FmtPkRefId,
                                   RefNo = s.FmtReferenceNo,
                               }).FirstOrDefault();


            result.Details = (from d in _context.RmFormTDailyInspection
                              where d.FmtdiPkRefNo == headerid
                              select new FORMTRptDetail
                              {

                                  Day = d.FmtdiDay,
                                  Description = d.FmtdiDescription,
                                  DescriptionHV = d.FmtdiDescriptionHv,
                                  DescriptionMC = d.FmtdiDescriptionMc,
                                  DescriptionPC = d.FmtdiDescriptionPc,
                                  DirectionFrom = d.FmtdiDirectionFrm,
                                  DirectionTo = d.FmtdiDirectionTo,
                                  FromTime = d.FmtdiAuditTimeFrm,
                                  HourlycountPerDay = d.FmtdiHourlyCountPerDay,
                                  TotalDay = d.FmtdiTotalDay,
                                  ToTime = d.FmtdiAuditTimeTo

                              }).FirstOrDefault();


            result.Details.Vechilce = (from v in _context.RmFormTVechicle
                                       where v.FmtvFmtdiPkRefNo == headerid
                                       select new FORMTRptVechicle
                                       {
                                           Axle = v.FmtvAxle,
                                           Count = v.FmtvCount,
                                           Loading = v.FmtvLoading,
                                           Time = Convert.ToInt32(v.FmtvTime),
                                           VechicleType = v.FmtvVechicleType
                                       }).ToArray();

            var DailyIns = (from d in _context.RmFormTDailyInspection where d.FmtdiFmtPkRefNo == pkrefno && d.FmtdiPkRefNo < headerid select d.FmtdiPkRefNo).DefaultIfEmpty();

            result.TotalPC = (from v in _context.RmFormTVechicle
                              where DailyIns.Contains(v.FmtvFmtdiPkRefNo) && v.FmtvVechicleType == "PC"
                              select v.FmtvCount).Sum();

            result.TotalHV = (from v in _context.RmFormTVechicle
                              where DailyIns.Contains(v.FmtvFmtdiPkRefNo) && v.FmtvVechicleType == "HV"
                              select v.FmtvCount).Sum();

            result.TotalMC = (from v in _context.RmFormTVechicle
                              where DailyIns.Contains(v.FmtvFmtdiPkRefNo) && v.FmtvVechicleType == "MC"
                              select v.FmtvCount).Sum();



            return result;

        }
        public async Task<List<FormUCUAHeaderRequestDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions)
        {
            //var query = (from hdr in _context.RmFormTHdr.Where(s => s.FmtActiveYn == true)
            //             join r in _context.RmRoadMaster on hdr.FmtRdCode equals r.RdmRdCode
            //             let DailyIns = (from d in _context.RmFormTDailyInspection where d.FmtdiFmtPkRefNo == hdr.FmtPkRefNo select d.FmtdiPkRefNo).DefaultIfEmpty()
            //             let vehicle = _context.RmFormTVechicle.Where(r => DailyIns.Contains(r.FmtvFmtdiPkRefNo)).DefaultIfEmpty()
 var query = (from hdr in _context.RmUcua.Where(s => s.RmmhActiveYn == true)
              select new
                         {
                             RefNo = hdr.RmmhPkRefNo,
                             RefId = hdr.RmmhRefId,
                             ReportingName = hdr.RmmhReportingName,
                             Location = hdr.RmmhLocation,
                             WorkScope = hdr.RmmhWorkScope,
                             Date = hdr.RmmhDateReceived,
                             Status = hdr.RmmhStatus,
                             SubmitSts = hdr.RmmhSubmitYn,

                         });



            query = query.OrderByDescending(x => x.RefNo);
            var search = filterOptions.Filters;

            if (!string.IsNullOrEmpty(search.Location))
            {
                query = query.Where(s => s.Location == search.Location);
            }
            if (!string.IsNullOrEmpty(search.WorkScope))
            {
                query = query.Where(s => s.WorkScope == search.WorkScope);
            }

            if (!string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtFrom) && !string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtTo))
            {
                DateTime dtFrom, dtTo;
                DateTime.TryParseExact(filterOptions.Filters.ReceivedDtFrom, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtFrom);
                DateTime.TryParseExact(filterOptions.Filters.ReceivedDtTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtTo);

                {
                    query = query.Where(x => x.Date.HasValue ? x.Date >= dtFrom && x.Date <= dtTo : false);
                }
            }


            if (!string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtFrom) && string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtTo))
            {
                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.ReceivedDtTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(x => x.Date.HasValue ? (x.Date.Value.Year == dt.Year && x.Date.Value.Month == dt.Month && x.Date.Value.Day == dt.Day) : false);
                }
            }

            if (string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtFrom) && !string.IsNullOrEmpty(filterOptions.Filters.ReceivedDtTo))
            {
                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.ReceivedDtTo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(x => x.Date.HasValue ? (x.Date.Value.Year == dt.Year && x.Date.Value.Month == dt.Month && x.Date.Value.Day == dt.Day) : false);
                }
            }

            if (!string.IsNullOrEmpty(search.SmartSearch))
            {

                DateTime dt;
                if (DateTime.TryParseExact(search.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
                    (s.RefId.Contains(search.SmartSearch) ||
                    s.ReportingName.Contains(search.SmartSearch) ||
                    s.Location.Contains(search.SmartSearch) ||
                    s.WorkScope.Contains(search.SmartSearch)) ||
                    s.Status.Contains(search.SmartSearch) ||

                    (s.Date.HasValue ? (s.Date.Value.Year == dt.Year && s.Date.Value.Month == dt.Month && s.Date.Value.Day == dt.Day) : true) && s.Date != null);
                }
                else
                {
                    query = query.Where(s =>
                     (s.RefId.Contains(search.SmartSearch) ||
                      s.ReportingName.Contains(search.SmartSearch) ||
                    s.Location.Contains(search.SmartSearch) ||
                    s.WorkScope.Contains(search.SmartSearch)) ||
                    s.Status.Contains(search.SmartSearch)

                     );
                }

            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.RefNo);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.ReportingName);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.Location);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.WorkScope);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.Date);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(s => s.Status);

            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.RefNo);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.ReportingName);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.Location);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.WorkScope);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.Date);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(s => s.Status);

            }




            var list = await query.Skip(filterOptions.StartPageNo)
  .Take(filterOptions.RecordsPerPage)
  .ToListAsync();

            return list.Select(s => new FormUCUAHeaderRequestDTO
            {
                PkRefNo=s.RefNo,
                RefId=s.RefId,
                Location=s.Location,
                WorkScope=s.WorkScope,
                Date = s.Date,
                Status = s.Status,
              SubmitSts = s.SubmitSts


            }).ToList();
        }
        public int? DeleteFormUCUA(int id)
        {
            try
            {

                var res = _context.Set<RmUcua>().FindAsync(id);
                res.Result.RmmhActiveYn = false;
                _context.Set<RmUcua>().Attach(res.Result);
                _context.Entry<RmUcua>(res.Result).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;

            }
            catch (Exception ex)
            {
                return 500;
            }
        }
    }
}
