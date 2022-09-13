using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB15Profiles : Profile
    {
        public FormB15Profiles()
        {
            string[] arrPrefix = new string[] { "B15H", "B15HH" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB15HeaderDTO, RmB15Hdr>().ReverseMap();
            this.CreateMap<FormB15HistoryDTO, RmB15History>().ReverseMap();            
        }
    }
}