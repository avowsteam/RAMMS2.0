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

namespace RAMMS.Repository
{
    public class DlpSpi : RepositoryBase<RmDlpSpi>, IDlpSpi
    {
        public DlpSpi(RAMMSContext context) : base(context) { _context = context ?? throw new ArgumentNullException(nameof(context)); }

        public async Task<List<RmDlpSpi>> GetDivisionMiri(int year)
        {
            List<RmDlpSpi> res =await  (from r in _context.RmDlpSpi where r.SpiYear == year select r).ToListAsync();
            return res;
        }

        public async Task<List<RmDlpSpi>> GetDivisionRMUMiri(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year && r.SpiDivCode == "Miri" select r).ToListAsync();
            return res;
        }

        public async Task<List<RmDlpSpi>> GetDivisionRMUBTN(int year)
        {
            List<RmDlpSpi> res = await (from r in _context.RmDlpSpi where r.SpiYear == year && r.SpiDivCode == "Batu Niah" select r).ToListAsync();
            return res;
        }

    }
}
