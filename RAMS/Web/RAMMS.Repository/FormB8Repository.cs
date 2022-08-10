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

    public class FormB8Repository : RepositoryBase<RmB8Hdr>, IFormB8Repository
    {
        public FormB8Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmB8Hdr

                         select new
                         {
                             RefNo = hdr.B8hPkRefNo,
                             RevisionYear = hdr.B8hRevisionYear,
                             RevisionNo = hdr.B8hRevisionNo,
                             RevisionDate = hdr.B8hRevisionDate,
                             CrByName = hdr.B8hCrByName,
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
                                 || (x.RevisionDate.HasValue && ((x.RevisionDate.Value.ToString().Contains(strVal)) || (dtSearch.HasValue && x.RevisionDate == dtSearch)))
                                 || (x.CrByName ?? "").Contains(strVal)
                                 );
                            break;
                        case "fromRevDate":
                            DateTime? dtFrom = Utility.ToDateTime(strVal);
                            string toDate = Utility.ToString(searchData.filter["toRevDate"]);
                            if (toDate == "")
                                query = query.Where(x => x.RevisionDate >= dtFrom);
                            else
                            {
                                DateTime? dtTo = Utility.ToDateTime(toDate);
                                query = query.Where(x => x.RevisionDate >= dtFrom && x.RevisionDate <= dtTo);
                            }
                            break;
                        case "toRevDate":
                            string frmDate = Utility.ToString(searchData.filter["fromRevDate"]);
                            if (frmDate == "")
                            {
                                DateTime? dtTo = Utility.ToDateTime(strVal);
                                query = query.Where(x => x.RevisionDate <= dtTo);
                            }
                            break;

                        case "Year":
                            int iFYr = Utility.ToInt(strVal);
                            query = query.Where(x => x.RevisionYear.HasValue && x.RevisionYear == iFYr);
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

        public RmB8Hdr GetHeaderById(int id)
        {
            RmB8Hdr res = (from r in _context.RmB8Hdr where r.B8hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB8Hdr where rn.B8hRevisionYear == res.B8hRevisionYear select rn.B8hRevisionNo).DefaultIfEmpty().Max() + 1;
            res.B8hRevisionNo = RevNo;
            res.RmB8History = (from r in _context.RmB8History where r.B8hiB8hPkRefNo == id select r).OrderBy(S => S.B8hiItemNo).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB8Hdr where rn.B8hRevisionYear == Year select rn.B8hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB8(RmB8Hdr FormB8)
        {
            try
            {


                _context.RmB8Hdr.Add(FormB8);
                _context.SaveChanges();

               

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<FormB8Rpt>> GetReportData(int headerid)
        {
            List<FormB8Rpt> details = new List<FormB8Rpt>();

            
                            
            details =await  (from o in _context.RmB8History
                              where (o.B8hiB8hPkRefNo == headerid)
                              orderby o.B8hiItemNo ascending
                              select new FormB8Rpt
                              {
                                  ItemNo = o.B8hiItemNo.ToString(),
                                  Description = o.B8hiDescription,
                                  Unit = o.B8hiUnit.ToString() ,
                                  Division = o.B8hiDivision,
                              }).ToListAsync();

            
            return details;
        }
    }
}
