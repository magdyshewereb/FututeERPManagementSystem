using ERPManagement.Application.Configuration;
using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Models.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ERPManagement.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }
        public EmailService(IOptions<EmailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = mailSettings.Value;
            _logger = logger;
        }
        public async Task<bool> SendEmailAsync(Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;

            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            _logger.LogError("Email sending failed");

            return false;
        }
        public async Task<string> SendEmailAsync(string mailTo, string sendTo, string body, string? subject, IList<IFormFile> attachments = null)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_emailSettings.FromAddress),
                    Subject = subject
                };

                var builder = new BodyBuilder();

                if (attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in attachments)
                    {
                        if (file.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();

                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                email.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
                email.To.Add(new MailboxAddress(sendTo, mailTo));

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(_emailSettings.Host, _emailSettings.Port, true);//SecureSocketOptions.StartTls
                    smtp.Authenticate(_emailSettings.FromAddress, _emailSettings.SmtpPassword);
                    await smtp.SendAsync(email);
                }

                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

    }
}
