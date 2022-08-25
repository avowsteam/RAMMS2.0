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

            //query.Select(s => s.B7dsPkRefNo).DefaultIfEmpty().Max();

            var query = (from hdr in _context.RmB7Hdr
                         let max = _context.RmB7Hdr.Select(s => s.B7hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B7hPkRefNo,
                             RevisionYear = hdr.B7hRevisionYear,
                             RevisionNo = hdr.B7hRevisionNo,
                             RevisionDate = hdr.B7hRevisionDate,
                             CrByName = hdr.B7hCrByName,
                             MaxRecord = (hdr.B7hPkRefNo == max)
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

        public RmB7Hdr GetHeaderById(int id, bool view)
        {
            RmB7Hdr res = (from r in _context.RmB7Hdr where r.B7hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == res.B7hRevisionYear select rn.B7hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (view == false)
                res.B7hRevisionNo = RevNo;
            res.RmB7LabourHistory = (from r in _context.RmB7LabourHistory where r.B7lhB7hPkRefNo == id select r).OrderBy(S => S.B7lhCode).ToList();
            res.RmB7MaterialHistory = (from r in _context.RmB7MaterialHistory where r.B7mhB7hPkRefNo == id select r).OrderBy(S => S.B7mhCode).ToList();
            res.RmB7EquipmentsHistory = (from r in _context.RmB7EquipmentsHistory where r.B7ehB7hPkRefNo == id select r).OrderBy(S => S.B7ehCode).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == Year select rn.B7hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB7(RmB7Hdr FormB7)
        {
            try
            {


                _context.RmB7Hdr.Add(FormB7);
                _context.SaveChanges();

                //foreach (var item in FormB7.RmB7LabourHistory.ToList())
                //{
                //    item.B7lhB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7LabourHistory.Add(item);
                //    _context.SaveChanges();
                //}

                //foreach (var item in FormB7.RmB7MaterialHistory.ToList())
                //{
                //    item.B7mhB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7MaterialHistory.Add(item);
                //    _context.SaveChanges();
                //}

                //foreach (var item in FormB7.RmB7EquipmentsHistory.ToList())
                //{
                //    item.B7ehB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7EquipmentsHistory.Add(item);
                //    _context.SaveChanges();
                //}

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<FormB7Rpt> GetReportData(int headerid)
        {
            FormB7Rpt details = new FormB7Rpt();

            details.Year = _context.RmB7Hdr.Where(x => x.B7hPkRefNo == headerid).Select(x => x.B7hRevisionYear).FirstOrDefault();

            details.Labours = await (from o in _context.RmB7LabourHistory
                                     where (o.B7lhB7hPkRefNo == headerid)
                                     orderby o.B7lhCode ascending
                                     select new Details
                                     {
                                         Code = o.B7lhCode,
                                         Name = o.B7lhName,
                                         Unit = o.B7lhUnitInHrs.ToString(),
                                         UnitPriceBatuNiah = o.B7lhUnitPriceBatuNiah.ToString(),
                                         UnitPriceMiri = o.B7lhUnitPriceMiri.ToString(),
                                     }).ToListAsync();

            details.Materials = await (from o in _context.RmB7MaterialHistory
                                       where (o.B7mhB7hPkRefNo == headerid)
                                       orderby o.B7mhCode ascending
                                       select new Details
                                       {
                                           Code = o.B7mhCode,
                                           Name = o.B7mhName,
                                           Unit = o.B7mhUnits.ToString(),
                                           UnitPriceBatuNiah = o.B7mhUnitPriceBatuNiah.ToString(),
                                           UnitPriceMiri = o.B7mhUnitPriceMiri.ToString(),
                                       }).ToListAsync();

            details.Equipments = await (from o in _context.RmB7EquipmentsHistory
                                        where (o.B7ehB7hPkRefNo == headerid)
                                        orderby o.B7ehCode ascending
                                        select new Details
                                        {
                                            Code = o.B7ehCode,
                                            Name = o.B7ehName,
                                            Unit = o.B7ehUnitInHrs.ToString(),
                                            UnitPriceBatuNiah = o.B7ehUnitPriceBatuNiah.ToString(),
                                            UnitPriceMiri = o.B7ehUnitPriceMiri.ToString(),
                                        }).ToListAsync();
            return details;
        }
    }
}
