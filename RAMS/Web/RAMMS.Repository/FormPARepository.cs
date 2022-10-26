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
    public class FormPARepository : RepositoryBase<RmPaymentCertificateHeader>, IFormPARepository
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
                    query = query.OrderBy(s => s.x.PchRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.x.PchSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.x.PchSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.x.PchSubmissionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.x.PchTotalPayment);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(s => s.x.PchStatus);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.x.PchRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.x.PchSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.x.PchSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.x.PchSubmissionDate);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.x.PchTotalPayment);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(s => s.x.PchStatus);

            }



            var list = query.Select(s => new FormPAHeaderResponseDTO
            {
                PkRefNo = s.x.PchPkRefNo,
                RefId = s.x.PchRefId,
                SubmissionYear = s.x.PchSubmissionYear,
                SubmissionMonth = s.x.PchSubmissionMonth,
                SubmissionDate = s.x.PchSubmissionDate,
                TotalPayment = s.x.PchTotalPayment,
                Status = s.x.PchStatus
            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }



        public RmPaymentCertificateHeader GetHeaderById(int id)
        {
            RmPaymentCertificateHeader res = (from r in _context.RmPaymentCertificateHeader where r.PchPkRefNo == id select r).FirstOrDefault();

            var resPA = (from r in _context.RmPaymentCertificateHeader where r.PchSubmissionYear == res.PchSubmissionYear && r.PchSubmissionMonth == res.PchSubmissionMonth select r).FirstOrDefault();
            var resPB = (from r in _context.RmPaymentCertificateHeader where r.PchSubmissionYear == res.PchSubmissionYear && r.PchSubmissionMonth == res.PchSubmissionMonth select r).FirstOrDefault();

            res.PchContractRoadLength = resPA.PchContractRoadLength;
            res.PchNetValueDeduction = resPA.PchNetValueDeduction;
            res.PchNetValueAddition = resPA.PchNetValueAddition;
            res.PchNetValueInstructedWork = resPB.PchNetValueInstructedWork;
            res.PchNetValueLadInstructedWork = resPB.PchNetValueLadInstructedWork;

            res.RmPaymentCertificate = (from r in _context.RmPaymentCertificate
                                        where r.PcPchPkRefNo == id
                                        select new RmPaymentCertificate
                                        {
                                            PcAddition = r.PcAddition,
                                            PcAmount = r.PcAmount,
                                            PcDeduction = r.PcDeduction,
                                            PcAmountIncludedInPc = r.PcAmountIncludedInPc,
                                            PcPaymentType = r.PcPaymentType,
                                            PcPchPkRefNo = r.PcPchPkRefNo,
                                            PcPchPkRefNoNavigation = r.PcPchPkRefNoNavigation,
                                            PcPkRefNo = r.PcPkRefNo,
                                            PcPreviousPayment = r.PcPreviousPayment,
                                            PcTotalToDate = r.PcTotalToDate
                                        }).ToList();

            return res;
        }


        public async Task<int> SaveFormPA(RmPaymentCertificateHeader FormPA)
        {
            try
            {

                _context.RmPaymentCertificateHeader.Add(FormPA);
                _context.SaveChanges();

                return FormPA.PchPkRefNo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateFormPA(RmPaymentCertificateHeader FormPAHeader, List<RmPaymentCertificate> FormPADetails)
        {
            try
            {

                //_context.RmPaymentCertificateHeader.Add(FormPAHeader);
                //_context.SaveChanges();

                IList<RmPaymentCertificate> child = (from r in _context.RmPaymentCertificate where r.PcPchPkRefNo == FormPAHeader.PchPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var item in FormPADetails)
                {
                    item.PcPchPkRefNo = FormPAHeader.PchPkRefNo;
                    item.PcPkRefNo = 0;
                    _context.RmPaymentCertificate.Add(item);
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
                IList<RmPaymentCertificate> child = (from r in _context.RmPaymentCertificate where r.PcPchPkRefNo == id select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmPaymentCertificateHeader> parent = (from r in _context.RmPaymentCertificateHeader where r.PchPkRefNo == id select r).ToList();
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
