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
    public class FormP1Repository : RepositoryBase<RmPaymentCertificateHeader>, IFormP1Repository
    {
        public FormP1Repository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormP1HeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormP1SearchGridDTO> filterOptions)
        {


            var query = (from x in _context.RmPaymentCertificateHeader

                         select new { x });


            query = query.OrderByDescending(x => x.x.PchPkRefNo);

            if (filterOptions.Filters.CertificateNo != null && filterOptions.Filters.CertificateNo != string.Empty)
            {
                query = query.Where(s => s.x.PchPaymentCertificateNo == Convert.ToInt32(filterOptions.Filters.CertificateNo));
            }


            if ((filterOptions.Filters.YearTo == null || filterOptions.Filters.YearTo == string.Empty) && (filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty))
                query = query.Where(s => s.x.PchSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty)
                query = query.Where(s => s.x.PchSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom) && s.x.PchSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && (filterOptions.Filters.YearFrom == null && filterOptions.Filters.YearFrom == string.Empty))
                query = query.Where(s => s.x.PchSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));


            if ((filterOptions.Filters.MonthTo == null || filterOptions.Filters.MonthTo == string.Empty) && (filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty))
                query = query.Where(s => s.x.PchSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty)
                query = query.Where(s => s.x.PchSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom) && s.x.PchSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && (filterOptions.Filters.MonthFrom == null && filterOptions.Filters.MonthFrom == string.Empty))
                query = query.Where(s => s.x.PchSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                query = query.Where(s =>
               (s.x.PchRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchPaymentCertificateNo.HasValue ? s.x.PchPaymentCertificateNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionYear.HasValue ? s.x.PchSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionMonth.HasValue ? s.x.PchSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
            }

            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
               (s.x.PchRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchPaymentCertificateNo.HasValue ? s.x.PchPaymentCertificateNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionYear.HasValue ? s.x.PchSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionMonth.HasValue ? s.x.PchSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch)||
               (s.x.PchSubmissionDate.HasValue ? (s.x.PchSubmissionDate.Value.Year == dt.Year && s.x.PchSubmissionDate.Value.Month == dt.Month && s.x.PchSubmissionDate.Value.Day == dt.Day) : true) && s.x.PchSubmissionDate != null);
                }
                else
                {
                    query = query.Where(s =>
               (s.x.PchRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.x.PchPaymentCertificateNo.HasValue ? s.x.PchPaymentCertificateNo.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionYear.HasValue ? s.x.PchSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.x.PchSubmissionMonth.HasValue ? s.x.PchSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
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



            var list = query.Select(s => new FormP1HeaderResponseDTO
            {
                PkRefNo = s.x.PchPkRefNo,
                RefId = s.x.PchRefId,
                SubmissionYear = s.x.PchSubmissionYear,
                SubmissionMonth = s.x.PchSubmissionMonth,
                SubmissionDate = s.x.PchSignDateSo,
                PaymentCertificateNo = s.x.PchPaymentCertificateNo,
                TotalPayment = s.x.PchTotalPayment,
                Status = s.x.PchStatus,
                SubmitSts =s.x.PchSubmitSts
            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }



        public RmPaymentCertificateHeader GetHeaderById(int id)
        {
            RmPaymentCertificateHeader res = (from r in _context.RmPaymentCertificateHeader where r.PchPkRefNo == id select r).FirstOrDefault();

            var resPA = (from r in _context.RmPaymentCertificateMamw where r.PcmamwSubmissionYear == res.PchSubmissionYear && r.PcmamwSubmissionMonth == res.PchSubmissionMonth select r).FirstOrDefault();
            var resPB = (from r in _context.RmPbIw where r.PbiwSubmissionYear == res.PchSubmissionYear && r.PbiwSubmissionMonth == res.PchSubmissionMonth select r).FirstOrDefault();

            if (resPA != null)
            {
                res.PchContractRoadLength = resPA.PcmamwTotalPayment;
                res.PchNetValueDeduction = resPA.PcmamwWorkValueDeduction;
                res.PchNetValueAddition = resPA.PcmamwWorkValueAddition;
            }
            if (resPB != null)
            {
                res.PchNetValueInstructedWork = resPB.PbiwAmountBeforeLad;
                res.PchNetValueLadInstructedWork = resPB.PbiwLaDamage;
            }

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


        public async Task<int> SaveFormP1(RmPaymentCertificateHeader FormP1, bool update = false)
        {
            try
            {
                if (!update)
                    _context.RmPaymentCertificateHeader.Add(FormP1);
                else
                    _context.RmPaymentCertificateHeader.Update(FormP1);
                _context.SaveChanges();

                return FormP1.PchPkRefNo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
 

        public async Task<int> UpdateFormP1(RmPaymentCertificateHeader FormP1Header, List<RmPaymentCertificate> FormP1Details)
        {
            try
            {

                //_context.RmPaymentCertificateHeader.Add(FormP1Header);
                //_context.SaveChanges();

                IList<RmPaymentCertificate> child = (from r in _context.RmPaymentCertificate where r.PcPchPkRefNo == FormP1Header.PchPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var item in FormP1Details)
                {
                    item.PcPchPkRefNo = FormP1Header.PchPkRefNo;
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

        public int? DeleteFormP1(int id)
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
