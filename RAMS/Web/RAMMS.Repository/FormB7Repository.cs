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

    public class FormB7Repository : RepositoryBase<RmB7Hdr>, IFormB7Repository
    {
        public FormB7Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmB7Hdr

                         select new
                         {
                             RefNo = hdr.B7hPkRefNo,
                             RevisionYear = hdr.B7hRevisionYear ,
                             RevisionNo = hdr.B7hRevisionNo ,
                             RevisionDate = hdr.B7hRevisionDate ,
                             CrByName = hdr.B7hCrByName ,
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

        public  RmB7Hdr GetHeaderById(int id)
        {
            RmB7Hdr res = (from r in _context.RmB7Hdr where r.B7hPkRefNo == id select r).FirstOrDefault();

            res.RmB7LabourHistory = (from r in _context.RmB7LabourHistory where r.B7lhB7hPkRefNo == id select r).ToList();
            res.RmB7MaterialHistory = (from r in _context.RmB7MaterialHistory where r.B7mhB7hPkRefNo == id select r).ToList();
            res.RmB7EquipmentsHistory = (from r in _context.RmB7EquipmentsHistory where r.B7ehB7hPkRefNo == id select r).ToList();

            return res;
        }

    }
}
