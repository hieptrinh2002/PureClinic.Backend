using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;

namespace PureLifeClinic.Infrastructure.ExternalServices.Email
{
    public class EmailService : IMailService
    {
        private readonly AppSettings _appSettings;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IWebHostEnvironment _env;

        public EmailService(IOptions<AppSettings> appSettings, IEmailTemplateService emailTemplateService, IWebHostEnvironment env)
        {
            _appSettings = appSettings.Value;
            _emailTemplateService = emailTemplateService;
            _env = env;
        }

        public async Task SendEmailAsync(MailRequestViewModel mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_appSettings?.MailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.MailSettings.Host, _appSettings.MailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.MailSettings.Mail, _appSettings.MailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailBatchAsync(List<MailRequestViewModel> emails)
        {
            foreach (var email in emails)
            {
                await SendEmailAsync(email);
            }
        }

        public async Task SendNoShowReminder(string email, DateTime appointmentDate)
        {
            var subject = "Your appointment booking has been cancelled.";

            var body = await _emailTemplateService.RenderTemplateAsync("NoShowReminderTemplate.html",
                new Dictionary<string, string>
                {
                    { "AppointmentDate", appointmentDate.ToString("HH:mm dd/MM/yyyy") }
                });

            await SendEmailAsync(new MailRequestViewModel
            {
                ToEmail = email,
                Subject = subject,
                Body = body
            });
        }
    }
}
