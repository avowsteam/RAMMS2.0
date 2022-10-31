using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.Business.ServiceProvider;
using RAMMS.DTO;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.ResponseBO;


namespace RAMMS.Web.UI.Models
{
    public class FormPAModel
    {

        public FormPAHeaderResponseDTO FormPAHeader { get; set; }

        public List<FormPACRRResponseDTO> FormPACRR { get; set; }

        public List<FormPACRRAResponseDTO> FormPACRRA { get; set; }

        public List<FormPACRRDResponseDTO> FormPACRRD { get; set; }

        public int view { get; set; }


    }

 
}
