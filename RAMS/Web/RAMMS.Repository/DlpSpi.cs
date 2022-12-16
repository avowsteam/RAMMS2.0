﻿using System;
using RAMMS.Domain.Models;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Reflection;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO;

namespace RAMMS.Repository
{
    public class DlpSpi : RepositoryBase<RmDlpSpi>, IDlpSpi
    {
        public DlpSpi(RAMMSContext context) : base(context) { _context = context ?? throw new ArgumentNullException(nameof(context)); }

        public async Task<List<RmDlpSpi>> GetDivisionMiri(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year select r).ToListAsync();

            var TotalSumMonthlyPlanned = res.Select(x => x.SpiMPlanned).Sum();
            TotalSumMonthlyPlanned = TotalSumMonthlyPlanned >0 ? TotalSumMonthlyPlanned : null;
            var SpiCPlan = res.Select(x => x.SpiCPlan).Sum();


            var list = res
                    .Select(x => new
                    {
                        x.SpiMonth,
                        x.SpiMPlanned,
                        x.SpiMActual,
                        x.SpiCPlan,
                        x.SpiCActual,
                        x.SpiPiWorkActual,
                        x.SpiPai,
                        x.SpiEff,
                        x.SpiRb,
                        x.SpiIw,
                        x.SpiSpi,
                        x.SpiPlannedPer,
                        x.SpiActualPer

                    })
                    .GroupBy(x => x.SpiMonth)
                    .Select(
                      x => new RmDlpSpi
                      {
                          SpiMonth = x.Select(x => x.SpiMonth).First(),
                          SpiMPlanned = (x.Sum(c => c.SpiMPlanned)),
                          SpiMActual = (x.Sum(c => c.SpiMActual)),
                          SpiCPlan = (x.Sum(y => y.SpiCPlan)),
                          SpiCActual = (x.Sum(y => y.SpiCActual)),
                          SpiPiWorkActual = x.Sum(y => y.SpiCPlan) > 0 ?  Math.Round((decimal)(((x.Sum(y => y.SpiCActual) / x.Sum(y => y.SpiCPlan * 80)))), 2): 0,// x.Sum(y => y.SpiPiWorkActual),
                          SpiPai = x.Sum(y => y.SpiCPlan) > 0 ?  Math.Round((decimal)(x.Sum(y => y.SpiCActual) /  x.Sum(y => y.SpiCPlan)), 2) : 0,
                          SpiEff = Math.Round((decimal)(x.Sum(y => y.SpiEff) / 2), 2) ,
                          SpiRb = Math.Round((decimal)(x.Sum(y => y.SpiRb) / 2), 2),
                          SpiIw = Math.Round((decimal)(x.Sum(y => y.SpiIw) / 2), 2),
                          SpiSpi =  Math.Round((decimal)((x.Sum(y => y.SpiCPlan) > 0 ? (x.Sum(y => y.SpiCActual) /  x.Sum(y => y.SpiCPlan)) : 0 * 80) + (x.Sum(y => y.SpiEff) / 2)), 2) + (x.Sum(y => y.SpiRb) / 2) - (x.Sum(y => y.SpiIw) / 2),
                          SpiPlannedPer = TotalSumMonthlyPlanned > 0 ? Math.Round((decimal)((x.Sum(y => y.SpiCPlan) / TotalSumMonthlyPlanned) * 100), 2) : 0 ,
                          SpiActualPer = TotalSumMonthlyPlanned > 0 ?Math.Round((decimal)((x.Sum(y => y.SpiCActual) / TotalSumMonthlyPlanned)) * 100, 2) : 0,
                      }
                    )
                    .ToList();
            return list;
        }

