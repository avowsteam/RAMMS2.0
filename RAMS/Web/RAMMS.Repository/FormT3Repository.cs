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
    public class FormT3Repository : RepositoryBase<RmT3Hdr>, IFormT3Repository
    {
        public FormT3Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmT3Hdr.Where(x => x.T3hActiveYn == true)
                         let max = _context.RmT3Hdr.Select(s => s.T3hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.T3hPkRefNo,
                             RefID = hdr.T3hPkRefId,
                             RMU = hdr.T3hRmuCode,
                             RevisionYear = hdr.T3hRevisionYear,
                             RevisionNo = hdr.T3hRevisionNo,
                             IssueDate = hdr.T3hRevisionDate,
                             Status = hdr.T3hStatus,
                             MaxRecord = (hdr.T3hPkRefNo == max)
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
                            string toDate = Utility.ToString(searchData.filter["toIssuDate"]);
                            if (toDate == "")
                                query = query.Where(x => x.IssueDate >= dtFrom);
                            else
                            {
                                DateTime? dtTo = Utility.ToDateTime(toDate);
                                query = query.Where(x => x.IssueDate >= dtFrom && x.IssueDate <= dtTo);
                            }
                            break;
                        case "toIssuDate":
                            string frmDate = Utility.ToString(searchData.filter["fromIssuDate"]);
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

        public RmT3Hdr GetHeaderById(int id, bool view)
        {
            RmT3Hdr res = (from r in _context.RmT3Hdr where r.T3hPkRefNo == id select r).FirstOrDefault();
            res.RmT3History = (from r in _context.RmT3History where r.T3hhT3hPkRefNo == id select r).OrderBy(S => S.T3hhOrder).ToList();

            return res;
        }

        public int DeleteHeader(RmT3Hdr frmT3)
        {
            _context.RmT3Hdr.Attach(frmT3);
            var entry = _context.Entry(frmT3);
            entry.Property(x => x.T3hActiveYn).IsModified = true;
            _context.SaveChanges();
            return frmT3.T3hPkRefNo;
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
            int? rev = (from rn in _context.RmT3Hdr where rn.T3hRevisionYear == Year && rn.T3hRmuCode == RmuCode && rn.T3hActiveYn == true select rn.T3hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<RmT3Hdr> FindDetails(RmT3Hdr frmT3)
        {
            //return await _context.RmFormMHdr.Include(x => x.RmFormMAuditDetails).ThenInclude(x => x.FmadFmhPkRefNoNavigation).Where(x => x.Fr1hAssetId == frmR1R2.Fr1hAssetId && x.Fr1hYearOfInsp == frmR1R2.Fr1hYearOfInsp && x.Fr1hActiveYn == true).FirstOrDefaultAsync();
            return await _context.RmT3Hdr.Include(x => x.RmT3History).ThenInclude(x => x.T3hhT3hPkRefNoNavigation).Where(x => x.T3hRmuCode == frmT3.T3hRmuCode && x.T3hRevisionYear == frmT3.T3hRevisionYear && x.T3hRevisionDate == frmT3.T3hRevisionDate && x.T3hRevisionNo == frmT3.T3hRevisionNo && x.T3hActiveYn == true).FirstOrDefaultAsync();
        }

        public async Task<RmT3Hdr> Save(RmT3Hdr frmT3, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmT3.T3hPkRefNo == 0)
            {
                isAdd = true;
                frmT3.T3hActiveYn = true;
                IDictionary<string, string> lstRef = new Dictionary<string, string>();
                //lstRef.Add("Year", Utility.ToString(frmR1R2.Fr1hYearOfInsp));
                //lstRef.Add("AssetID", Utility.ToString(frmR1R2.Fr1hAssetId));
                //frmR1R2.FmhPkRefNo = Common.RefNumber.FormRefNumber.GetRefNumber(FormType.FormR1R2, lstRef);
                _context.RmT3Hdr.Add(frmT3);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "T3hPkRefNo",
                    "T3hRevisionYear", "T3hRmuName"
                };
                //_context.RmFormS1Dtl.Update(formS1Details);
                //var dtls = frmR1R2.RmFormR2Hdr;
                //frmR1R2.RmFormR2Hdr = null;
                _context.RmT3Hdr.Attach(frmT3);

                var entry = _context.Entry(frmT3);
                entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                {
                    p.IsModified = true;
                });
                if (updateSubmit)
                {
                    entry.Property(x => x.T3hSubmitSts).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();
            if (isAdd)
            {
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                //lstData.Add("RoadCode", frmT3.T3hRmuCode);
                ////lstData.Add("ActivityCode", frmR1R2.FmhActCode);
                ////lstData.Add("Date", Utility.ToString(frmR1R2.FmhAuditedDate, "YYYYMMDD"));
                //lstData.Add("Year", frmR1R2.FmhAuditedDate.Value.Year.ToString());
                //lstData.Add("MonthNo", frmR1R2.FmhAuditedDate.Value.Month.ToString());
                //lstData.Add("Day", frmR1R2.FmhAuditedDate.Value.Day.ToString());
                //lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(frmR1R2.FmhPkRefNo));
                //frmR1R2.FmhRefId = FormRefNumber.GetRefNumber(FormType.FormM, lstData);
                await _context.SaveChangesAsync();
            }
            return frmT3;
        }

        public async Task<int> SaveFormT3(List<RmT3History> FormT3)
        {
            try
            {
                if (FormT3[0].T3hhPkRefNoHistory == 0)
                    _context.RmT3History.AddRange(FormT3);
                else
                {
                    _context.RmT3History.UpdateRange(FormT3);
                    // string[] arrNotReqUpdate = new string[] { "T3hhPkRefNoHistory"

                    //};
                    // var entry = _context.Entry(FormT3);
                    // entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                    // {
                    //     p.IsModified = true;
                    // });
                    // _context.Entry(FormT3).State = EntityState.Modified;
                }
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<FormT3Rpt> GetReportData(int headerid)
        {
            return GetReportDataV2(headerid);
        }

        public List<RmT3History> GetHistoryData(int year)
        {
            List<RmT3History> res = (from r in _context.RmT3History where r.T3hhT3hPkRefNo == year select r).OrderBy(x => x.T3hhOrder).ToList();
            return res;
        }
        public List<RmB14History> GetPlannedBudgetData(string Rmucode, int year)
        {
            var list = _context.RmB14Hdr.Where(x => x.B14hRmuCode == Rmucode && x.B14hRevisionYear == year && x.B14hSubmitSts == true).OrderByDescending(x => x.B14hPkRefNo).ToList();
            List<RmB14History> res = new List<RmB14History>();
            if (list.Count > 0)
                res = (from r in _context.RmB14History where r.B14hhB14hPkRefNo == list[0].B14hPkRefNo select r).OrderBy(x => x.B14hhOrder).ToList();
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

        public async Task<GridWrapper<object>> GetAWPBHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var max = _context.RmT3Hdr.Count() > 0 ? _context.RmT3Hdr.Select(s => s.T3hPkRefNo).DefaultIfEmpty().Max() : 0;

            var query = (from history in _context.RmT3History
                         where history.T3hhT3hPkRefNo == max
                         let hdr = _context.RmT3Hdr.FirstOrDefault(x => x.T3hPkRefNo == max)
                         select new FormAWPBDTO
                         {
                             RefNo = max,
                             RMU = hdr.T3hRmuCode,
                             Feature = history.T3hhFeature,
                             ActivityCode = history.T3hhActCode,
                             ActivityName = history.T3hhActName,
                             Jan = history.T3hhJan,
                             Feb = history.T3hhFeb,
                             Mar = history.T3hhMar,
                             Apr = history.T3hhApr,
                             May = history.T3hhMay,
                             Jun = history.T3hhJun,
                             Jul = history.T3hhJul,
                             Aug = history.T3hhAug,
                             Sep = history.T3hhSep,
                             Oct = history.T3hhOct,
                             Nov = history.T3hhNov,
                             Dec = history.T3hhDec,
                             SubTotal = history.T3hhSubTotal,
                             Unit = history.T3hhUnitOfService
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

        public List<FormT3Rpt> GetReportDataV2(int headerid)
        {


            List<FormT3Rpt> detail = (from o in _context.RmT3Hdr
                                          //where (o.Fr1hAiRdCode == roadcode.Fr1hAiRdCode && o.Fr1hDtOfInsp.HasValue && o.Fr1hDtOfInsp < roadcode.Fr1hDtOfInsp) || o.Fr1hPkRefNo == headerid
                                      where o.T3hPkRefNo == headerid
                                      let formT3 = _context.RmT3History.OrderBy(x => x.T3hhOrder).FirstOrDefault(x => x.T3hhT3hPkRefNo == o.T3hPkRefNo)
                                      select new FormT3Rpt
                                      {
                                          RevisionNo = o.T3hRevisionNo,
                                          RevisionDate = o.T3hRevisionDate,
                                          RevisionYear = o.T3hRevisionYear,
                                          RmuCode = o.T3hRmuCode,

                                          Jan = formT3.T3hhJan,
                                          Feb = formT3.T3hhFeb,
                                          Mar = formT3.T3hhMar,
                                          Apr = formT3.T3hhApr,
                                          May = formT3.T3hhMay,
                                          Jun = formT3.T3hhJun,
                                          Jul = formT3.T3hhJul,
                                          Aug = formT3.T3hhAug,
                                          Sep = formT3.T3hhSep,
                                          Oct = formT3.T3hhOct,
                                          Nov = formT3.T3hhNov,
                                          Dec = formT3.T3hhDec,
                                          SubTotal = formT3.T3hhSubTotal,
                                          UnitOfService = formT3.T3hhUnitOfService,
                                          Remarks = formT3.T3hhRemarks
                                      }).ToList();

            return detail;
        }

        public int? GetB14RevisionNo(string Rmucode, int? year)
        {
            var res = _context.RmB14Hdr.Where(x => x.B14hRmuCode == Rmucode && x.B14hRevisionYear == year && x.B14hSubmitSts == true).OrderByDescending(x => x.B14hPkRefNo).Select(x => x.B14hRevisionNo).FirstOrDefault();
            return res;
        }
    }
}
