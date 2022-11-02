using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormMapProfiles : Profile
    {
        public FormMapProfiles()
        {
            string[] arrPrefix = new string[] { "RMMH", "RMMD" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormMapHeaderDTO, RmMapHeader>().ReverseMap();
            this.CreateMap<FormMapDetailsDTO, RmMapDetails>().ReverseMap();
        }
    }
}
