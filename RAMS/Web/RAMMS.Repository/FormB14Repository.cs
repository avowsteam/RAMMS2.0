using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAMMS.Common;
using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository
{

    public class FormB14Repository : RepositoryBase<RmB14Hdr>, IFormB14Repository
    {
        public FormB14Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmB14Hdr
                         let max = _context.RmB14Hdr.Select(s => s.B14hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B14hPkRefNo,
                             RMU = hdr.B14hRmuCode,
                             RevisionYear = hdr.B14hRevisionYear,
                             RevisionNo = hdr.B14hRevisionNo,
                             IssueDate = hdr.B14hRevisionDate,
                             MaxRecord = (hdr.B14hPkRefNo == max)
                         });

            if (searchData.filter != null)
            {
                foreach (var item in searchData.filter.Where(x => !string.IsNullOrEmpty(x.Value)))
                {
                    string strVal = Utility.ToString(item.Value).Trim();
                    switch (item.Key)
                    {
                        case "KeySearch":
                            DateTime? dtSearch = Utility.ToDateTime(strVal);
                            query = query.Where(x =>
                                 (x.RevisionYear.HasValue ? x.RevisionYear.Value.ToString() : "").Contains(strVal)
                                 || (x.RevisionNo.HasValue ? x.RevisionNo.Value.ToString() : "").Contains(strVal)
                                 || (x.IssueDate.HasValue && ((x.IssueDate.Value.ToString().Contains(strVal)) || (dtSearch.HasValue && x.IssueDate == dtSearch)))
                                 || (x.RMU ?? "").Contains(strVal)
                                 );
                            break;
                        case "fromIssuDate":
                            DateTime? dtFrom = Utility.ToDateTime(strVal);
                            string toDate = Utility.ToString(searchData.filter["fromIssuDate"]);
                            if (toDate == "")
                                query = query.Where(x => x.IssueDate >= dtFrom);
                            else
                            {
                                DateTime? dtTo = Utility.ToDateTime(toDate);
                                query = query.Where(x => x.IssueDate >= dtFrom && x.IssueDate <= dtTo);
                            }
                            break;
                        case "toIssuDate":
                            string frmDate = Utility.ToString(searchData.filter["toIssuDate"]);
                            if (frmDate == "")
                            {
                                DateTime? dtTo = Utility.ToDateTime(strVal);
                                query = query.Where(x => x.IssueDate <= dtTo);
                            }
                            break;
                        case "Year":
                            int iFYr = Utility.ToInt(strVal);
                            query = query.Where(x => x.RevisionYear.HasValue && x.RevisionYear == iFYr);
                            break;
                        case "RMU":
                            query = query.Where(x => x.RMU == strVal);
                            break;
                        default:
                            query = query.WhereEquals(item.Key, strVal);
                            break;
                    }
                }

            }
            GridWrapper<object> grid = new GridWrapper<object>();
            grid.recordsTotal = await query.CountAsync();
            grid.recordsFiltered = grid.recordsTotal;
            grid.draw = searchData.draw;
            grid.data = await query.Order(searchData, query.OrderByDescending(s => s.RefNo)).Skip(searchData.start)
                                .Take(searchData.length)
                                .ToListAsync(); ;

            return grid;
        }

        public async Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var max = _context.RmB14Hdr.Count() > 0 ? _context.RmB14Hdr.Select(s => s.B14hPkRefNo).DefaultIfEmpty().Max() : 0;

            var query = (from hdr in _context.RmB14Hdr
                         where hdr.B14hPkRefNo == max
                         let history = _context.RmB14History.FirstOrDefault(x => x.B14hhB14hPkRefNo == max)
                         select new FormAWPBDTO
                         {
                             RefNo = hdr.B14hPkRefNo,
                             RMU = hdr.B14hRmuCode,
                             Feature = history.B14hhFeature ,
                             ActivityCode = history.B14hhActCode ,
                             ActivityName = history.B14hhActName,
                             Jan = history.B14hhJan ,
                             Feb = history.B14hhFeb ,
                             Mar = history.B14hhMar,
                             Apr = history.B14hhApr,
                             May = history.B14hhMay,
                             Jun = history.B14hhJun,
                             Jul = history.B14hhJul,
                             Aug = history.B14hhAug,
                             Sep = history.B14hhSep,
                             Oct = history.B14hhOct,
                             Nov = history.B14hhNov,
                             Dec = history.B14hhDec,
                             SubTotal = history.B14hhSubTotal,
                             Unit = history.B14hhUnitOfService
                         });


            GridWrapper<object> grid = new GridWrapper<object>();
            grid.recordsTotal = await query.CountAsync();
            grid.recordsFiltered = grid.recordsTotal;
            grid.draw = searchData.draw;
            grid.data = await query.Order(searchData, query.OrderByDescending(s => s.RefNo)).Skip(searchData.start)
                                .Take(searchData.length)
                                .ToListAsync(); ;

            return grid;
        }

        public RmB14Hdr GetHeaderById(int id, bool view)
        {
            RmB14Hdr res = (from r in _context.RmB14Hdr where r.B14hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB14Hdr
                          where rn.B14hRevisionYear == res.B14hRevisionYear && rn.B14hRmuCode == res.B14hRmuCode
                          select rn.B14hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (view == false)
                res.B14hRevisionNo = RevNo;
            res.RmB14History = (from r in _context.RmB14History where r.B14hhB14hPkRefNo == id select r).OrderBy(S => S.B14hhPkRefNo).ToList();

            return res;
        }

        public int? GetMaxRev(int Year, string RmuCode)
        {
            int? rev = (from rn in _context.RmB14Hdr where rn.B14hRevisionYear == Year && rn.B14hRmuCode == RmuCode select rn.B14hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB14(RmB14Hdr FormB14)
        {
            try
            {

                _context.RmB14Hdr.Add(FormB14);
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<FormB14Rpt> GetReportData(int headerid)
        {
            FormB14Rpt details = new FormB14Rpt();


            return details;
        }
    }
}
