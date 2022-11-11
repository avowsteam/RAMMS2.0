﻿using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IFormUCUAService
    {
        Task<FormUCUAResponseDTO> GetHeaderById(int id);
        Task<FormUCUAResponseDTO> SaveFormUCUA(FormUCUAResponseDTO FormUCUA);
        Task<int> Update(FormUCUAResponseDTO formUCUA);
        Task<byte[]> FormDownload(string formname, int id, string filepath);
        Task<PagingResult<FormUCUAHeaderRequestDTO>> GetHeaderList(FilteredPagingDefinition<FormUCUASearchGridDTO> filterOptions);
        int? DeleteFormUCUA(int id);
    }
}
