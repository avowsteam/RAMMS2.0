using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;
namespace RAMMS.DTO.Profiles
{
    public class FormUCUAProfiles: Profile
    {
        public FormUCUAProfiles()
        {
            string[] arrPrefix = new string[] { "Rmmh" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);

            this.CreateMap<FormUCUAResponseDTO, RmUcua>().ReverseMap();
           
        }
    }
}