        public async Task<List<RmDlpSpi>> GetDivisionRMUMiri(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year && r.SpiDivCode == "Miri" select r).ToListAsync();
            return res;
        }

        public async Task<List<RmDlpSpi>> GetDivisionRMUBTN(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year && r.SpiDivCode == "BTN" select r).ToListAsync();
            return res;
        }

        public async Task<int> Save(List<SpiData> spiData)
        {

            try
            {
                foreach (var spi in spiData)
                {
                    var form = await _context.RmDlpSpi.Where(x => x.SpiPkRefNo == spi.id).FirstOrDefaultAsync();
                    form.SpiEff = Convert.ToDecimal(spi.eff);
                    form.SpiRb = Convert.ToDecimal(spi.rb);
                    form.SpiIw = Convert.ToDecimal(spi.iw);
                    form.SpiSpi = Convert.ToDecimal(spi.spi);

                    _context.RmDlpSpi.Attach(form);
                    var entry = _context.Entry(form);
                    entry.Property(x => x.SpiEff).IsModified = true;
                    entry.Property(x => x.SpiRb).IsModified = true;
                    entry.Property(x => x.SpiIw).IsModified = true;
                    entry.Property(x => x.SpiSpi).IsModified = true;
                    _context.SaveChanges();

                }
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public async Task<int> SyncMiri(int year)
        {

            var context = _context;
            {
                var connection = context.Database.GetDbConnection();

                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_syncspi_miri";
                    cmd.CommandTimeout = 2000;
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@year";
                    p.Value = year;
                    cmd.Parameters.Add(p);
                    connection.Open();
                    int ret = await cmd.ExecuteNonQueryAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    return 500;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<int> SyncBTN(int year)
        {

            var context = _context;
            {
                var connection = context.Database.GetDbConnection();

                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_syncspi_btn";
                    cmd.CommandTimeout = 2000;
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@year";
                    p.Value = year;
                    cmd.Parameters.Add(p);
                    connection.Open();
                    int ret = await cmd.ExecuteNonQueryAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    return 500;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        #region RMI & IRI
        public async Task<int> SaveIRI(List<DlpIRIDTO> iRIData)
        {
            try
            {
                if (_context.RmRmiIri.Count(a => a.RmiiriYear == iRIData.FirstOrDefault().RmiiriYear) == 0)
                {
                    foreach (var spi in iRIData)
                    {
                        var form = await _context.RmRmiIri.Where(a => a.RmiiriPkRefNo == spi.RmiiriPkRefNo).FirstOrDefaultAsync();
                        if (form == null)
                            form = new RmRmiIri();
                        form.RmiiriYear = spi.RmiiriYear;
                        form.RmiiriMonth = spi.RmiiriMonth;
                        form.RmiiriConditionNo = spi.RmiiriConditionNo;
                        form.RmiiriType = spi.RmiiriType;
                        form.RmiiriPercentage = spi.RmiiriPercentage;
                        form.RmiiriRoadLength = spi.RmiiriRoadLength;
                        if (form.RmiiriPkRefNo != 0)
                        {
                            form.RmiiriUpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            form.RmiiriCreatedDate = DateTime.Now;
                        }
                        _context.RmRmiIri.Add(form);
                        _context.SaveChanges();

                    }
                }
                else
                {
                    int conditionNo = 1;
                    foreach (var spi in iRIData)
                    {
                        if (spi.RmiiriType == "RMI")
                        {
                            conditionNo = 1;
                        }
                        else if (spi.RmiiriType == "IRI")
                        {
                            if (spi.RmiiriRoadLength1.HasValue && spi.RmiiriRoadLength1.Value != 0)
                            {
                                conditionNo = 1;
                            }
                            if (spi.RmiiriRoadLength2.HasValue && spi.RmiiriRoadLength2.Value != 0)
                            {
                                conditionNo = 2;
                            }
                            if (spi.RmiiriRoadLength3.HasValue && spi.RmiiriRoadLength3.Value != 0)
                            {
                                conditionNo = 3;
                            }
                        }
                        var form = await _context.RmRmiIri.Where(a => a.RmiiriYear == spi.RmiiriYear && a.RmiiriType == spi.RmiiriType && a.RmiiriConditionNo == conditionNo).FirstOrDefaultAsync();
                        if (form != null)
                        {
                            form.RmiiriYear = spi.RmiiriYear;
                            form.RmiiriMonth = spi.RmiiriMonth;
                            form.RmiiriConditionNo = spi.RmiiriConditionNo;
                            form.RmiiriType = spi.RmiiriType;
                            form.RmiiriPercentage = spi.RmiiriPercentage;
                            form.RmiiriRoadLength = spi.RmiiriRoadLength;
                            form.RmiiriUpdatedDate = DateTime.Now;
                            _context.SaveChanges();
                        }

                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 500;
            }
        }

        public async Task<List<RmRmiIri>> GetFilteredRecordList(FilteredPagingDefinition<FormASearchGridDTO> filterOptions)
        {
            List<RmRmiIri> result = new List<RmRmiIri>();
            var query = (from x in _context.RmRmiIri
                         select new { x });
            result = await query.Select(s => s.x).ToListAsync();

            if (filterOptions.Filters != null && filterOptions.Filters.Year != null)
                result = result.Where(a => a.RmiiriYear == filterOptions.Filters.Year).ToList();
            //result = await query.Select(s => s.x).Skip(filterOptions.StartPageNo)
            //                    .Take(filterOptions.RecordsPerPage)
            //                    .ToListAsync();
            return result;
        }

        public async Task<int> GetFilteredRecordCount(FilteredPagingDefinition<FormASearchGridDTO> filterOptions)
        {
            var query = (from x in _context.RmRmiIri
                         select new { x });
            return await query.CountAsync().ConfigureAwait(false);
        }

        public async Task<List<RmRmiIri>> GetIRIData(int year)
        {
            List<RmRmiIri> result = new List<RmRmiIri>();
            var query = (from x in _context.RmRmiIri.Where(a => a.RmiiriYear.Value == year)
                         select new { x });

            result = await query.Select(s => s.x)
                                .ToListAsync();
            return result;
        }

        public int? DeleteFormIRI(int id)
        {
            try
            {
                var rmRmiIri = _context.RmRmiIri.Where(x => x.RmiiriYear == id).ToList();
                if (!rmRmiIri.Any())
                    return null;
                _context.RmRmiIri.RemoveRange(rmRmiIri);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 500;
            }
        }
        #endregion
    }
}