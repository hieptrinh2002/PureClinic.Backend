using Microsoft.AspNetCore.Hosting;
using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.Infrastructure.ExternalServices.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IWebHostEnvironment _env;

        public EmailTemplateService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> RenderTemplateAsync(string templateName, Dictionary<string, string> values)
        {
            var path = Path.Combine(_env.ContentRootPath, "ExternalServices", "Email", "Templates", templateName)
                .Replace("PureLifeClinic.API", "PureLifeClinic.Infrastructure");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Email template not found: {templateName}");

            var template = await File.ReadAllTextAsync(path);

            foreach (var kv in values)
            {
                template = template.Replace("{{" + kv.Key + "}}", kv.Value);
            }

            return template;
        }
    }
}
