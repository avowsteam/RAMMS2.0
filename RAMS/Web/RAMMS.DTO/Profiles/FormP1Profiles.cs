using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormP1Profiles : Profile
    {
        public FormP1Profiles()
        {
            string[] arrPrefix = new string[] { "Pch", "Pc" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormP1ResponseDTO, RmPaymentCertificate>().ReverseMap();
            this.CreateMap<FormP1HeaderResponseDTO, RmPaymentCertificateHeader>().ReverseMap();
        }
    }
}
