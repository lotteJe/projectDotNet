using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(String vanEmail, string email, string subject, string message);
    }
}
