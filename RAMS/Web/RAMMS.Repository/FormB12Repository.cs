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

    public class FormB12Repository : RepositoryBase<RmB12Hdr>, IFormB12Repository
    {
        public FormB12Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {

            //query.Select(s => s.B12dsPkRefNo).DefaultIfEmpty().Max();

            var query = (from hdr in _context.RmB12Hdr
                         let max = _context.RmB12Hdr.Select(s => s.B12hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B12hPkRefNo,
                             RevisionYear = hdr.B12hRevisionYear,
                             RevisionNo = hdr.B12hRevisionNo,
                             RevisionDate = hdr.B12hRevisionDate,
                             CrByName = hdr.B12hCrByName,
                             MaxRecord = (hdr.B12hPkRefNo == max)
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

        public RmB12Hdr GetHeaderById(int id, bool view)
        {
            RmB12Hdr res = (from r in _context.RmB12Hdr where r.B12hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB12Hdr where rn.B12hRevisionYear == res.B12hRevisionYear select rn.B12hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (view == false)
                res.B12hRevisionNo = RevNo;
            res.RmB12DesiredServiceLevelHistory  = (from r in _context.RmB12DesiredServiceLevelHistory where r.B12dslhB12hPkRefNo == id select r).OrderBy(S => S.B12dslhActCode).ToList();
           

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB12Hdr where rn.B12hRevisionYear == Year select rn.B12hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<int> SaveFormB12(RmB12Hdr FormB12)
        {
            try
            {


                _context.RmB12Hdr.Add(FormB12);
                _context.SaveChanges();

                

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //public async Task<FoRmB12Rpt> GetReportData(int headerid)
        //{
        //    FoRmB12Rpt details = new FoRmB12Rpt();

        //    details.Year = _context.RmB12Hdr.Where(x => x.B12hPkRefNo == headerid).Select(x => x.B12hRevisionYear).FirstOrDefault();

        //    details.Labours = await (from o in _context.RmB12LabourHistory
        //                             where (o.B12lhB12hPkRefNo == headerid)
        //                             orderby o.B12lhCode ascending
        //                             select new Details
        //                             {
        //                                 Code = o.B12lhCode,
        //                                 Name = o.B12lhName,
        //                                 Unit = o.B12lhUnitInHrs.ToString(),
        //                                 UnitPriceBatuNiah = o.B12lhUnitPriceBatuNiah.ToString(),
        //                                 UnitPriceMiri = o.B12lhUnitPriceMiri.ToString(),
        //                             }).ToListAsync();

        //    details.Materials = await (from o in _context.RmB12MaterialHistory
        //                               where (o.B12mhB12hPkRefNo == headerid)
        //                               orderby o.B12mhCode ascending
        //                               select new Details
        //                               {
        //                                   Code = o.B12mhCode,
        //                                   Name = o.B12mhName,
        //                                   Unit = o.B12mhUnits.ToString(),
        //                                   UnitPriceBatuNiah = o.B12mhUnitPriceBatuNiah.ToString(),
        //                                   UnitPriceMiri = o.B12mhUnitPriceMiri.ToString(),
        //                               }).ToListAsync();

        //    details.Equipments = await (from o in _context.RmB12EquipmentsHistory
        //                                where (o.B12ehB12hPkRefNo == headerid)
        //                                orderby o.B12ehCode ascending
        //                                select new Details
        //                                {
        //                                    Code = o.B12ehCode,
        //                                    Name = o.B12ehName,
        //                                    Unit = o.B12ehUnitInHrs.ToString(),
        //                                    UnitPriceBatuNiah = o.B12ehUnitPriceBatuNiah.ToString(),
        //                                    UnitPriceMiri = o.B12ehUnitPriceMiri.ToString(),
        //                                }).ToListAsync();
        //    return details;
        //}
    }
}
