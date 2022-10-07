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

            var query = (from hdr in _context.RmB12Hdr where hdr.B12hActiveYn == true
                         let max = _context.RmB12Hdr.Select(s => s.B12hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B12hPkRefNo,
                             ReferenceNo = hdr.B12hPkRefId ,
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
            //int? RevNo = (from rn in _context.RmB12Hdr where rn.B12hRevisionYear == res.B12hRevisionYear select rn.B12hRevisionNo).DefaultIfEmpty().Max() + 1;
            //if (view == false)
            //    res.B12hRevisionNo = RevNo;
            res.RmB12DesiredServiceLevelHistory  = (from r in _context.RmB12DesiredServiceLevelHistory where r.B12dslhB12hPkRefNo == id select r).OrderBy(S => S.B12dslhOrder).ToList();
           

            return res;
        }

        public async Task<RmB12Hdr> FindDetails(RmB12Hdr frmB12)
        {
            return await _context.RmB12Hdr.Include(x => x.RmB12DesiredServiceLevelHistory).ThenInclude(x => x.B12dslhB12hPkRefNoNavigation).Where(x =>  x.B12hRevisionYear == frmB12.B12hRevisionYear && x.B12hRevisionDate == frmB12.B12hRevisionDate && x.B12hRevisionNo == frmB12.B12hRevisionNo && x.B12hActiveYn == true).FirstOrDefaultAsync();
        }


        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB12Hdr where rn.B12hRevisionYear == Year select rn.B12hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<RmB12Hdr> Save(RmB12Hdr frmB12, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmB12.B12hPkRefNo == 0)
            {
                isAdd = true;
                frmB12.B12hActiveYn = true;
                _context.RmB12Hdr.Add(frmB12);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "B12hPkRefNo",
                    "B12hRevisionYear"
                };
    
                _context.RmB12Hdr.Attach(frmB12);

                var entry = _context.Entry(frmB12);
                entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                {
                    p.IsModified = true;
                });
                if (updateSubmit)
                {
                    entry.Property(x => x.B12hSubmitSts).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();
            if (isAdd)
            {
                
                await _context.SaveChangesAsync();
            }
            return frmB12;
        }

        public async Task<int> SaveFormB12(List<RmB12DesiredServiceLevelHistory> FormB12)
        {
            try
            {
                if (FormB12[0].B12dslhPkRefNo == 0)
                {
                    //_context.RmB12DesiredServiceLevelHistory.RemoveRange()
                    var res =  _context.RmB12DesiredServiceLevelHistory.Where(x => x.B12dslhB12hPkRefNo == FormB12[0].B12dslhB12hPkRefNo);
                    _context.RemoveRange(res);
                    _context.SaveChanges();
                    _context.RmB12DesiredServiceLevelHistory.AddRange(FormB12);
                }
                
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<FormB12Rpt> GetReportData(int headerid)
        {
            FormB12Rpt details = new FormB12Rpt();

            details.Year = _context.RmB12Hdr.Where(x => x.B12hPkRefNo == headerid).Select(x => x.B12hRevisionYear).FirstOrDefault();

            details.B12History  = await (from o in _context.RmB12DesiredServiceLevelHistory
                                     where (o.B12dslhB12hPkRefNo == headerid)
                                     orderby o.B12dslhOrder ascending
                                     select new B12History
                                     {
                                         Feature = o.B12dslhFeature,
                                         Code = o.B12dslhActCode,
                                         Name = o.B12dslhActName,
                                         Unit = o.B12dslhUnitOfService.ToString(),
                                         UnitPriceBatuNiah = o.B12dslhRmuBatuniah.ToString(),
                                         UnitPriceMiri = o.B12dslhRmuMiri.ToString(),
                                     }).ToListAsync();

           
            return details;
        }

        public int DeleteHeader(RmB12Hdr frmB12)
        {
            _context.RmB12Hdr.Attach(frmB12);
            var entry = _context.Entry(frmB12);
            entry.Property(x => x.B12hActiveYn).IsModified = true;
            _context.SaveChanges();
            return frmB12.B12hPkRefNo;
        }

        public List<RmB12DesiredServiceLevelHistory> GetHistoryData(int year)
        {
            List<RmB12DesiredServiceLevelHistory> res = (from r in _context.RmB12DesiredServiceLevelHistory where r.B12dslhB12hPkRefNo == year select r).OrderBy(x => x.B12dslhOrder).ToList();
            return res;
        }

        public List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetDataMiri(int year)
        {

            var list = _context.RmB13ProposedPlannedBudget.Where(x => x.B13pRmu == "Miri" &&  x.B13pRevisionYear == year && x.B13pSubmitSts == true).OrderByDescending(x => x.B13pPkRefNo).ToList();
            
            List<RmB13ProposedPlannedBudgetHistory> res = new List<RmB13ProposedPlannedBudgetHistory>();
            if (list.Count > 0)
                res = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == list[0].B13pPkRefNo select r).ToList();
            return res;
        }

        public List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetDataBTN(int year)
        {

            var list = _context.RmB13ProposedPlannedBudget.Where(x => x.B13pRmu == "Batu Niah" && x.B13pRevisionYear == year && x.B13pSubmitSts == true).OrderByDescending(x => x.B13pPkRefNo).ToList();

            List<RmB13ProposedPlannedBudgetHistory> res = new List<RmB13ProposedPlannedBudgetHistory>();
            if (list.Count > 0)
                res = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == list[0].B13pPkRefNo select r).ToList();
            return res;
        }

        public List<RmB10DailyProductionHistory> GetUnitData(int year)
        {
            var list = _context.RmB10DailyProduction.Where(x => x.B10dpRevisionYear == year).OrderByDescending(x => x.B10dpPkRefNo).ToList();
            List<RmB10DailyProductionHistory> res = new List<RmB10DailyProductionHistory>();
            if (list.Count > 0)
                res = (from r in _context.RmB10DailyProductionHistory where r.B10dphB10dpPkRefNo == list[0].B10dpPkRefNo select r).ToList();
            return res;
        }


    }
}
