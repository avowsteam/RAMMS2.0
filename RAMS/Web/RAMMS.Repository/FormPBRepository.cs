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
    public class FormPBRepository : RepositoryBase<RmPbIw>, IFormPBRepository
    {
        public FormPBRepository(RAMMSContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FormPBHeaderResponseDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormPBSearchGridDTO> filterOptions)
        {


            var query = (from hdr in _context.RmPbIw
                         from dtl in _context.RmPbIwDetails.Where(a => a.PbiwdPbiwPkRefNo == 0).DefaultIfEmpty()
                         select new { hdr, dtl });



            query = query.OrderByDescending(x => x.hdr.PbiwPkRefNo);

          

            if (filterOptions.Filters.ProjectTitle != null && filterOptions.Filters.ProjectTitle != string.Empty)
            {
                query = (from hdr in _context.RmPbIw
                         from dtl in _context.RmPbIwDetails.Where(a => a.PbiwdPbiwPkRefNo == hdr.PbiwPkRefNo).DefaultIfEmpty()
                         where dtl.PbiwdProjectTitle.Contains(filterOptions.Filters.ProjectTitle)
                         select new { hdr, dtl });
            }

            if (filterOptions.Filters.IWRefNo != null && filterOptions.Filters.IWRefNo != string.Empty)
            {
                query = query.Where(s => s.hdr.PbiwRefId == filterOptions.Filters.IWRefNo);
            }


            if ((filterOptions.Filters.YearTo == null || filterOptions.Filters.YearTo == string.Empty) && (filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty))
                query = query.Where(s => s.hdr.PbiwSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && filterOptions.Filters.YearFrom != null && filterOptions.Filters.YearFrom != string.Empty)
                query = query.Where(s => s.hdr.PbiwSubmissionYear >= Convert.ToInt32(filterOptions.Filters.YearFrom) && s.hdr.PbiwSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));
            else if ((filterOptions.Filters.YearTo != null || filterOptions.Filters.YearTo != string.Empty) && (filterOptions.Filters.YearFrom == null && filterOptions.Filters.YearFrom == string.Empty))
                query = query.Where(s => s.hdr.PbiwSubmissionYear <= Convert.ToInt32(filterOptions.Filters.YearTo));


            if ((filterOptions.Filters.MonthTo == null || filterOptions.Filters.MonthTo == string.Empty) && (filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty))
                query = query.Where(s => s.hdr.PbiwSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && filterOptions.Filters.MonthFrom != null && filterOptions.Filters.MonthFrom != string.Empty)
                query = query.Where(s => s.hdr.PbiwSubmissionMonth >= Convert.ToInt32(filterOptions.Filters.MonthFrom) && s.hdr.PbiwSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));
            else if ((filterOptions.Filters.MonthTo != null || filterOptions.Filters.MonthTo != string.Empty) && (filterOptions.Filters.MonthFrom == null && filterOptions.Filters.MonthFrom == string.Empty))
                query = query.Where(s => s.hdr.PbiwSubmissionMonth <= Convert.ToInt32(filterOptions.Filters.MonthTo));


            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                query = query.Where(s =>
               (s.hdr.PbiwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.dtl.PbiwdProjectTitle.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwSubmissionYear.HasValue ? s.hdr.PbiwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.hdr.PbiwSubmissionMonth.HasValue ? s.hdr.PbiwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
            }

            if (!string.IsNullOrEmpty(filterOptions.Filters.SmartSearch))
            {
                DateTime dt;
                if (DateTime.TryParseExact(filterOptions.Filters.SmartSearch, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                {
                    query = query.Where(s =>
               (s.hdr.PbiwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.dtl.PbiwdProjectTitle.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwSubmissionYear.HasValue ? s.hdr.PbiwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.hdr.PbiwSubmissionMonth.HasValue ? s.hdr.PbiwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.hdr.PbiwSignDateSo.HasValue ? (s.hdr.PbiwSignDateSo.Value.Year == dt.Year && s.hdr.PbiwSignDateSo.Value.Month == dt.Month && s.hdr.PbiwSignDateSo.Value.Day == dt.Day) : true) && s.hdr.PbiwSignDateSo != null);
                }
                else
                {
                    query = query.Where(s =>
               (s.hdr.PbiwRefId.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwStatus.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.dtl.PbiwdProjectTitle.Contains(filterOptions.Filters.SmartSearch)) ||
               (s.hdr.PbiwSubmissionYear.HasValue ? s.hdr.PbiwSubmissionYear.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch) ||
               (s.hdr.PbiwSubmissionMonth.HasValue ? s.hdr.PbiwSubmissionMonth.Value.ToString() : "").Contains(filterOptions.Filters.SmartSearch));
                }
            }


            if (filterOptions.sortOrder == SortOrder.Ascending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderBy(s => s.hdr.PbiwRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderBy(s => s.hdr.PbiwSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderBy(s => s.hdr.PbiwSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderBy(s => s.hdr.PbiwSignDateSo);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderBy(s => s.hdr.PbiwAmountBeforeLad);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderBy(s => s.hdr.PbiwLaDamage);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderBy(s => s.hdr.PbiwFinalPayment);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderBy(s => s.hdr.PbiwStatus);


            }
            else if (filterOptions.sortOrder == SortOrder.Descending)
            {
                if (filterOptions.ColumnIndex == 2)
                    query = query.OrderByDescending(s => s.hdr.PbiwRefId);
                if (filterOptions.ColumnIndex == 3)
                    query = query.OrderByDescending(s => s.hdr.PbiwSubmissionYear);
                if (filterOptions.ColumnIndex == 4)
                    query = query.OrderByDescending(s => s.hdr.PbiwSubmissionMonth);
                if (filterOptions.ColumnIndex == 5)
                    query = query.OrderByDescending(s => s.hdr.PbiwSignDateSo);
                if (filterOptions.ColumnIndex == 6)
                    query = query.OrderByDescending(s => s.hdr.PbiwAmountBeforeLad);
                if (filterOptions.ColumnIndex == 7)
                    query = query.OrderByDescending(s => s.hdr.PbiwLaDamage);
                if (filterOptions.ColumnIndex == 8)
                    query = query.OrderByDescending(s => s.hdr.PbiwFinalPayment);
                if (filterOptions.ColumnIndex == 9)
                    query = query.OrderByDescending(s => s.hdr.PbiwStatus);

            }



            var list = query.Select(s => new FormPBHeaderResponseDTO
            {
                PkRefNo = s.hdr.PbiwPkRefNo,
                RefId = s.hdr.PbiwRefId,
                SubmissionYear = s.hdr.PbiwSubmissionYear,
                SubmissionMonth = s.hdr.PbiwSubmissionMonth,
                SubmissionDate = s.hdr.PbiwSignDateSo,
                AmountBeforeLad = s.hdr.PbiwAmountBeforeLad,
                LaDamage = s.hdr.PbiwLaDamage,
                FinalPayment = s.hdr.PbiwFinalPayment,
                Status = s.hdr.PbiwStatus
            }).ToList();


            return list.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToList();


        }



        public RmPbIw GetHeaderById(int id)
        {
            RmPbIw res = (from r in _context.RmPbIw where r.PbiwPkRefNo == id select r).FirstOrDefault();

            res.RmPbIwDetails = (from r in _context.RmPbIwDetails
                                 where r.PbiwdPbiwPkRefNo == id
                                 select new RmPbIwDetails
                                 {
                                     PbiwdCompletionDate = r.PbiwdCompletionDate,
                                     PbiwdAmountBeforeLad = r.PbiwdAmountBeforeLad,
                                     PbiwdCompletionRefNo = r.PbiwdCompletionRefNo,
                                     PbiwdFinalPayment = r.PbiwdFinalPayment,
                                     PbiwdIwRef = r.PbiwdIwRef,
                                     PbiwdLaDamage = r.PbiwdLaDamage,
                                     PbiwdPbiwPkRefNo = r.PbiwdPbiwPkRefNo,
                                     PbiwdPkRefNo = r.PbiwdPkRefNo,
                                     PbiwdProjectTitle = r.PbiwdProjectTitle
                                 }).ToList();

            return res;
        }


        public async Task<int> SaveFormPB(RmPbIw FormPB, bool update = false)
        {
            try
            {
                if (!update)
                    _context.RmPbIw.Add(FormPB);
                else
                    _context.RmPbIw.Update(FormPB);
                _context.SaveChanges();

                return FormPB.PbiwPkRefNo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<int> UpdateFormPB(RmPbIw FormPBHeader, List<RmPbIwDetails> FormPBDetails)
        {
            try
            {

                IList<RmPbIwDetails> child = (from r in _context.RmPbIwDetails where r.PbiwdPbiwPkRefNo == FormPBHeader.PbiwPkRefNo select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                foreach (var item in FormPBDetails)
                {
                    item.PbiwdPbiwPkRefNo = FormPBHeader.PbiwPkRefNo;
                    item.PbiwdPkRefNo = 0;
                    _context.RmPbIwDetails.Add(item);
                    _context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int? DeleteFormPB(int id)
        {
            try
            {
                IList<RmPbIwDetails> child = (from r in _context.RmPbIwDetails where r.PbiwdPbiwPkRefNo == id select r).ToList();
                foreach (var item in child)
                {
                    _context.Remove(item);
                    _context.SaveChanges();
                }

                IList<RmPbIw> parent = (from r in _context.RmPbIw where r.PbiwPkRefNo == id select r).ToList();
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
