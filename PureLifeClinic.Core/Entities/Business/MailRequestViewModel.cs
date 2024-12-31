using Microsoft.AspNetCore.Http;

namespace PureLifeClinic.Core.Entities.Business
{
    public class MailRequestViewModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
