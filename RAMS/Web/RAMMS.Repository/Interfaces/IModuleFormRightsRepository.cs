﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAMMS.Domain.Models;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.Wrappers;

namespace RAMMS.Repository.Interfaces
{
    public interface IModuleFormRightsRepository : IRepositoryBase<RmModuleRightByForm>
    {

        IList<RmModuleRightByForm> GetIWRightsAll(int UserID);
    }
}
