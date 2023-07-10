using System;
using System.Collections.Generic;
using System.Text;

namespace RAMMS.DTO
{
    public class MailRequestDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }       

        public void PrepareRequest(string emailTo, string emailSubject, string emaiBody)
        {
            this.ToEmail = emailTo;
            this.Subject = emailSubject;
            this.Body = emaiBody;
        }
    }
}
