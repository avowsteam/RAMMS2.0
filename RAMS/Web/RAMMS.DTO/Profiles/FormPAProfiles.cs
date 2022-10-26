using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormPAProfiles : Profile
    {
        public FormPAProfiles()
        {
            string[] arrPrefix = new string[] { "Pcmamw", "Crrd" , "Crra" , "Crr" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);

            this.CreateMap<FormPAHeaderResponseDTO, RmPaymentCertificateMamw>().ReverseMap();
            this.CreateMap<FormPACRRDResponseDTO, RmPaymentCertificateCrrd>().ReverseMap();
            this.CreateMap<FormPACRRDResponseDTO, RmPaymentCertificateCrrd>().ReverseMap();
            this.CreateMap<FormPACRRAResponseDTO, RmPaymentCertificateCrra>().ReverseMap();
            this.CreateMap<FormPACRRResponseDTO, RmPaymentCertificateCrr>().ReverseMap();
        }
    }
}
