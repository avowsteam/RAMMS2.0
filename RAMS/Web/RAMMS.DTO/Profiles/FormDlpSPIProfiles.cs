using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormDlpSPIProfiles : Profile
    {
        public FormDlpSPIProfiles()
        {
            string[] arrPrefix = new string[] { "Spi" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<DlpSPIDTO, RmDlpSpi>().ReverseMap();
          
        }
    }
}
