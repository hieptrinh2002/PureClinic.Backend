namespace PureLifeClinic.Application.Interfaces.IServices.ILogger
{
    public interface IAuditLogger
    {
        Task LogAsync(string action, string entityName, string entityId, string performedBy, string details);
    }
}
