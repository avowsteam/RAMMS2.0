﻿using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class MAMVProfiles : Profile
    {
        public MAMVProfiles()
        {
            string[] arrPrefix = new string[] { "Fv1h", "Fv1d", "Fv2h", "Fv2e", "Fv2l", "Fv2m","Fv3h","Fv3d","Fv4h", "Fv5h", "Fv5d", "Fsiids" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);

            this.CreateMap<FormV1ResponseDTO, RmFormV1Hdr>().ReverseMap();
            this.CreateMap<FormV1DtlResponseDTO, RmFormV1Dtl>().ReverseMap();
            this.CreateMap<FormV2HeaderResponseDTO, RmFormV2Hdr>().ReverseMap();
            this.CreateMap<FormV2EquipDetailsResponseDTO, RmFormV2Eqp>().ReverseMap();
            this.CreateMap<FormV2LabourDetailsResponseDTO, RmFormV2Lab>().ReverseMap();
            this.CreateMap<FormV2MaterialDetailsResponseDTO, RmFormV2Mat>().ReverseMap();
            this.CreateMap<FormV3ResponseDTO, RmFormV3Hdr>().ReverseMap();
            this.CreateMap<FormV3DtlGridDTO, RmFormV3Dtl>().ReverseMap();
            this.CreateMap<FormV4ResponseDTO, RmFormV4Hdr>().ReverseMap();
            this.CreateMap<FormV5ResponseDTO, RmFormV5Hdr>().ReverseMap();
            this.CreateMap<FormV5DtlResponseDTO, RmFormV5Dtl>().ReverseMap();
            this.CreateMap<FormS2DayScheduleDTO, RmFormS2DaySchedule>().ReverseMap();
        }
    }
}