using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IBackgroundJobs
{
    public interface IEmailConfirmationJob
    {
        Task SendConfirmationEmailsAsync(string email);
        Task SendConfirmationEmailsAsync(MailRequestViewModel request);

    }
}
