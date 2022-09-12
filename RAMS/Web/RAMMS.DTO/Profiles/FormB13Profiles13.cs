using AutoMapper;
using RAMMS.Domain.Models;
using RAMMS.DTO.ResponseBO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO.Profiles
{
    public class FormB13Profiles : Profile
    {
        public FormB13Profiles()
        {
            string[] arrPrefix = new string[] { "B13p", "B13ph", "B13rh" };
            this.RecognizeDestinationPrefixes(arrPrefix);
            this.RecognizePrefixes(arrPrefix);
            this.CreateMap<FormB13ResponseDTO, RmB13ProposedPlannedBudget>().ReverseMap();
            this.CreateMap<FormB13HistoryResponseDTO, RmB13ProposedPlannedBudgetHistory>().ReverseMap();
            this.CreateMap<FormB13HistoryRevisionResponseDTO, RmB13RevisionHistory>().ReverseMap();
        }
    }
}
