using RAMMS.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Business.ServiceProvider.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestDto mailRequest);
    }
}
