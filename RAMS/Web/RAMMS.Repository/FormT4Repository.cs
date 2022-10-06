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
    public class FormT4Repository : RepositoryBase<RmT4DesiredBdgtHeader>, IFormT4Repository
    {
        public FormT4Repository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormT4HeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormT4SearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmT4DesiredBdgtHeader

                         select new { x });


            query = query.OrderByDescending(x => x.x.T4dbhPkRefNo);

            if (filterOptions.Filters.Year != null && filterOptions.Filters.Year != string.Empty)
            {
                query = query.Where(s => s.x.T4dbhRevisionYear == Convert.ToInt32(filterOptions.Filters.Year));
            }

            if (!string.IsNullOrEmpty(filterOptions.Filters.RMU))
            {
                query = query.Where(s => s.x.T4dbhRmu == filterOptions.Filters.RMU);
            }


            string frmDate = Utility.ToString(filterOptions.Filters.FromDate);
            string toDate = Utility.ToString(filterOptions.Filters.ToDate);

            DateTime? dtFrom = Utility.ToDateTime(frmDate);
            DateTime? dtTo = Utility.ToDateTime(toDate);
            if (toDate == "" && frmDate != "")
                query = query.Where(s => s.x.T4dbhRevisionDate >= dtFrom);
            else if (toDate != "" && frmDate != "")
                query = query.Where(s => s.x.T4dbhRevisionDate >= dtFrom && s.x.T4dbhRevisionDate <= dtTo);
            else if (frmDate == "" && toDate != "")
                query = query.Where(s => s.x.T4dbhRevisionDate <= dtTo);


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {


                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
                     s.x.T4dbhPkRefId.Contains(filterOptions.Filters.SmartSearch) ||
                    s.x.T4dbhRmu.Contains(filterOptions.Filters.SmartSearch) ||
                     s.x.T4dbhStatus.Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.T4dbhRevisionNo.HasValue ? s.x.T4dbhRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.T4dbhRevisionYear.HasValue ? s.x.T4dbhRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                    (s.x.T4dbhRevisionDate.HasValue ? (s.x.T4dbhRevisionDate.Value.Year == dt.Year && s.x.T4dbhRevisionDate.Value.Month == dt.Month && s.x.T4dbhRevisionDate.Value.Day == dt.Day) : true) && s.x.T4dbhRevisionDate != null);
                }
                else
                {
                    query = query.Where(s =>
                     s.x.T4dbhPkRefId.Contains(filterOptions.Filters.SmartSearch) ||
                   s.x.T4dbhRmu.Contains(filterOptions.Filters.SmartSearch) ||
                     s.x.T4dbhStatus.Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.T4dbhRevisionNo.HasValue ? s.x.T4dbhRevisionNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.T4dbhRevisionYear.HasValue ? s.x.T4dbhRevisionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }

            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.x.T4dbhRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.T4dbhRevisionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.T4dbhRevisionNo);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.T4dbhRevisionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.x.T4dbhStatus);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.T4dbhRmu);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.T4dbhRevisionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.T4dbhRevisionNo);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.T4dbhRevisionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.x.T4dbhStatus);
            }



            var list = query.Select(s => new FormT4HeaderResponseDTO
            {
                PkRefNo = s.x.T4dbhPkRefNo,
                PkRefId = s.x.T4dbhPkRefId,
                RevisionDate = s.x.T4dbhRevisionDate,
                RevisionNo = s.x.T4dbhRevisionNo,
                RevisionYear = s.x.T4dbhRevisionYear,
                Status = s.x.T4dbhStatus,
                Rmu = s.x.T4dbhRmu

            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }


        public async Task<RmT4DesiredBdgtHeader> SaveFormT4(RmT4DesiredBdgtHeader FormT4)
        {
            try
            {
                _context.RmT4DesiredBdgtHeader.Add(FormT4);
                _context.SaveChanges();
               
                return FormT4;
            }
            catch (Exception ex)
            {
                await _context.DisposeAsync();
                throw ex;
            }
        }




        public RmT4DesiredBdgtHeader GetHeaderById(int id)
        {
            var FormT4 = (from r in _context.RmT4DesiredBdgtHeader where r.T4dbhPkRefNo == id select r).FirstOrDefault();

            if (!FormT4.T4dbhSubmitSts)
            {
                var B9year = (from r in _context.RmB9DesiredService select r.B9dsRevisionYear).DefaultIfEmpty().Max();
                var B9rev = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == B9year select r.B9dsRevisionNo).DefaultIfEmpty().Max();
                var B9hdrPkrefNo = (from r in _context.RmB9DesiredService where r.B9dsRevisionYear == B9year && r.B9dsRevisionNo == B9rev select r.B9dsPkRefNo).FirstOrDefault();

                var B10year = (from r in _context.RmB10DailyProduction select r.B10dpRevisionYear).DefaultIfEmpty().Max();
                var B10rev = (from r in _context.RmB10DailyProduction where r.B10dpRevisionYear == B10year select r.B10dpRevisionNo).DefaultIfEmpty().Max();
                var B10hdrPkrefNo = (from r in _context.RmB10DailyProduction where r.B10dpRevisionYear == B10year && r.B10dpRevisionNo == B10rev select r.B10dpPkRefNo).FirstOrDefault();

                var B11year = (from r in _context.RmB11Hdr where r.B11hRmuCode == FormT4.T4dbhRmu select r.B11hRevisionYear).DefaultIfEmpty().Max();
                var B11rev = (from r in _context.RmB11Hdr where r.B11hRevisionYear == B11year && r.B11hRmuCode == FormT4.T4dbhRmu select r.B11hRevisionNo).DefaultIfEmpty().Max();
                var B11hdrPkrefNo = (from r in _context.RmB11Hdr where r.B11hRevisionYear == B11year && r.B11hRmuCode == FormT4.T4dbhRmu && r.B11hRevisionNo == B11rev select r.B11hPkRefNo).FirstOrDefault();

                var B13year = (from r in _context.RmB13ProposedPlannedBudget select r.B13pRevisionYear).DefaultIfEmpty().Max();
                var B13rev = (from r in _context.RmB13ProposedPlannedBudget where r.B13pRevisionYear == B13year select r.B13pRevisionNo).DefaultIfEmpty().Max();
                var B13hdrPkrefNo = (from r in _context.RmB13ProposedPlannedBudget where r.B13pRevisionYear == B13year && r.B13pRmu == FormT4.T4dbhRmu && r.B13pRevisionNo == B13rev select r.B13pPkRefNo).FirstOrDefault();


                FormT4.RmT4DesiredBdgt = (from r in _context.RmB9DesiredServiceHistory
                                          where r.B9dshB9dsPkRefNo == B9hdrPkrefNo
                                          orderby r.B9dshPkRefNo
                                          select new RmT4DesiredBdgt
                                          {
                                              T4dbFeature = r.B9dshFeature,
                                              T4dbCode = r.B9dshCode,
                                              T4dbName = r.B9dshName,
                                              T4dbInvCond1 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == B13hdrPkrefNo && rec.B13phCode == r.B9dshCode select rec.B13phInvCond1).FirstOrDefault(),
                                              T4dbInvCond2 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == B13hdrPkrefNo && rec.B13phCode == r.B9dshCode select rec.B13phInvCond2).FirstOrDefault(),
                                              T4dbInvCond3 = (from rec in _context.RmB13ProposedPlannedBudgetHistory where rec.B13phB13pPkRefNo == B13hdrPkrefNo && rec.B13phCode == r.B9dshCode select rec.B13phInvCond3).FirstOrDefault(),
                                              T4dbSlCond1 = r.B9dshCond1,
                                              T4dbSlCond2 = r.B9dshCond2,
                                              T4dbSlCond3 = r.B9dshCond3,
                                              T4dbCdcLabour = (from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum(),
                                              T4dbCdcEquipment = (from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum(),
                                              T4dbCdcMaterial = (from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum(),
                                              T4dbCrewDaysCost = Convert.ToDecimal((from lr in _context.RmB11LabourCost where lr.B11lcB11hPkRefNo == B11hdrPkrefNo && lr.B11lcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11lcLabourTotalPrice).Sum()) + Convert.ToDecimal((from lr in _context.RmB11EquipmentCost where lr.B11ecB11hPkRefNo == B11hdrPkrefNo && lr.B11ecActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11ecEquipmentTotalPrice).Sum()) + Convert.ToDecimal((from lr in _context.RmB11MaterialCost where lr.B11mcB11hPkRefNo == B11hdrPkrefNo && lr.B11mcActivityId == Convert.ToInt32(r.B9dshCode) select lr.B11mcMaterialTotalPrice).Sum()),
                                              T4dbAverageDailyProduction = (from rec in _context.RmB10DailyProductionHistory where rec.B10dphB10dpPkRefNo == B10hdrPkrefNo && rec.B10dphCode == r.B9dshCode select rec.B10dphAdpValue).FirstOrDefault(),
                                              T4dbUnitOfService = (from rec in _context.RmB10DailyProductionHistory where rec.B10dphB10dpPkRefNo == B10hdrPkrefNo && rec.B10dphCode == r.B9dshCode select rec.B10dphAdpUnit).FirstOrDefault(),
                                          }).ToList();
            }
            else
            {
                FormT4.RmT4DesiredBdgt = (from r in _context.RmT4DesiredBdgt
                                          where r.T4dbPkRefNo == FormT4.T4dbhPkRefNo
                                                       select r).ToList();
            }




            return FormT4;
        }

        public int? GetMaxRev(int Year, string RMU)
        {
            int? rev = (from rn in _context.RmT4DesiredBdgtHeader where rn.T4dbhRevisionYear == Year && rn.T4dbhRmu == RMU select rn.T4dbhRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }



        public async Task<int> UpdateFormT4(RmT4DesiredBdgtHeader FormT4, List<RmT4DesiredBdgt> FormT4History)
        {
            try
            {


                IList<RmT4DesiredBdgt> child = (from r in _context.RmT4DesiredBdgt where r.T4dbT4pdbhPkRefNo == FormT4.T4dbhPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var item in FormT4History)
                {
                    item.T4dbT4pdbhPkRefNo = FormT4.T4dbhPkRefNo;
                    item.T4dbPkRefNo = 0;
                    _context.RmT4DesiredBdgt.Add(item);
                    _context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int? DeleteFormT4(int id)
        {
            try
            {
                IList<RmT4DesiredBdgt> child = (from r in _context.RmT4DesiredBdgt where r.T4dbT4pdbhPkRefNo == id select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmT4DesiredBdgtHeader> parent = (from r in _context.RmT4DesiredBdgtHeader where r.T4dbhPkRefNo == id select r).ToList();
                foreach (var item in parent)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                
                return 1;

            }
            catch (Exception ex)
            {
                return 500;
            }
        }



    }
}
