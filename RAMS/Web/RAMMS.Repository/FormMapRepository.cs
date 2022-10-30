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
                                 || (x.RevisionNo.HasValue ? x.RevisionNo.Value.ToString() : "").Contains(strVal)
                                 || (x.RMU ?? "").Contains(strVal)
                                 );
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

        public RmMapHeader GetHeaderById(int id, bool view)
        {
            RmMapHeader res = (from r in _context.RmMapHeader where r.RmmhPkRefNo == id select r).FirstOrDefault();
            //res.RmMapDetails = (from r in _context.RmMapDetails where r.RmmdRmmhPkRefNo == id select r).ToList();//sakthivel

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
            //return await _context.RmMapHeader.Include(x => x.RmMapDetails).ThenInclude(x => x.RmmdRmmhPkRefNoNavigation).Where(x => x.RmmhRmuCode == frmT3.RmmhRmuCode && x.RmmhYear == frmT3.RmmhYear && x.RmmhRevisionNo == frmT3.RmmhRevisionNo && x.RmmhActiveYn == true).FirstOrDefaultAsync(); sakthivel
            return await _context.RmMapHeader.FirstAsync();
        }

        public async Task<RmMapHeader> Save(RmMapHeader frmT3, bool updateSubmit)
        {
            bool isAdd = false;
            if (frmT3.RmmhPkRefNo == 0)
            {
                isAdd = true;
                frmT3.RmmhActiveYn = true;
                IDictionary<string, string> lstRef = new Dictionary<string, string>();
                //lstRef.Add("Year", Utility.ToString(frmR1R2.Fr1hYearOfInsp));
                //lstRef.Add("AssetID", Utility.ToString(frmR1R2.Fr1hAssetId));
                //frmR1R2.FmhPkRefNo = Common.RefNumber.FormRefNumber.GetRefNumber(FormType.FormR1R2, lstRef);
                _context.RmMapHeader.Add(frmT3);
            }
            else
            {
                string[] arrNotReqUpdate = new string[] { "RmmhPkRefNo",
                    "RmmhYear", "RmmhRmuName"
                };
                //_context.RmFormS1Dtl.Update(formS1Details);
                //var dtls = frmR1R2.RmFormR2Hdr;
                //frmR1R2.RmFormR2Hdr = null;
                _context.RmMapHeader.Attach(frmT3);

                var entry = _context.Entry(frmT3);
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
    }
}
