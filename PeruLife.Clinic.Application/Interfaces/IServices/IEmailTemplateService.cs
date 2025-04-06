namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IEmailTemplateService
    {
        Task<string> RenderTemplateAsync(string templateName, Dictionary<string, string> values);
    }
}
