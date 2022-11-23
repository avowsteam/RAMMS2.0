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
    public class FormPARepository : RepositoryBase<RmPaymentCertificateMamw>, IFormPARepository
    {
        public FormPARepository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormPAHeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormPASearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmPaymentCertificateMamw

                         select new { x });


            query = query.OrderByDescending(x => x.x.PcmamwPkRefNo);




            if ((filterOptions.Filters.YearTo == null || filterOptions.Filters.YearTo == string.Empty) && (filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty))
                query = query.Where(s => s.x.PcmamwSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty)
                query = query.Where(s => s.x.PcmamwSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom) && s.x.PcmamwSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && (filterOptions.Filters.YearFrom == null && filterOptions.Filters.YearFrom == string.Empty))
                query = query.Where(s => s.x.PcmamwSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));


            if ((filterOptions.Filters.MonthTo == null || filterOptions.Filters.MonthTo == string.Empty) && (filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty))
                query = query.Where(s => s.x.PcmamwSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty)
                query = query.Where(s => s.x.PcmamwSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom) && s.x.PcmamwSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && (filterOptions.Filters.MonthFrom == null && filterOptions.Filters.MonthFrom == string.Empty))
                query = query.Where(s => s.x.PcmamwSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                query = query.Where(s =>
               (s.x.PcmamwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PcmamwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PcmamwWorkValueDeduction.HasValue ? s.x.PcmamwWorkValueDeduction.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PcmamwWorkValueAddition.HasValue ? s.x.PcmamwWorkValueAddition.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PcmamwSubmissionYear.HasValue ? s.x.PcmamwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PcmamwSubmissionMonth.HasValue ? s.x.PcmamwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
            }


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {

                    query = query.Where(s =>
                   (s.x.PcmamwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
                   (s.x.PcmamwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
                   (s.x.PcmamwWorkValueDeduction.HasValue ? s.x.PcmamwWorkValueDeduction.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.PcmamwWorkValueAddition.HasValue ? s.x.PcmamwWorkValueAddition.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.PcmamwSubmissionYear.HasValue ? s.x.PcmamwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.PcmamwSubmissionMonth.HasValue ? s.x.PcmamwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                   (s.x.PcmamwSignDateSp.HasValue ? (s.x.PcmamwSignDateSp.Value.Year == dt.Year && s.x.PcmamwSignDateSp.Value.Month == dt.Month && s.x.PcmamwSignDateSp.Value.Day == dt.Day) : true) && s.x.PcmamwSignDateSp != null);
                }
                else
                {
                    query = query.Where(s =>
                 (s.x.PcmamwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
                 (s.x.PcmamwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
                 (s.x.PcmamwWorkValueDeduction.HasValue ? s.x.PcmamwWorkValueDeduction.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                 (s.x.PcmamwWorkValueAddition.HasValue ? s.x.PcmamwWorkValueAddition.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                 (s.x.PcmamwSubmissionYear.HasValue ? s.x.PcmamwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
                 (s.x.PcmamwSubmissionMonth.HasValue ? s.x.PcmamwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }
            }

            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.x.PcmamwRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.PcmamwSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.PcmamwSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.PcmamwWorkValueDeduction);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.x.PcmamwWorkValueAddition);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(s => s.x.PcmamwSignDateSp);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(s => s.x.PcmamwStatus);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.PcmamwRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.PcmamwSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.PcmamwSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.PcmamwWorkValueDeduction);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.x.PcmamwWorkValueAddition);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(s => s.x.PcmamwSignDateSp);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(s => s.x.PcmamwStatus);

            }



            var list = query.Select(s => new FormPAHeaderResponseDTO
            {
                PkRefNo = s.x.PcmamwPkRefNo,
                RefId = s.x.PcmamwRefId,
                SubmissionYear = s.x.PcmamwSubmissionYear,
                SubmissionMonth = s.x.PcmamwSubmissionMonth,
                WorkValueDeduction = s.x.PcmamwWorkValueDeduction,
                WorkValueAddition = s.x.PcmamwWorkValueAddition,
                SignDateSp = s.x.PcmamwSignDateSp,
                Status = s.x.PcmamwStatus,
                SubmitSts = s.x.PcmamwSubmitSts
            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }


        public class Employee
        {
            public int ID { get; set; }
            public string Description { get; set; }

        }

        public RmPaymentCertificateMamw GetHeaderById(int id)
        {
            RmPaymentCertificateMamw res = (from r in _context.RmPaymentCertificateMamw where r.PcmamwPkRefNo == id select r).Include(x => x.RmPaymentCertificateCrr).Include(x => x.RmPaymentCertificateCrra).Include(x => x.RmPaymentCertificateCrrd).FirstOrDefault();

            //res.RmPaymentCertificateCrr = (from r in _context.RmPaymentCertificateCrr
            //                               where r.CrrPcmamwPkRefNo == id
            //                               select new RmPaymentCertificateCrr
            //                               {
            //                                   CrrContractRate = r.CrrContractRate,
            //                                   CrrDivision = r.CrrDivision,
            //                                   CrrPaved = r.CrrPaved,
            //                                   CrrPcmamwPkRefNo = r.CrrPcmamwPkRefNo,
            //                                   CrrPkRefNo = r.CrrPkRefNo,
            //                                   CrrSubTotal = r.CrrSubTotal,
            //                                   CrrTotalAmount = r.CrrTotalAmount,
            //                                   CrrUnpaved = r.CrrUnpaved
            //                               }).ToList();





            return res;
        }


        public async Task<int> SaveFormPA(RmPaymentCertificateMamw FormPA, bool update = false)
        {
            try
            {
                if (!update)
                    _context.RmPaymentCertificateMamw.Add(FormPA);
                else
                    _context.RmPaymentCertificateMamw.Update(FormPA);
                _context.SaveChanges();
                return FormPA.PcmamwPkRefNo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateFormPA(RmPaymentCertificateMamw FormPAHeader, List<RmPaymentCertificateCrr> FormPACrr, List<RmPaymentCertificateCrra> FormPACrra, List<RmPaymentCertificateCrrd> FormPACrrd)
        {
            try
            {
                IList<RmPaymentCertificateCrr> child = (from r in _context.RmPaymentCertificateCrr where r.CrrPcmamwPkRefNo == FormPAHeader.PcmamwPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                   // _context.SaveChanges();
                }

                foreach (var item in FormPACrr)
                {
                    item.CrrPcmamwPkRefNo = FormPAHeader.PcmamwPkRefNo;
                    item.CrrPkRefNo = 0;
                    _context.RmPaymentCertificateCrr.Add(item);
                    _context.SaveChanges();
                }

                IList<RmPaymentCertificateCrrd> child2 = (from r in _context.RmPaymentCertificateCrrd where r.CrrdPcmamwPkRefNo == FormPAHeader.PcmamwPkRefNo select r).ToList();
                foreach (var item in child2)
                {
                    _context.Remove(item);
                   // _context.SaveChanges();
                }

                foreach (var item in FormPACrrd)
                {
                    item.CrrdPcmamwPkRefNo = FormPAHeader.PcmamwPkRefNo;
                    item.CrrdPkRefNo = 0;
                    _context.RmPaymentCertificateCrrd.Add(item);
                    _context.SaveChanges();
                }
                

                IList<RmPaymentCertificateCrra> child3 = (from r in _context.RmPaymentCertificateCrra where r.CrraPcmamwPkRefNo == FormPAHeader.PcmamwPkRefNo select r).ToList();
                foreach (var item in child3)
                {
                    _context.Remove(item);
                   // _context.SaveChanges();
                }

                foreach (var item in FormPACrra)
                {
                    item.CrraPcmamwPkRefNo = FormPAHeader.PcmamwPkRefNo;
                    item.CrraPkRefNo = 0;
                    _context.RmPaymentCertificateCrra.Add(item);
                    _context.SaveChanges();
                }


                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int? DeleteFormPA(int id)
        {
            try
            {
                IList<RmPaymentCertificateCrr> crr = (from r in _context.RmPaymentCertificateCrr where r.CrrPcmamwPkRefNo == id select r).ToList();
                foreach (var item in crr)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmPaymentCertificateCrra> crra = (from r in _context.RmPaymentCertificateCrra where r.CrraPcmamwPkRefNo == id select r).ToList();
                foreach (var item in crra)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmPaymentCertificateCrrd> crrd = (from r in _context.RmPaymentCertificateCrrd where r.CrrdPcmamwPkRefNo == id select r).ToList();
                foreach (var item in crrd)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmPaymentCertificateMamw> Mamw = (from r in _context.RmPaymentCertificateMamw where r.PcmamwPkRefNo == id select r).ToList();
                foreach (var item in Mamw)
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
