using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB8Profiles : Profile
    {
        public FormB8Profiles()
        {
            string[] arrPrefix = new string[] { "B8h", "B8hi" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB8HeaderDTO, RmB8Hdr>().ReverseMap();
            this.CreateMap<FormB8HistoryDTO, RmB8History>().ReverseMap();
        }
    }
}
