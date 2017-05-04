using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace KostenBatenTool.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string vanEmail, string naarEmail, MimeMessage message);
    }
}
