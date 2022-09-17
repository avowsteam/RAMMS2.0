using Microsoft.EntityFrameworkCore;
using RAMMS.Common;
using RAMMS.Domain.Models;
using RAMMS.DTO;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository
{
    public class FormB13Repository : RepositoryBase<RmB13ProposedPlannedBudget>, IFormB13Repository
    {
        public FormB13Repository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormB13ResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormB13SearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmB13ProposedPlannedBudget

                         select new { x });


            query = query.OrderByDescending(x => x.x.B13pPkRefNo);

            if (filterOptions.Filters.Year != null && filterOptions.Filters.Year != string.Empty)
            {
                query = query.Where(s => s.x.B13pRevisionYear == Convert.ToInt32(filterOptions.Filters.Year));
            }

            if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
            {
                query = query.Where(s => s.x.B13pRmu == filterOptions.Filters.RMU);
            }


            string frmDate = Utility.ToString(filterOptions.Filters.FromDate);
            string toDate = Utility.ToString(filterOptions.Filters.ToDate);

            DateTime? dtFrom = Utility.ToDateTime(frmDate);
            DateTime? dtTo = Utility.ToDateTime(toDate);
            if (toDate == "" && frmDate != "")
                query = query.Where(s => s.x.B13pRevisionDate >= dtFrom);
            else if (toDate != "" && frmDate != "")
                query = query.Where(s => s.x.B13pRevisionDate >= dtFrom && s.x.B13pRevisionDate <= dtTo);
            else if (frmDate == "" && toDate != "")
                query = query.Where(s => s.x.B13pRevisionDate <= dtTo);


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {


                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
                    s.x.B13pRmu.Contains(filterOptions.Filters.SmartSearch) ||
                     s.x.B13pStatus.Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B13pRevisionNo.HasValue ? s.x.B13pRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B13pRevisionYear.HasValue ? s.x.B13pRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.B13pRevisionDate.HasValue ? (s.x.B13pRevisionDate.Value.Year == dt.Year && s.x.B13pRevisionDate.Value.Month == dt.Month && s.x.B13pRevisionDate.Value.Day == dt.Day) : true) && s.x.B13pRevisionDate != null);
                }
                else
                {
                    query = query.Where(s =>
                   s.x.B13pRmu.Contains(filterOptions.Filters.SmartSearch) ||
                     s.x.B13pStatus.Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B13pRevisionNo.HasValue ? s.x.B13pRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.B13pRevisionYear.HasValue ? s.x.B13pRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }

            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.x.B13pRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.B13pRevisionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.B13pRevisionNo);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.B13pRevisionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.x.B13pStatus);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.B13pRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.B13pRevisionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.B13pRevisionNo);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.B13pRevisionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.x.B13pStatus);

            }



            var list = query.Select(s => new FormB13ResponseDTO
            {
                PkRefNo = s.x.B13pPkRefNo,
                RevisionDate = s.x.B13pRevisionDate,
                RevisionNo = s.x.B13pRevisionNo,
                RevisionYear = s.x.B13pRevisionYear,
                Status = s.x.B13pStatus,
                Rmu = s.x.B13pRmu

            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }


        public async Task<RmB13ProposedPlannedBudget> SaveFormB13(RmB13ProposedPlannedBudget FormB13)
        {
            try
            {
                _context.RmB13ProposedPlannedBudget.Add(FormB13);
                _context.SaveChanges();

                //var B9rev = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == FormB13.B13pRevisionYear select r.B9dsRevisionNo).DefaultIfEmpty().Max();
                //var B9hdrPkrefNo = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == FormB13.B13pRevisionYear && r.B9dsRevisionNo == B9rev select r.B9dsPkRefNo).FirstOrDefault();

                //var B11rev = (from r in _context.RmB11Hdr where r.B11hRevisionYear == FormB13.B13pRevisionYear && r.B11hRmuCode == FormB13.B13pRmu select r.B11hRevisionNo).DefaultIfEmpty().Max();
                //var B11hdrPkrefNo = (from r in _context.RmB11Hdr where r.B11hRevisionYear == FormB13.B13pRevisionYear && r.B11hRmuCode == FormB13.B13pRmu && r.B11hRevisionNo == B11rev select r.B11hPkRefNo).FirstOrDefault();

                //FormB13.RmB13ProposedPlannedBudgetHistory = (from r in _context.RmB9DesiredServiceHistory
                //                                             where r.B9dshB9dsPkRefNo == B9hdrPkrefNo
                //                                             select new RmB13ProposedPlannedBudgetHistory
                //                                             {
                //                                                 B13phFeature = r.B9dshFeature,
                //                                                 B13phCode = r.B9dshCode,
                //                                                 B13phName = r.B9dshName,
                //                                                 B13phSlCond1 = r.B9dshCond1,
                //                                                 B13phSlCond2 = r.B9dshCond2,
                //                                                 B13phSlCond3 = r.B9dshCond3,
                //                                                 B13phCdcLabour = (from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum(),
                //                                                 B13phCdcEquipment = (from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum(),
                //                                                 B13phCdcMaterial = (from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum(),
                //                                                 B13phCrewDaysCost = (from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum() + (from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum() + (from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum()

                //                                             }).ToList();

                return FormB13;
            }
            catch (Exception ex)
            {
                await _context.DisposeAsync();
                throw ex;
            }
        }




        public RmB13ProposedPlannedBudget GetHeaderById(int id)
        {
            var FormB13 = (from r in _context.RmB13ProposedPlannedBudget where r.B13pPkRefNo == id select r).FirstOrDefault();

            if (!FormB13.B13pSubmitSts)
            {
                var B9rev = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == FormB13.B13pRevisionYear select r.B9dsRevisionNo).DefaultIfEmpty().Max();
                var B9hdrPkrefNo = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == FormB13.B13pRevisionYear && r.B9dsRevisionNo == B9rev select r.B9dsPkRefNo).FirstOrDefault();

                var B10rev = (from r in _context.RmB10DailyProduction where r.B10dpRevisionYear == FormB13.B13pRevisionYear select r.B10dpRevisionNo).DefaultIfEmpty().Max();
                var B10hdrPkrefNo = (from r in _context.RmB10DailyProduction where r.B10dpRevisionYear == FormB13.B13pRevisionYear && r.B10dpRevisionNo == B10rev select r.B10dpPkRefNo).FirstOrDefault();


                var B11rev = (from r in _context.RmB11Hdr where r.B11hRevisionYear == FormB13.B13pRevisionYear && r.B11hRmuCode == FormB13.B13pRmu select r.B11hRevisionNo).DefaultIfEmpty().Max();
                var B11hdrPkrefNo = (from r in _context.RmB11Hdr where r.B11hRevisionYear == FormB13.B13pRevisionYear && r.B11hRmuCode == FormB13.B13pRmu && r.B11hRevisionNo == B11rev select r.B11hPkRefNo).FirstOrDefault();

                FormB13.RmB13ProposedPlannedBudgetHistory = (from r in _context.RmB9DesiredServiceHistory
                                                             where r.B9dshB9dsPkRefNo == B9hdrPkrefNo
                                                             orderby r.B9dshPkRefNo
                                                             select new RmB13ProposedPlannedBudgetHistory
                                                             {
                                                                 B13phFeature = r.B9dshFeature,
                                                                 B13phCode = r.B9dshCode,
                                                                 B13phName = r.B9dshName,
                                                                 B13phSlCond1 = r.B9dshCond1,
                                                                 B13phSlCond2 = r.B9dshCond2,
                                                                 B13phSlCond3 = r.B9dshCond3,
                                                                 B13phCdcLabour = (from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum(),
                                                                 B13phCdcEquipment = (from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum(),
                                                                 B13phCdcMaterial = (from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum(),
                                                                 B13phCrewDaysCost = (from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum() + (from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum() + (from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum(),
                                                                 B13phInvCond1 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == id && rec.B13phCode == r.B9dshCode select rec.B13phInvCond1).FirstOrDefault(),
                                                                 B13phInvCond2 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == id && rec.B13phCode == r.B9dshCode select rec.B13phInvCond2).FirstOrDefault(),
                                                                 B13phInvCond3 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == id && rec.B13phCode == r.B9dshCode select rec.B13phInvCond3).FirstOrDefault(),
                                                                 B13phSlAvgDesired = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == id && rec.B13phCode == r.B9dshCode select rec.B13phSlAvgDesired).FirstOrDefault(),
                                                                 B13phAverageDailyProduction = (from rec in _context.RmB10DailyProductionHistory where   rec.B10dphB10dpPkRefNo == B10hdrPkrefNo && rec.B10dphCode == r.B9dshCode select rec.B10dphAdpValue).FirstOrDefault(),
                                                                 B13phUnitOfService = (from rec in _context.RmB10DailyProductionHistory where  rec.B10dphB10dpPkRefNo == B10hdrPkrefNo && rec.B10dphCode == r.B9dshCode select rec.B10dphAdpUnit).FirstOrDefault(),
                                                             }).ToList();
            }
            else
            {
                FormB13.RmB13ProposedPlannedBudgetHistory = (from r in _context.RmB13ProposedPlannedBudgetHistory
                                                             where r.B13phB13pPkRefNo == FormB13.B13pPkRefNo
                                                             select r).ToList();
            }


            FormB13.RmB13RevisionHistory = (from rn in _context.RmB13ProposedPlannedBudget
                                            where rn.B13pRevisionYear == FormB13.B13pRevisionYear && rn.B13pRmu == FormB13.B13pRmu && rn.B13pRevisionNo <= FormB13.B13pRevisionNo
                                            orderby rn.B13pRevisionNo
                                            select new RmB13RevisionHistory
                                            {
                                                B13rhDate = rn.B13pRevisionDate,
                                                B13rhDescription = rn.B13pUserDesignationAgrd,
                                                B13rhRevNo = rn.B13pRevisionNo
                                            }
                                            ).ToList();


            return FormB13;
        }

        public int? GetMaxRev(int Year, string RMU)
        {
            int? rev = (from rn in _context.RmB13ProposedPlannedBudget where rn.B13pRevisionYear == Year && rn.B13pRmu == RMU select rn.B13pRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }



        public async Task<int> UpdateFormB13(RmB13ProposedPlannedBudget FormB13, List<RmB13ProposedPlannedBudgetHistory> FormB13History)
        {
            try
            {


                IList<RmB13ProposedPlannedBudgetHistory> child = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == FormB13.B13pPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var item in FormB13History)
                {
                    item.B13phB13pPkRefNo = FormB13.B13pPkRefNo;
                    item.B13phPkRefNo = 0;
                    _context.RmB13ProposedPlannedBudgetHistory.Add(item);
                    _context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int? DeleteFormB13(int id)
        {
            try
            {
                IList<RmB13ProposedPlannedBudgetHistory> child = (from r in _context.RmB13ProposedPlannedBudgetHistory where r.B13phB13pPkRefNo == id select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmB13ProposedPlannedBudget> parent = (from r in _context.RmB13ProposedPlannedBudget where r.B13pPkRefNo == id select r).ToList();
                foreach (var item in parent)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                //var res = _context.Set<RmFormF3Hdr>().FindAsync(id);
                //res.Result.Ff3hActiveYn = false;
                //_context.Set<RmFormF3Hdr>().Attach(res.Result);
                //_context.Entry<RmFormF3Hdr>(res.Result).State = EntityState.Modified;
                //_context.SaveChanges();
                return 1;

            }
            catch (Exception ex)
            {
                return 500;
            }
        }



    }
}
