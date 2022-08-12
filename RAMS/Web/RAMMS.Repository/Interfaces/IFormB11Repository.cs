﻿using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository.Interfaces
{
    public interface IFormB11Repository : IRepositoryBase<RmB11Hdr>
    {

        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        RmB11Hdr GetHeaderById(int id);
        int? GetMaxRev(int Year);
        List<RmB7LabourHistory> GetLabourHistoryData(int year);
        List<RmB11LabourCost> GetLabourViewHistoryData(int id); 
        Task<int> SaveFormB11(RmB11Hdr FormB11);
    }
}
