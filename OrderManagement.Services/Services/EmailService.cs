using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigurationDto _emailConfig;

        public EmailService(EmailConfigurationDto emailConfig)
        {
            _emailConfig = emailConfig;
        }

        #region SendEmailAsync
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailConfig.UserName);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                }
            }
        } 
        #endregion
    }
}
