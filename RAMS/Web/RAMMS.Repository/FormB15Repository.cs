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
    public class FormB15Repository : RepositoryBase<RmB15Hdr>, IFormB15Repository
    {
        public FormB15Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmB15Hdr.Where(x => x.B15hActiveYn == true)
                         let max = _context.RmB15Hdr.Select(s => s.B15hPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.B15hPkRefNo,
                             RefID = hdr.B15hPkRefId,
                             RMU = hdr.B15hRmuCode,
                             RevisionYear = hdr.B15hRevisionYear,
                             RevisionNo = hdr.B15hRevisionNo,
                             IssueDate = hdr.B15hRevisionDate,
                             Status = hdr.B15hStatus,
                             MaxRecord = (hdr.B15hPkRefNo == max)
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

        public RmB15Hdr GetHeaderById(int id, bool view)
        {
            RmB15Hdr res = (from r in _context.RmB15Hdr where r.B15hPkRefNo == id select r).FirstOrDefault();
            //int? RevNo = (from rn in _context.RmB15Hdr
            //              where rn.B15hRevisionYear == res.B15hRevisionYear && rn.B15hRmuCode == res.B15hRmuCode
            //              select rn.B15hRevisionNo).DefaultIfEmpty().Max() + 1;
            //if (view == false)
            //    res.B15hRevisionNo = RevNo;
            res.RmB15History = (from r in _context.RmB15History where r.B15hhB15hPkRefNo == id select r).OrderBy(S => S.B15hhOrder).ToList();

            return res;
        }

        public int DeleteHeader(RmB15Hdr frmB15)
        {
            _context.RmB15Hdr.Attach(frmB15);
            var entry = _context.Entry(frmB15);
            entry.Property(x => x.B15hActiveYn).IsModified = true;
            _context.SaveChanges();
            return frmB15.B15hPkRefNo;
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
            int? rev = (from rn in _context.RmB15Hdr where rn.B15hRevisionYear == Year && rn.B15hRmuCode == RmuCode select rn.B15hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public async Task<RmB15Hdr> FindDetails(RmB15Hdr frmB15)
        {
            //return await _context.RmFormMHdr.Include(x => x.RmFormMAuditDetails).ThenInclude(x => x.FmadFmhPkRefNoNavigation).Where(x => x.Fr1hAssetId == frmR1R2.Fr1hAssetId && x.Fr1hYearOfInsp == frmR1R2.Fr1hYearOfInsp && x.Fr1hActiveYn == true).FirstOrDefaultAsync();
            return await _context.RmB15Hdr.Include(x => x.RmB15History).ThenInclude(x => x.B15hhB15hPkRefNoNavigation).Where(x => x.B15hRmuCode == frmB15.B15hRmuCode && x.B15hRevisionYear == frmB15.B15hRevisionYear && x.B15hRevisionDate == frmB15.B15hRevisionDate && x.B15hRevisionNo == frmB15.B15hRevisionNo && x.B15hActiveYn == true).FirstOrDefaultAsync();
        }

        public async Task<RmB15Hdr> Save(RmB15Hdr frmB15, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmB15.B15hPkRefNo == 0)
            {
                isAdd = true;
                frmB15.B15hActiveYn = true;
                IDictionary<string, string> lstRef = new Dictionary<string, string>();
                //lstRef.Add("Year", Utility.ToString(frmR1R2.Fr1hYearOfInsp));
                //lstRef.Add("AssetID", Utility.ToString(frmR1R2.Fr1hAssetId));
                //frmR1R2.FmhPkRefNo = Common.RefNumber.FormRefNumber.GetRefNumber(FormType.FormR1R2, lstRef);
                _context.RmB15Hdr.Add(frmB15);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "B15hPkRefNo",
                    "B15hRevisionYear", "B15hRmuName"
                };
                //_context.RmFormS1Dtl.Update(formS1Details);
                //var dtls = frmR1R2.RmFormR2Hdr;
                //frmR1R2.RmFormR2Hdr = null;
                _context.RmB15Hdr.Attach(frmB15);

                var entry = _context.Entry(frmB15);
                entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                {
                    p.IsModified = true;
                });
                if (updateSubmit)
                {
                    entry.Property(x => x.B15hSubmitSts).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();
            if (isAdd)
            {
                IDictionary<string, string> lstData = new Dictionary<string, string>();
                //lstData.Add("RoadCode", frmB15.B15hRmuCode);
                ////lstData.Add("ActivityCode", frmR1R2.FmhActCode);
                ////lstData.Add("Date", Utility.ToString(frmR1R2.FmhAuditedDate, "YYYYMMDD"));
                //lstData.Add("Year", frmR1R2.FmhAuditedDate.Value.Year.ToString());
                //lstData.Add("MonthNo", frmR1R2.FmhAuditedDate.Value.Month.ToString());
                //lstData.Add("Day", frmR1R2.FmhAuditedDate.Value.Day.ToString());
                //lstData.Add(FormRefNumber.NewRunningNumber, Utility.ToString(frmR1R2.FmhPkRefNo));
                //frmR1R2.FmhRefId = FormRefNumber.GetRefNumber(FormType.FormM, lstData);
                await _context.SaveChangesAsync();
            }
            return frmB15;
        }

        public async Task<int> SaveFormB15(List<RmB15History> FormB15)
        {
            try
            {
                if (FormB15[0].B15hhPkRefNoHistory == 0)
                    _context.RmB15History.AddRange(FormB15);
                else
                {
                    _context.RmB15History.UpdateRange(FormB15);
                    // string[] arrNotReqUpdate = new string[] { "B15hhPkRefNoHistory"

                    //};
                    // var entry = _context.Entry(FormB15);
                    // entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                    // {
                    //     p.IsModified = true;
                    // });
                    // _context.Entry(FormB15).State = EntityState.Modified;
                }
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<FormB15Rpt> GetReportData(int headerid)
        {
            return GetReportDataV2(headerid);
        }

        public List<RmB15History> GetHistoryData(int pkRefNo)
        {
            List<RmB15History> res = (from r in _context.RmB15History where r.B15hhB15hPkRefNo == pkRefNo select r).OrderBy(x => x.B15hhOrder).ToList();
            return res;
        }
        public List<RmB13ProposedPlannedBudgetHistory> GetPlannedBudgetData(string Rmucode, int year)
        {
            if (Rmucode.ToUpper() == "MRI")
                Rmucode = "Miri";
            else if (Rmucode.ToUpper() == "BTN")
                Rmucode = "Batu Niah";
            var list = _context.RmB13ProposedPlannedBudget.Where(x => x.B13pRmu == Rmucode && x.B13pRevisionYear == year && x.B13pSubmitSts == true).OrderByDescending(x => x.B13pPkRefNo).ToList();
            List<RmB13ProposedPlannedBudgetHistory> res = new List<RmB13ProposedPlannedBudgetHistory>();
            if (list.Count > 0)
                res = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == list[0].B13pPkRefNo select r).ToList();
            return res;
        }

        public List<FormB15Rpt> GetReportDataV2(int headerid)
        {

            List<FormB15Rpt> detail = (from o in _context.RmB15Hdr
                                           //where (o.Fr1hAiRdCode == roadcode.Fr1hAiRdCode && o.Fr1hDtOfInsp.HasValue && o.Fr1hDtOfInsp < roadcode.Fr1hDtOfInsp) || o.Fr1hPkRefNo == headerid
                                       where o.B15hPkRefNo == headerid
                                       let formB15 = _context.RmB15History.OrderBy(x => x.B15hhOrder).FirstOrDefault(x => x.B15hhB15hPkRefNo == o.B15hPkRefNo)
                                       let formB15UID= _context.RmUsers.FirstOrDefault(x => x.UsrPkId == o.B15hUseridProsd)
                                       select new FormB15Rpt
                                       {
                                           RevisionNo = o.B15hRevisionNo,
                                           RevisionDate = o.B15hRevisionDate,
                                           RevisionYear = o.B15hRevisionYear,

                                           UseridProsd = o.B15hUseridProsd,
                                           UserNameProsd = formB15UID.UsrUserName,
                                           UserDesignationProsd = formB15UID.UsrPosition,
                                           DtProsd = o.B15hDtProsd,
                                           SignProsd = o.B15hSignProsd,

                                           UseridFclitd = o.B15hUseridFclitd,
                                           UserNameFclitd = o.B15hUserNameFclitd,
                                           UserDesignationFclitd = o.B15hUserDesignationFclitd,
                                           DtFclitd = o.B15hDtFclitd,
                                           SignFclitd = o.B15hSignFclitd,

                                           UseridAgrd = o.B15hUseridAgrd,
                                           UserNameAgrd = o.B15hUserNameAgrd,
                                           UserDesignationAgrd = o.B15hUserDesignationAgrd,
                                           DtAgrd = o.B15hDtAgrd,
                                           SignAgrd = o.B15hSignAgrd,

                                           UseridEndosd = o.B15hUseridEndosd,
                                           UserNameEndosd = o.B15hUserNameEndosd,
                                           UserDesignationEndosd = o.B15hUserDesignationEndosd,
                                           DtEndosd = o.B15hDtEndosd,
                                           SignEndosd = o.B15hSignEndosd,

                                           Jan = formB15.B15hhJan,
                                           Feb = formB15.B15hhFeb,
                                           Mar = formB15.B15hhMar,
                                           Apr = formB15.B15hhApr,
                                           May = formB15.B15hhMay,
                                           Jun = formB15.B15hhJun,
                                           Jul = formB15.B15hhJul,
                                           Aug = formB15.B15hhAug,
                                           Sep = formB15.B15hhSep,
                                           Oct = formB15.B15hhOct,
                                           Nov = formB15.B15hhNov,
                                           Dec = formB15.B15hhDec,
                                           SubTotal = formB15.B15hhSubTotal,
                                           Remarks = formB15.B15hhRemarks
                                       }).ToList();

            return detail;
        }
    }
}
