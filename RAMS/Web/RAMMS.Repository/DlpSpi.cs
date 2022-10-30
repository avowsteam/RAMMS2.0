using System;
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

namespace RAMMS.Repository
{
    public class DlpSpi : RepositoryBase<RmDlpSpi>, IDlpSpi
    {
        public DlpSpi(RAMMSContext context) : base(context) { _context = context ?? throw new ArgumentNullException(nameof(context)); }

        public async Task<List<RmDlpSpi>> GetDivisionMiri(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year select r).ToListAsync();
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
                              SpiMPlanned = x.Sum(c => c.SpiMPlanned),
                              SpiMActual = x.Sum(c => c.SpiMActual),
                              SpiCPlan = x.Sum(y => y.SpiCPlan),
                              SpiCActual = x.Sum(y => y.SpiCPlan),
                              SpiPiWorkActual = x.Sum(y => y.SpiPiWorkActual),
                              SpiPai = x.Sum(y => y.SpiPai),
                              SpiEff = x.Sum(y => y.SpiEff),
                              SpiRb = x.Sum(y => y.SpiRb),
                              SpiIw = x.Sum(y => y.SpiIw),
                              SpiSpi = x.Sum(y => y.SpiSpi),
                              SpiPlannedPer = x.Sum(y => y.SpiPlannedPer),
                              SpiActualPer = x.Sum(y => y.SpiActualPer),
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

                    _context.RmDlpSpi.Attach(form);
                    var entry = _context.Entry(form);
                    entry.Property(x => x.SpiEff).IsModified = true;
                    entry.Property(x => x.SpiRb).IsModified = true;
                    entry.Property(x => x.SpiIw).IsModified = true;
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