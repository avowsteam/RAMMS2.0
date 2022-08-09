using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB7Profiles : Profile
    {
        public FormB7Profiles()
        {
            string[] arrPrefix = new string[] { "B7h", "B7lh", "B7mh", "B7eh" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB7HeaderDTO, RmB7Hdr>().ReverseMap();
            this.CreateMap<FormB7LabourHistoryDTO, RmB7LabourHistory>().ReverseMap();
            this.CreateMap<FormB7MaterialHistoryDTO, RmB7MaterialHistory>().ReverseMap();
            this.CreateMap<FormB7EquipmentsHistoryDTO, RmB7EquipmentsHistory>().ReverseMap();
        }
    }
}
