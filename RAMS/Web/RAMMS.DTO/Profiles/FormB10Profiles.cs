using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB10Profiles : Profile
    {
        public FormB10Profiles()
        {
            string[] arrPrefix = new string[] { "B10dp", "B10dph"  };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB10ResponseDTO, RmB10DailyProduction>().ReverseMap();
            this.CreateMap<FormB10HistoryResponseDTO, RmB10DailyProductionHistory>().ReverseMap();
        }
    }
}
