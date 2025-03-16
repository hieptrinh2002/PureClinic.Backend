using Hangfire;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.BackgroundServices.Jobs
{
    public class EmailConfirmationJob : IEmailConfirmationJob
    {
        private readonly IMailService _emailService;

        public EmailConfirmationJob(IMailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendConfirmationEmailsAsync(string userEmail)
        {
            MailRequestViewModel model = new MailRequestViewModel
            {
                ToEmail = userEmail,
                Subject = "Confirm booking appointment",
                Body = "Please confirm your booking appointment."
            };

            BackgroundJob.Enqueue<IMailService>(mailService => mailService.SendEmailAsync(model));

            return Task.CompletedTask; 
        }

        public Task SendConfirmationEmailsAsync(MailRequestViewModel request)
        {
            BackgroundJob.Enqueue<IMailService>(mailService => mailService.SendEmailAsync(request));
            return Task.CompletedTask;

        }
    }
}
