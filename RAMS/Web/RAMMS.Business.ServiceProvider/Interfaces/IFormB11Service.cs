using RAMMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RAMMS.DTO.ResponseBO;
using RAMMS.DTO.JQueryModel;
using RAMMS.DTO.Report;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO.RequestBO;


namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IFormB11Service
    {
        Task<FormB11DTO> GetHeaderById(int headerId,int IsEdit);
        Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData);
        int? GetMaxRev(int Year);
        Task<List<FormB7LabourHistoryDTO>> GetLabourHistoryData(int year);
        Task<List<FormB7MaterialHistoryDTO>> GetMaterialHistoryData(int year);
        Task<List<FormB7EquipmentsHistoryDTO>> GetEquipmentHistoryData(int year); 
        Task<List<FormB11LabourCostDTO>> GetLabourViewHistoryData(int id);
        Task<List<FormB11MaterialCostDTO>> GetMaterialViewHistoryData(int id);
        Task<List<FormB11EquipmentCostDTO>> GetEquipmentViewHistoryData(int id); 
        Task<int> SaveFormB11(FormB11DTO FormB11);
        Byte[] FormDownload(string formname, int id, string Rmucode, string basepath, string filepath);
    }
}
