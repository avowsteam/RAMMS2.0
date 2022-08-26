using Microsoft.EntityFrameworkCore;
using RAMMS.Common;
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
    public class FormB9Repository : RepositoryBase<RmB9DesiredService>, IFormB9Repository
    {
        public FormB9Repository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormB9ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB9SearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmB9DesiredService

                         select new { x });


            query = query.OrderByDescending(x => x.x.B9dsPkRefNo);

            if (filterOptions.Filters.Year != null && filterOptions.Filters.Year != string.Empty)
            {
                query = query.Where(s => s.x.B9dsRevisionYear == Convert.ToInt32(filterOptions.Filters.Year));
            }


            string frmDate = Utility.ToString(filterOptions.Filters.FromDate);
            string toDate = Utility.ToString(filterOptions.Filters.ToDate);

            DateTime? dtFrom = Utility.ToDateTime(frmDate);
            DateTime? dtTo = Utility.ToDateTime(toDate);
            if (toDate == "" && frmDate != "")
                query = query.Where(s => s.x.B9dsRevisionDate >= dtFrom);
            else if (toDate != "" && frmDate != "")
                query = query.Where(s => s.x.B9dsRevisionDate >= dtFrom && s.x.B9dsRevisionDate <= dtTo);
            else if (frmDate == "" && toDate != "")
                query = query.Where(s => s.x.B9dsRevisionDate <= dtTo);





            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {


                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
                    s.x.B9dsUserName.Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B9dsRevisionNo.HasValue ? s.x.B9dsRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B9dsRevisionYear.HasValue ? s.x.B9dsRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B9dsRevisionDate.HasValue ? (s.x.B9dsRevisionDate.Value.Year == dt.Year && s.x.B9dsRevisionDate.Value.Month == dt.Month && s.x.B9dsRevisionDate.Value.Day == dt.Day) : true) && s.x.B9dsRevisionDate != null);
                }
                else
                {
                    query = query.Where(s =>
                   s.x.B9dsUserName.Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B9dsRevisionNo.HasValue ? s.x.B9dsRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B9dsRevisionYear.HasValue ? s.x.B9dsRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }

            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.x.B9dsRevisionYear);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.B9dsRevisionNo);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.B9dsRevisionDate);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.B9dsUserName);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.B9dsRevisionYear);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.B9dsRevisionNo);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.B9dsRevisionDate);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.B9dsUserName);

            }

            int? MaxRecord = 0;
            if (query.Count() > 0)
                MaxRecord = query.Select(s => s.x.B9dsPkRefNo).DefaultIfEmpty().Max();

            var list = query.Select(s => new FormB9ResponseDTO
            {
                PkRefNo = s.x.B9dsPkRefNo,
                RevisionDate = s.x.B9dsRevisionDate,
                RevisionNo = s.x.B9dsRevisionNo,
                RevisionYear = s.x.B9dsRevisionYear,
                UserId = s.x.B9dsUserId,
                UserName = s.x.B9dsUserName,
                MaxRecord = (s.x.B9dsPkRefNo == MaxRecord)
            }).ToList();



            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }


        public async Task<List<FormB9HistoryResponseDTO>> GetFormB9HistoryGridList(FilteredPagingDefinition<FormB9HistoryResponseDTO> filterOptions)
        {
            List<FormB9HistoryResponseDTO> result = new List<FormB9HistoryResponseDTO>();

            var query = from x in _context.RmB9DesiredServiceHistory
                        where x.B9dshB9dsPkRefNo == filterOptions.Filters.B9dsPkRefNo
                        orderby x.B9dshPkRefNo descending
                        select new { x };


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderBy(x => x.x.B9dshFeature);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(x => x.x.B9dshCode);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(x => x.x.B9dshName);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(x => x.x.B9dshCond1);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(x => x.x.B9dshCond2);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {

                if (filterOptions.ColumnIndex == 1)
                    query = query.OrderByDescending(x => x.x.B9dshFeature);
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(x => x.x.B9dshCode);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(x => x.x.B9dshName);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(x => x.x.B9dshCond1);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(x => x.x.B9dshCond2);

            }

            var list = await query.ToListAsync();


            return list.Select(s => new FormB9HistoryResponseDTO
            {
                B9dsPkRefNo = s.x.B9dshPkRefNo,
                Code = s.x.B9dshCode,
                Cond1 = s.x.B9dshCond1,
                Cond2 = s.x.B9dshCond2,
                Cond3 = s.x.B9dshCond3,
                Feature = s.x.B9dshFeature,
                Name = s.x.B9dshName,
                Remarks = s.x.B9dshRemarks,
                RevisionDate = s.x.B9dshRevisionDate,
                RevisionNo = s.x.B9dshRevisionNo,
                UnitOfServiceId = s.x.B9dshUnitOfServiceId,
                UnitDescription= s.x.B9dshUnitDescription,
                UserId = s.x.B9dshUserId,
                UserName = s.x.B9dshUserName
            }).ToList();

        }


        public RmB9DesiredService GetHeaderById(int id)
        {
            RmB9DesiredService res = (from r in _context.RmB9DesiredService where r.B9dsPkRefNo == id select r).FirstOrDefault();

            //int? RevNo = (from rn in _context.RmB9DesiredService where rn.B9dsRevisionYear == res.B9dsRevisionYear select rn.B9dsRevisionNo).DefaultIfEmpty().Max() + 1;

            //res.B9dsRevisionNo = RevNo;

            res.RmB9DesiredServiceHistory = (from r in _context.RmB9DesiredServiceHistory
                                             where r.B9dshB9dsPkRefNo == id
                                             select new RmB9DesiredServiceHistory
                                             {
                                                 B9dshB9dsPkRefNo = r.B9dshB9dsPkRefNo,
                                                 B9dshCode = r.B9dshCode,
                                                 B9dshCond1 = r.B9dshCond1,
                                                 B9dshCond2 = r.B9dshCond2,
                                                 B9dshCond3 = r.B9dshCond3,
                                                 B9dshFeature = r.B9dshFeature,
                                                 B9dshName = r.B9dshName,
                                                 B9dshPkRefNo = r.B9dshPkRefNo,
                                                 B9dshRemarks = r.B9dshRemarks,
                                                 B9dshUnitDescription = r.B9dshUnitDescription,
                                                 B9dshUnitOfServiceId = r.B9dshUnitOfServiceId,
                                                 B9dshUnitOfService = (from s in _context.RmDdLookup where s.DdlType == "UnitServiceLevel" && s.DdlTypeValue == Convert.ToString(r.B9dshUnitOfServiceId) select s.DdlTypeDesc).FirstOrDefault()
                                             }).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB9DesiredService where rn.B9dsRevisionYear == Year select rn.B9dsRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB9(RmB9DesiredService FormB9, List<RmB9DesiredServiceHistory> FormB9History)
        {
            try
            {

                _context.RmB9DesiredService.Add(FormB9);
                _context.SaveChanges();

                foreach (var item in FormB9History)
                {
                    item.B9dshB9dsPkRefNo = FormB9.B9dsPkRefNo;
                    item.B9dshPkRefNo = 0;
                    _context.RmB9DesiredServiceHistory.Add(item);
                    _context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        //public async Task<RmB9DesiredServiceHistory> GetReportData(int headerid)
        //{
        //    FORMB9Rpt result = (from s in _context.RmFormB9Hdr
        //                        where s.Ff1hPkRefNo == headerid && s.Ff1hActiveYn == true
        //                        select new FORMB9Rpt
        //                        {
        //                            CrewLeader = s.Ff1hCrewName,
        //                            District = s.Ff1hDist,
        //                            InspectedByDesignation = s.Ff1hInspectedDesg,
        //                            InspectedByName = s.Ff1hInspectedName,
        //                            InspectedDate = s.Ff1hInspectedDate,
        //                            Division = s.Ff1hDivCode,
        //                            RMU = (from r in _context.RmDdLookup where r.DdlType == "RMU" && r.DdlTypeCode == s.Ff1hRmuCode select r.DdlTypeDesc).FirstOrDefault(),
        //                            RoadCode = s.Ff1hRdCode,
        //                            RoadName = s.Ff1hRdName,
        //                            RoadLength = s.Ff1hRoadLength
        //                        }).FirstOrDefault();


        //    result.Details = (from d in _context.RmFormB9Dtl
        //                      join a in _context.RmAllassetInventory on d.Ff1dAssetId equals a.AiAssetId
        //                      where d.Ff1dFf1hPkRefNo == headerid
        //                      orderby d.Ff1dPkRefNo descending
        //                      select new FORMB9RptDetail
        //                      {

        //                          Descriptions = d.Ff1dDescription,
        //                          LocationChKm = a.AiLocChKm,
        //                          LocationChM = a.AiLocChM == "" ? 0 : Convert.ToInt32(a.AiLocChM),
        //                          Length = Convert.ToDecimal(a.AiLength),
        //                          Width = Convert.ToDecimal(a.AiWidth),
        //                          BottomWidth = Convert.ToDecimal(a.AiBotWidth),
        //                          Height = Convert.ToDecimal(a.AiHeight),
        //                          Condition = d.Ff1dOverallCondition,
        //                          Tier = Convert.ToInt32(a.AiTier),
        //                          StructCode = a.AiStrucCode
        //                      }).ToArray();
        //    return result;

        //}


    }
}
