using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormT4Profiles : Profile
    {
        public FormT4Profiles()
        {
            string[] arrPrefix = new string[] { "T4dbh", "T4db" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormT4HeaderResponseDTO, RmT4DesiredBdgtHeader>().ReverseMap();
            this.CreateMap<FormT4ResponseDTO, RmT4DesiredBdgt>().ReverseMap();
           
        }
    }
}
