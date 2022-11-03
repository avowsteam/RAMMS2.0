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
    public class FormMapRepository : RepositoryBase<RmMapHeader>, IFormMapRepository
    {
        public FormMapRepository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {
            var query = (from hdr in _context.RmMapHeader.Where(x => x.RmmhActiveYn == true)
                         let max = _context.RmMapHeader.Select(s => s.RmmhPkRefNo).DefaultIfEmpty().Max()
                         select new
                         {
                             RefNo = hdr.RmmhPkRefNo,
                             RefID = hdr.RmmhRefId,
                             RMU = hdr.RmmhRmuCode,
                             RevisionYear = hdr.RmmhYear,
                             RevisionNo = hdr.RmmhRevisionNo,
                             Month = hdr.RmmhMonth,
                             Status = hdr.RmmhStatus,
                             MaxRecord = (hdr.RmmhPkRefNo == max)
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
                                 || (x.RMU ?? "").Contains(strVal)
                                 );
                            break;
                        case "FromYear":
                            int iFYr = Utility.ToInt(strVal);
                            query = query.Where(x => x.RevisionYear.HasValue && x.RevisionYear >= iFYr);
                            break;
                        case "ToYear":
                            int iTYr = Utility.ToInt(strVal);
                            query = query.Where(x => x.RevisionYear.HasValue && x.RevisionYear <= iTYr);
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

        public RmMapHeader GetHeaderById(int id, bool view)
        {
            RmMapHeader res = (from r in _context.RmMapHeader where r.RmmhPkRefNo == id select r).FirstOrDefault();
            res.RmMapDetails = (from r in _context.RmMapDetails where r.RmmdRmmhPkRefNo == id select r).ToList();

            return res;
        }

        public int DeleteHeader(RmMapHeader frmT3)
        {
            _context.RmMapHeader.Attach(frmT3);
            var entry = _context.Entry(frmT3);
            entry.Property(x => x.RmmhActiveYn).IsModified = true;
            _context.SaveChanges();
            return frmT3.RmmhPkRefNo;
        }

        public async Task<RmMapHeader> FindDetails(RmMapHeader frmT3)
        {
            //return await _context.RmFormMHdr.Include(x => x.RmFormMAuditDetails).ThenInclude(x => x.FmadFmhPkRefNoNavigation).Where(x => x.Fr1hAssetId == frmR1R2.Fr1hAssetId && x.Fr1hYearOfInsp == frmR1R2.Fr1hYearOfInsp && x.Fr1hActiveYn == true).FirstOrDefaultAsync();
            return await _context.RmMapHeader.Include(x => x.RmMapDetails).ThenInclude(x => x.RmmdRmmhPkRefNoNavigation).Where(x => x.RmmhRmuCode == frmT3.RmmhRmuCode && x.RmmhYear == frmT3.RmmhYear && x.RmmhMonth == frmT3.RmmhMonth && x.RmmhActiveYn == true).FirstOrDefaultAsync();
        }

        public List<RmFormDHdr> GetForDDetails(string RMU, int Year, int Month)
        {
            List<RmFormDHdr> res = (from r in _context.RmFormDHdr where r.FdhRmu == RMU && r.FdhYear == Year && r.FdhDate.Value.Month == Month && r.FdhActiveYn==true select r).ToList();
            List<RmFormDDtl> lstRMDtl = new List<RmFormDDtl>();
            for(int i = 0; i < res.Count(); i++){
                lstRMDtl = new List<RmFormDDtl>();
                lstRMDtl = (from r in _context.RmFormDDtl where r.FddFdhPkRefNo == res[i].FdhPkRefNo select r).ToList();
                res[i].RmFormDDtl = lstRMDtl;
            }            
            return res;
        }

        public List<RmMapDetails> GetForMapDetails(int ID)
        {
            List<RmMapDetails> res = (from r in _context.RmMapDetails where r.RmmdRmmhPkRefNo == ID select r).ToList();            
            return res;
        }

        public async Task<RmMapHeader> Save(RmMapHeader frmmap, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmmap.RmmhPkRefNo == 0)
            {
                isAdd = true;
                frmmap.RmmhActiveYn = true;
                IDictionary<string, string> lstRef = new Dictionary<string, string>();
                //lstRef.Add("Year", Utility.ToString(frmR1R2.Fr1hYearOfInsp));
                //lstRef.Add("AssetID", Utility.ToString(frmR1R2.Fr1hAssetId));
                //frmR1R2.FmhPkRefNo = Common.RefNumber.FormRefNumber.GetRefNumber(FormType.FormR1R2, lstRef);
                _context.RmMapHeader.Add(frmmap);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "RmmhPkRefNo",
                    "RmmhYear", "RmmhRmuName","RmmhMonth"
                };
                //_context.RmFormS1Dtl.Update(formS1Details);
                //var dtls = frmR1R2.RmFormR2Hdr;
                //frmR1R2.RmFormR2Hdr = null;
                _context.RmMapHeader.Attach(frmmap);

                var entry = _context.Entry(frmmap);
                entry.Properties.Where(x => !arrNotReqUpdate.Contains(x.Metadata.Name)).ToList().ForEach((p) =>
                {
                    p.IsModified = true;
                });
                if (updateSubmit)
                {
                    entry.Property(x => x.RmmhSubmitSts).IsModified = true;
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
            return frmmap;
        }

        public async Task<int> SaveFormB14(List<RmMapDetails> Formmap)
        {
            try
            {
                if (Formmap[0].RmmdPkRefNoDetails == 0)
                {
                    _context.RmMapDetails.AddRange(Formmap);
                }
                else
                {
                    _context.RmMapDetails.UpdateRange(Formmap);
                }
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<RmMapDetails> GetHistoryData(int Id)
        {
            List<RmMapDetails> res = (from r in _context.RmMapDetails where r.RmmdRmmhPkRefNo == Id select r).OrderBy(x => x.RmmdActivityId).OrderBy(x=>x.RmmdActivityWeekDayNo).ToList();
            return res;
        }

        public List<FormMapRpt> GetReportData(int headerid)
        {
            return GetReportDataV2(headerid);
        }

        public List<FormMapRpt> GetReportDataV2(int headerid)
        {

            List<FormMapRpt> detail = (from o in _context.RmMapHeader
                                           //where (o.Fr1hAiRdCode == roadcode.Fr1hAiRdCode && o.Fr1hDtOfInsp.HasValue && o.Fr1hDtOfInsp < roadcode.Fr1hDtOfInsp) || o.Fr1hPkRefNo == headerid
                                       where o.RmmhPkRefNo == headerid
                                       let formB14 = _context.RmMapDetails.OrderBy(x => x.RmmdActivityId).OrderBy(x=>x.RmmdActivityWeekDayNo).FirstOrDefault(x => x.RmmdRmmhPkRefNo == o.RmmhPkRefNo)
                                       let formB14UID = _context.RmUsers.FirstOrDefault(x => x.UsrPkId == o.RmmhPreparedBy)
                                       select new FormMapRpt
                                       {
                                           Month=o.RmmhMonth,
                                           Year=o.RmmhYear,

                                           PreparedBy = o.RmmhPreparedBy,
                                           PreparedName = formB14UID.UsrUserName,
                                           PreparedDesig = formB14UID.UsrPosition,
                                           PreparedDate = o.RmmhPreparedDate,
                                           PreparedSign = o.RmmhPreparedSign,
                                           
                                           CheckedBy = o.RmmhCheckedBy,
                                           CheckedName = o.RmmhCheckedName,
                                           CheckedDesig = o.RmmhCheckedDesig,
                                           CheckedDate = o.RmmhCheckedDate,
                                           CheckedSign = o.RmmhCheckedSign,

                                           VerifiedBy = o.RmmhVerifiedBy,
                                           VerifiedName = o.RmmhVerifiedName,
                                           VerifiedDesig = o.RmmhVerifiedDesig,
                                           VerifiedDate = o.RmmhVerifiedDate,
                                           VerifiedSign = o.RmmhVerifiedSign,

                                          
                                           ActivityLocationCode = formB14.RmmdActivityLocationCode,
                                           QuantityKm = formB14.RmmdQuantityKm,
                                           ProductUnit = formB14.RmmdProductUnit,                                           
                                       }).ToList();

            return detail;
        }
    }
}
