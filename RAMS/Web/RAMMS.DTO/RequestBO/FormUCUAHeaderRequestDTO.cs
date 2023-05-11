using System;
using AutoMapper.Configuration.Conventions;

namespace RAMMS.DTO.RequestBO
{
  public class FormUCUAHeaderRequestDTO
    {
        public int PkRefNo { get; set; }
        public string RefId { get; set; }
        public string RMU { get; set; }
        public string RoadCode { get; set; }
        public string RoadName { get; set; }

        public int? TotalPC { get; set; }
        public int? TotalHV { get; set; }
        public int? TotalMC { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public bool? SubmitSts { get; set; }
        public string Recordedby { get; set; }
        public string Headedby { get; set; }
        public string ReportingName { get; set; }

        public string Location { get; set; }
        public string WorkScope { get; set; }
        public string ReceivedDtFrom { get; set; }
        public string ReceivedDtTo { get; set; }
    }
    public class FormUCUAImagesDTO
    {
        public int PkRefNo { get; set; }
        public int? RmmhPkRefNo { get; set; }
        public string RmmhRefNo { get; set; }
        public string ImgRefId { get; set; }
        public string ImageTypeCode { get; set; }
        public int? ImageSrno { get; set; }
        public string ImageFilenameSys { get; set; }
        public string ImageFilenameUpload { get; set; }
        public string ImageUserFilePath { get; set; }
        public int? ModBy { get; set; }
        public DateTime? ModDt { get; set; }
        public int? CrBy { get; set; }
        public DateTime? CrDt { get; set; }
        public bool SubmitSts { get; set; }
        public bool ActiveYn { get; set; }
        public string Source { get; set; }
    }
}
