using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestViewModel mailRequest);
        Task SendEmailBatchAsync(List<MailRequestViewModel> emailList);
    }
}
