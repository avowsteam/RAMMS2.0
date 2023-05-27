using Microsoft.Extensions.Options;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Common;
using RAMMS.DTO;
using System;
using System.Collections.Generic;
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
            var email = new MailMessage();
            email.From = new MailAddress(_mailSettings.Mail);
            email.To.Add(mailRequest.ToEmail);
            email.Subject = mailRequest.Subject;

            var mailSettingsUsed = string.Empty;

            try
            {
                //using (MailMessage emailMessage = new MailMessage())
                //{
                //    emailMessage.From = new MailAddress(_mailSettings.Mail);
                //    emailMessage.To.Add(new MailAddress(mailRequest.ToEmail));
                //    emailMessage.Subject = mailRequest.Subject;
                //    emailMessage.Body = mailRequest.Body;
                //    emailMessage.Priority = MailPriority.Normal;
                //    emailMessage.IsBodyHtml = true;
                //    using (SmtpClient MailClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port))
                //    {
                //        MailClient.EnableSsl = true;
                //        MailClient.UseDefaultCredentials = false;
                //        MailClient.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                //        MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        await MailClient.SendMailAsync(emailMessage);
                //    }

                //    //smtp.UseDefaultCredentials = false;        
                //    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;            
                //}

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailSettings.Mail);
                message.To.Add(new MailAddress(mailRequest.ToEmail));
                message.Subject = mailRequest.Subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = mailRequest.Body;
                smtp.Port = _mailSettings.Port;
                smtp.Host = _mailSettings.Host; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception(mailSettingsUsed, ex);
            }
        }
    }
}
