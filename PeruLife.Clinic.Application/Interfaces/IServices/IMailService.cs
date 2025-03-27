using PureLifeClinic.Application.BusinessObjects.EmailViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestViewModel mailRequest);
        Task SendEmailBatchAsync(List<MailRequestViewModel> emailList);
    }
}
