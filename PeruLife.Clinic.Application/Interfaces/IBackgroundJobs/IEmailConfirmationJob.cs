using PureLifeClinic.Application.BusinessObjects.EmailViewModels;

namespace PureLifeClinic.Application.Interfaces.IBackgroundJobs
{
    public interface IEmailConfirmationJob
    {
        Task SendConfirmationEmailsAsync(string email);
        Task SendConfirmationEmailsAsync(MailRequestViewModel request);

    }
}
