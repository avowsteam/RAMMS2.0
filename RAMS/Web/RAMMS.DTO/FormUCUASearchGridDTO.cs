using System;

namespace RAMMS.DTO
{
   public class FormUCUASearchGridDTO
    {
        public string SmartSearch { get; set; }
        public string RefNo { get; set; }
        public string ReportingName { get; set; }
        
        public string Location { get; set; }
        public string WorkScope { get; set; }
        public string ReceivedDtFrom { get; set; }
        public string ReceivedDtTo { get; set; }
        public bool SubmitSts { get; set; }
        public string Status { get; set; }

    }
}
