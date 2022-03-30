﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAMMS.Domain.Models;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace RAMMS.Repository
{
    public class ModuleFormRightsRepository : RepositoryBase<RmModuleRightByForm>, IModuleFormRightsRepository
    {

        public ModuleFormRightsRepository(RAMMSContext context) : base(context) { _context = context ?? throw new ArgumentNullException(nameof(context)); }


        public IList<RmModuleRightByForm> GetIWRightsByUser(int UserID)
        {
            return (from s in _context.RmGroupUser where s.RmUsersUsrPkId == UserID
                    join uug in _context.RmModuleRightByForm on s.RmGroupsUgPkId equals uug.MfrUgPkId
                    select uug).ToListAsync().Result;
        }


        public IList<RmModuleRightByForm> GetIWRightsAll()
        {
            return _context.RmModuleRightByForm.ToListAsync().Result;
        }

        public string GetUserGroupName(string code)
        {
            return _context.RmGroup.Where(x => x.UgGroupCode == code).Select(z => z.UgGroupName).FirstOrDefault();
        }

    }
}