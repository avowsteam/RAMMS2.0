using Microsoft.Extensions.Options;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Common;
using RAMMS.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Business.ServiceProvider.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequestDto mailRequest)
        {
            var mailSettingsUsed = string.Empty;
            try
            {  
                var smtpClient = new SmtpClient
                {
                    Host = _mailSettings.Host,
                    Port = _mailSettings.Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password)
                };

                MailMessage message = new MailMessage();               
                message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
                message.To.Add(mailRequest.ToEmail);
                //message.CC.Add("spandana.k@avowstech.com");
                message.Subject = mailRequest.Subject;
                message.IsBodyHtml = true; 
                message.Body = mailRequest.Body;           
                {
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(mailSettingsUsed, ex);
            }
        }
    }
}
