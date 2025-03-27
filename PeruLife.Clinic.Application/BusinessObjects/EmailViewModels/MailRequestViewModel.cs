using Microsoft.AspNetCore.Http;

namespace PureLifeClinic.Application.BusinessObjects.EmailViewModels
{
    public class MailRequestViewModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
