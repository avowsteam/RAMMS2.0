using System;
using System.Collections.Generic;

namespace RAMMS.Domain.Models
{
    public partial class RmUcuaImage
    {
        public int UcuaPkRefNo { get; set; }
        public int? UcuaRmmhPkRefNo { get; set; }
        public string UcuaRmmhRefNo { get; set; }
        public string UcuaImgRefId { get; set; }
        public string UcuaImageTypeCode { get; set; }
        public int? UcuaImageSrno { get; set; }
        public string UcuaImageFilenameSys { get; set; }
        public string UcuaImageFilenameUpload { get; set; }
        public string UcuaImageUserFilePath { get; set; }
        public int? UcuaModBy { get; set; }
        public DateTime? UcuaModDt { get; set; }
        public int? UcuaCrBy { get; set; }
        public DateTime? UcuaCrDt { get; set; }
        public bool UcuaSubmitSts { get; set; }
        public bool UcuaActiveYn { get; set; }
        public string UcuaSource { get; set; }
    }
}
