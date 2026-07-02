using Microsoft.Extensions.Configuration;
using notification_service.Application.Abstractions.Services;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace notification_service.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendMailAsync(string to, string subject, string content)
        {
            return SendMailAsync(new[] { to }, subject, content);
        }

        public async Task SendMailAsync(string[] toes, string subject, string content)
        {
            var username = _configuration["Mail:Username"];
            var password = _configuration["Mail:Password"];
            var host = _configuration["Mail:Host"];
            var port = int.Parse(_configuration["Mail:Port"]!);
            var fromName = _configuration["Mail:FromName"] ?? "Mini Commerce";

            using var mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = content,
                From = new MailAddress(username!, fromName, Encoding.UTF8)
            };

            foreach (var to in toes)
            {
                mail.To.Add(to);
            }

            using var smtp = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
