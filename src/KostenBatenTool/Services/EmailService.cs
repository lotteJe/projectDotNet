using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace KostenBatenTool.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _ec;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            this._ec = emailConfig.Value;
        }

        public async Task SendEmailAsync(string vanEmail, string naarEmail, MimeMessage message)
        {
            try
            {
                MimeMessage emailMessage = message;
                emailMessage.From.Add(new MailboxAddress(vanEmail, _ec.FromAddress));
                emailMessage.To.Add(new MailboxAddress("", naarEmail));
                using (var client = new SmtpClient())
                {
                    client.LocalDomain = _ec.LocalDomain;

                    await client.ConnectAsync(_ec.MailServerAddress, Convert.ToInt32(_ec.MailServerPort),
                        SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(_ec.UserId, _ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
