using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using RAMMS.Common;
using Microsoft.EntityFrameworkCore;
using RAMMS.Common.RefNumber;
using RAMMS.DTO.Report;
using RAMMS.DTO.ResponseBO;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO.RequestBO;

namespace RAMMS.Repository
{
    public class FormB11Repository : RepositoryBase<RmB11Hdr>, IFormB11Repository
    {
        public FormB11Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmB11Hdr

                         select new
                         {
                             RefNo = hdr.B11hPkRefNo,
                             RevisionYear = hdr.B11hRevisionYear,
                             RevisionNo = hdr.B11hRevisionNo,
                             RevisionDate = hdr.B11hRevisionDate,
                             CrByName = hdr.B11hCrByName,
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

        public RmB11Hdr GetHeaderById(int id)
        {
            RmB11Hdr res = (from r in _context.RmB11Hdr where r.B11hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB11Hdr where rn.B11hRevisionYear == res.B11hRevisionYear select rn.B11hRevisionNo).DefaultIfEmpty().Max() + 1;
            res.B11hRevisionNo = RevNo;
            res.RmB11CrewDayCostHeader = (from r in _context.RmB11CrewDayCostHeader where r.B11cdchB11hPkRefNo == id select r).ToList();
            res.RmB11LabourCost = (from r in _context.RmB11LabourCost where r.B11lcB11hPkRefNo == id select r).ToList();
            res.RmB11EquipmentCost = (from r in _context.RmB11EquipmentCost where r.B11ecB11hPkRefNo == id select r).ToList();
            res.RmB11MaterialCost = (from r in _context.RmB11MaterialCost where r.B11mcB11hPkRefNo == id select r).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB11Hdr where rn.B11hRevisionYear == Year select rn.B11hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public List<RmB7LabourHistory> GetLabourHistoryData(int year)
        {
            int? RefNo = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == year select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            List<RmB7LabourHistory> res = (from r in _context.RmB7LabourHistory where r.B7lhB7hPkRefNo == RefNo select r).OrderBy(x=>x.B7lhCode).ToList();            
            return res;
        }
    }
}
