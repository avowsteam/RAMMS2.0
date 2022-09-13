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
            var query = (from hdr in _context.RmB14Hdr.Where(x => x.B14hActiveYn == true)
                         let max = _context.RmB14Hdr.Select(s => s.B14hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B14hPkRefNo,
                             RMU = hdr.B14hRmuCode,
                             RevisionYear = hdr.B14hRevisionYear,
                             RevisionNo = hdr.B14hRevisionNo,
                             IssueDate = hdr.B14hRevisionDate,
                             Status = hdr.B14hStatus,
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

        public RmB14Hdr GetHeaderById(int id, bool view)
        {
            RmB14Hdr res = (from r in _context.RmB14Hdr where r.B14hPkRefNo == id select r).FirstOrDefault();
            //int? RevNo = (from rn in _context.RmB14Hdr
            //              where rn.B14hRevisionYear == res.B14hRevisionYear && rn.B14hRmuCode == res.B14hRmuCode
            //              select rn.B14hRevisionNo).DefaultIfEmpty().Max() + 1;
            //if (view == false)
            //    res.B14hRevisionNo = RevNo;
            res.RmB14History = (from r in _context.RmB14History where r.B14hhB14hPkRefNo == id select r).OrderBy(S => S.B14hhOrder).ToList();

            return res;
        }

        public int DeleteHeader(RmB14Hdr frmB14)
        {
            _context.RmB14Hdr.Attach(frmB14);
            var entry = _context.Entry(frmB14);
            entry.Property(x => x.B14hActiveYn).IsModified = true;
            _context.SaveChanges();
            return frmB14.B14hPkRefNo;
        }

        public bool isF1Exist(int id)
        {
            var rmF2dtl = _context.RmFormF1Dtl.FirstOrDefault(x => x.Ff1dR1hPkRefNo == id);
            if (rmF2dtl != null)
                return true;

            return false;
        }

        public int? GetMaxRev(int Year, string RmuCode)
        {
            int? rev = (from rn in _context.RmB14Hdr where rn.B14hRevisionYear == Year && rn.B14hRmuCode == RmuCode select rn.B14hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<RmB14Hdr> FindDetails(RmB14Hdr frmB14)
        {
            //return await _context.RmFormMHdr.Include(x => x.RmFormMAuditDetails).ThenInclude(x => x.FmadFmhPkRefNoNavigation).Where(x => x.Fr1hAssetId == frmR1R2.Fr1hAssetId && x.Fr1hYearOfInsp == frmR1R2.Fr1hYearOfInsp && x.Fr1hActiveYn == true).FirstOrDefaultAsync();
            return await _context.RmB14Hdr.Include(x => x.RmB14History).ThenInclude(x => x.B14hhB14hPkRefNoNavigation).Where(x => x.B14hRmuCode == frmB14.B14hRmuCode && x.B14hRevisionYear == frmB14.B14hRevisionYear && x.B14hRevisionDate == frmB14.B14hRevisionDate && x.B14hRevisionNo == frmB14.B14hRevisionNo && x.B14hActiveYn == true).FirstOrDefaultAsync();
        }

        public async Task<RmB14Hdr> Save(RmB14Hdr frmB14, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmB14.B14hPkRefNo == 0)
            {
                isAdd = true;
                frmB14.B14hActiveYn = true;
                IDictionary<string, string> lstRef = new Dictionary<string, string>();
                //lstRef.Add("Year", Utility.ToString(frmR1R2.Fr1hYearOfInsp));
                //lstRef.Add("AssetID", Utility.ToString(frmR1R2.Fr1hAssetId));
                //frmR1R2.FmhPkRefNo = Common.RefNumber.FormRefNumber.GetRefNumber(FormType.FormR1R2, lstRef);
                _context.RmB14Hdr.Add(frmB14);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "B14hPkRefNo",
                    "B14hRevisionYear", "B14hRmuName"
                };
                //_context.RmFormS1Dtl.Update(formS1Details);
                //var dtls = frmR1R2.RmFormR2Hdr;
                //frmR1R2.RmFormR2Hdr = null;
                _context.RmB14Hdr.Attach(frmB14);

                var entry = _context.Entry(frmB14);
                entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                {
                    p.IsModified = true;
                });
                if (updateSubmit)
                {
                    entry.Property(x => x.B14hSubmitSts).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();
            if (isAdd)
            {
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                //lstData.Add("RoadCode", frmB14.B14hRmuCode);
                ////lstData.Add("ActivityCode", frmR1R2.FmhActCode);
                ////lstData.Add("Date", Utility.ToString(frmR1R2.FmhAuditedDate, "YYYYMMDD"));
                //lstData.Add("Year", frmR1R2.FmhAuditedDate.Value.Year.ToString());
                //lstData.Add("MonthNo", frmR1R2.FmhAuditedDate.Value.Month.ToString());
                //lstData.Add("Day", frmR1R2.FmhAuditedDate.Value.Day.ToString());
                //lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(frmR1R2.FmhPkRefNo));
                //frmR1R2.FmhRefId = FormRefNumber.GetRefNumber(FormType.FormM, lstData);
                await _context.SaveChangesAsync();
            }
            return frmB14;
        }

        public async Task<int> SaveFormB14(List<RmB14History> FormB14)
        {
            try
            {
                if (FormB14[0].B14hhPkRefNoHistory == 0)
                    _context.RmB14History.AddRange(FormB14);
                else
                {
                    _context.RmB14History.UpdateRange(FormB14);
                    // string[] arrNotReqUpdate = new string[] { "B14hhPkRefNoHistory"

                    //};
                    // var entry = _context.Entry(FormB14);
                    // entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                    // {
                    //     p.IsModified = true;
                    // });
                    // _context.Entry(FormB14).State = EntityState.Modified;
                }
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

        public List<RmB14History> GetHistoryData(int year)
        {
            List<RmB14History> res = (from r in _context.RmB14History where r.B14hhB14hPkRefNo == year select r).OrderBy(x => x.B14hhOrder).ToList();
            return res;
        }
        public List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetData(string Rmucode, int year)
        {
            if (Rmucode.ToUpper() == "MRI")
                Rmucode = "Miri";
            var list = _context.RmB13ProposedPlannedBudget.Where(x => x.B13pRmu == Rmucode && x.B13pRevisionYear == year && x.B13pSubmitSts == true).OrderByDescending(x => x.B13pPkRefNo).ToList();
            List<RmB13ProposedPlannedBudgetHistory> res = new List<RmB13ProposedPlannedBudgetHistory>();
            if (list.Count > 0)
                res = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == list[0].B13pPkRefNo select r).ToList();
            return res;
        }

        public async Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var max = _context.RmB14Hdr.Count() > 0 ? _context.RmB14Hdr.Select(s => s.B14hPkRefNo).DefaultIfEmpty().Max() : 0;

            var query = (from history in _context.RmB14History
                         where history.B14hhB14hPkRefNo == max
                         let hdr = _context.RmB14Hdr.FirstOrDefault(x => x.B14hPkRefNo == max)
                         select new FormAWPBDTO
                         {
                             RefNo = max,
                             RMU = hdr.B14hRmuCode,
                             Feature = history.B14hhFeature,
                             ActivityCode = history.B14hhActCode,
                             ActivityName = history.B14hhActName,
                             Jan = history.B14hhJan,
                             Feb = history.B14hhFeb,
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
    }
}
