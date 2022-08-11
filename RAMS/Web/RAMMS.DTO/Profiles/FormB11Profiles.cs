using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB11Profiles : Profile
    {
        public FormB11Profiles()
        {
            string[] arrPrefix = new string[] { "B11h", "B11CDCH", "B11LC", "B11EC", "B11MC" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB11DTO, RmB11Hdr>().ReverseMap();
            this.CreateMap<FormB11CrewDayCostHeaderDTO, RmB11CrewDayCostHeader>().ReverseMap();
            this.CreateMap<FormB11LabourCostDTO, RmB11LabourCost>().ReverseMap();
            this.CreateMap<FormB11EquipmentCostDTO, RmB11EquipmentCost>().ReverseMap();
            this.CreateMap<FormB11MaterialCostDTO, RmB11MaterialCost>().ReverseMap();
        }
    }
}
