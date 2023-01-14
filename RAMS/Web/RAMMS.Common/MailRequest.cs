//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RAMMS.Common
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> CC { get; set; }
        //public List<IFormFile> Attachments { get; set; }
    }

    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailNotification
    {
        public void SendMail(string subject, string body, string toMail)
        {
            MailNotification mailNotification = new MailNotification();
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = toMail,
                //ToEmail = "rahah@edgenta.com",
                Body = body,
                //CC = new List<string>
                //    {
                //        "nagulmeera.s@avowstech.com",
                //        "bahadoor.s@avowstech.com",
                //        "zarif.m@avowstech.com"
                //    },
                //CC = new List<string>
                //    {
                //        "nagulmeera.s@avowstech.com",
                //        "anand.k@avowstech.com",
                //        "jayanth.k@avowstech.com",
                //        "bahadoor.s@avowstech.com",
                //        "zarif.m@avowstech.com"
                //    },
                Subject = subject
            };
            MailSettings mailSettings = new MailSettings
            {
                DisplayName = "KLCCWorms",
                Host = "smtpdm-ap-southeast-1.aliyun.com",
                Mail = "eworms@klccuhibcc.com",
                Password = "gUZ5r2Oe4p",
                Port = 80
            };
            mailNotification.SendEmail(mailRequest, mailSettings);
        }

        public void SendEmail(MailRequest mailRequest,MailSettings mailSettings)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(mailSettings.Mail);
                message.To.Add(new MailAddress(mailRequest.ToEmail));
                message.Subject = mailRequest.Subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = mailRequest.Body;
                smtp.Port = mailSettings.Port;
                smtp.Host = mailSettings.Host; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailSettings.Mail, mailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }

        //public void SendEmail(MailRequest mailRequest, MailSettings _mailSettings)
        //{
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
        //    email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        //    if (mailRequest.CC != null)
        //        foreach (string ccmail in mailRequest.CC)
        //            email.Cc.Add(MailboxAddress.Parse(ccmail));
        //    email.Subject = mailRequest.Subject;
        //    var builder = new BodyBuilder();

        //    try
        //    {
        //        builder.HtmlBody = mailRequest.Body;
        //        email.Body = builder.ToMessageBody();
        //        using var smtp = new SmtpClient();
        //        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //        smtp.Send(email);
        //        smtp.Disconnect(true);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
