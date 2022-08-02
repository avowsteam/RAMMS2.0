using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB9Profiles : Profile
    {
        public FormB9Profiles()
        {
            string[] arrPrefix = new string[] { "B9ds", "B9dsh"  };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB9ResponseDTO, RmB9DesiredService>().ReverseMap();
            this.CreateMap<FormB9HistoryResponseDTO, RmB9DesiredServiceHistory>().ReverseMap();
        }
    }
}
