using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB14Profiles : Profile
    {
        public FormB14Profiles()
        {
            string[] arrPrefix = new string[] { "B14h", "B14hh"  };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB14HeaderDTO, RmB14Hdr>().ReverseMap();
            this.CreateMap<FormB14HistoryDTO, RmB14History>().ReverseMap();
        }
    }
}
