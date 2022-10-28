using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormPBProfiles : Profile
    {
        public FormPBProfiles()
        {
            string[] arrPrefix = new string[] { "Pbiw", "Pbiwd" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormPBHeaderResponseDTO, RmPbIw>().ReverseMap();
            this.CreateMap<FormPBDetailResponseDTO, RmPbIwDetails>().ReverseMap();
        }
    }
}
