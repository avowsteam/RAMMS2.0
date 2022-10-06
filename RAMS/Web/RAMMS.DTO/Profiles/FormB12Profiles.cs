using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB12Profiles : Profile
    {
        public FormB12Profiles()
        {
            string[] arrPrefix = new string[] { "B12h", "B12dslh" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB12DTO, RmB12Hdr>().ReverseMap();
            this.CreateMap<FormB12HistoryDTO, RmB12DesiredServiceLevelHistory>().ReverseMap();
           
        }
    }
}
