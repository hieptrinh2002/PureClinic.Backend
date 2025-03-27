using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Jobs
{
    public class EmailConfirmationJob : IEmailConfirmationJob
    {
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly UserManager<User> _userManager;

        public EmailConfirmationJob(IBackgroundJobService backgroundJobService, UserManager<User> userManager)
        {
            _backgroundJobService = backgroundJobService;
            _userManager = userManager;
        }

        public async Task SendConfirmationEmailsAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail) ?? throw new NotFoundException("User not found");

            var model = new MailRequestViewModel
            {
                ToEmail = userEmail,
                Subject = "Confirm booking appointment",
                Body = "Please confirm your booking appointment."
            };

            _backgroundJobService.ScheduleImmediateJob<IMailService>(mailService => mailService.SendEmailAsync(model));
        }

        public async Task SendConfirmationEmailsAsync(MailRequestViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.ToEmail) ?? throw new NotFoundException("User not found");
            _backgroundJobService.ScheduleImmediateJob<IMailService>(mailService => mailService.SendEmailAsync(request));
        }
    }
}
