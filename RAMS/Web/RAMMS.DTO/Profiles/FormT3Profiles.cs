using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormT3Profiles : Profile
    {
        public FormT3Profiles()
        {
            string[] arrPrefix = new string[] { "T3H", "T3HH" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormT3HeaderDTO, RmT3Hdr>().ReverseMap();
            this.CreateMap<FormT3HistoryDTO, RmT3History>().ReverseMap();
        }
    }
}
