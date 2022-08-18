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
    public class FormB10Repository : RepositoryBase<RmB10DailyProduction>, IFormB10Repository
    {
        public FormB10Repository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormB10ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB10SearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmB10DailyProduction

                         select new { x });


            query = query.OrderByDescending(x => x.x.B10dpPkRefNo);

            if (filterOptions.Filters.Year != null && filterOptions.Filters.Year != string.Empty)
            {
                query = query.Where(s => s.x.B10dpRevisionYear == Convert.ToInt32(filterOptions.Filters.Year));
            }

            string frmDate = Utility.ToString(filterOptions.Filters.FromDate);
            string toDate = Utility.ToString(filterOptions.Filters.ToDate);

            DateTime? dtFrom = Utility.ToDateTime(frmDate);
            DateTime? dtTo = Utility.ToDateTime(toDate);
            if (toDate == "" && frmDate != "")
                query = query.Where(s => s.x.B10dpRevisionDate >= dtFrom);
            else if (toDate != "" && frmDate != "")
                query = query.Where(s => s.x.B10dpRevisionDate >= dtFrom && s.x.B10dpRevisionDate <= dtTo);
            else if (frmDate == "" && toDate != "")
                query = query.Where(s => s.x.B10dpRevisionDate <= dtTo);


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {


                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
                    s.x.B10dpUserName.Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B10dpRevisionNo.HasValue ? s.x.B10dpRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B10dpRevisionYear.HasValue ? s.x.B10dpRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B10dpRevisionDate.HasValue ? (s.x.B10dpRevisionDate.Value.Year == dt.Year && s.x.B10dpRevisionDate.Value.Month == dt.Month && s.x.B10dpRevisionDate.Value.Day == dt.Day) : true) && s.x.B10dpRevisionDate != null);
                }
                else
                {
                    query = query.Where(s =>
                   s.x.B10dpUserName.Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B10dpRevisionNo.HasValue ? s.x.B10dpRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B10dpRevisionYear.HasValue ? s.x.B10dpRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }

            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.x.B10dpRevisionYear);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.B10dpRevisionNo);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.B10dpRevisionDate);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.B10dpUserName);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.B10dpRevisionYear);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.B10dpRevisionNo);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.B10dpRevisionDate);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.B10dpUserName);

            }

            int? MaxRecord = query.Select(s => s.x.B10dpPkRefNo).DefaultIfEmpty().Max();


            var list = query.Select(s => new FormB10ResponseDTO
            {
                PkRefNo = s.x.B10dpPkRefNo,
                RevisionDate = s.x.B10dpRevisionDate,
                RevisionNo = s.x.B10dpRevisionNo,
                RevisionYear = s.x.B10dpRevisionYear,
                UserId = s.x.B10dpUserId,
                UserName = s.x.B10dpUserName,
                MaxRecord = (s.x.B10dpPkRefNo == MaxRecord)
            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }



        public RmB10DailyProduction GetHeaderById(int id)
        {
            RmB10DailyProduction res = (from r in _context.RmB10DailyProduction where r.B10dpPkRefNo == id select r).FirstOrDefault();

            //int? RevNo = (from rn in _context.RmB10DailyProduction where rn.B10dpRevisionYear == res.B10dpRevisionYear select rn.B10dpRevisionNo).DefaultIfEmpty().Max() + 1;

            //res.B10dpRevisionNo = RevNo;

            res.RmB10DailyProductionHistory = (from r in _context.RmB10DailyProductionHistory
                                               where r.B10dphB10dpPkRefNo == id
                                               select new RmB10DailyProductionHistory
                                               {
                                                   B10dphB10dpPkRefNo = r.B10dphB10dpPkRefNo,
                                                   B10dphCode = r.B10dphCode,
                                                   B10dphAdpUnit = r.B10dphAdpUnit,
                                                   B10dphAdpUnitDescription = r.B10dphAdpUnitDescription,
                                                   B10dphAdpValue = r.B10dphAdpValue,
                                                   B10dphFeature = r.B10dphFeature,
                                                   B10dphName = r.B10dphName,
                                                   B10dphPkRefNo = r.B10dphPkRefNo,
                                               }).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB10DailyProduction where rn.B10dpRevisionYear == Year select rn.B10dpRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB10(RmB10DailyProduction FormB10, List<RmB10DailyProductionHistory> FormB10History)
        {
            try
            {

                _context.RmB10DailyProduction.Add(FormB10);
                _context.SaveChanges();

                foreach (var item in FormB10History)
                {
                    item.B10dphB10dpPkRefNo = FormB10.B10dpPkRefNo;
                    item.B10dphPkRefNo = 0;
                    _context.RmB10DailyProductionHistory.Add(item);
                    _context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



    }
}
