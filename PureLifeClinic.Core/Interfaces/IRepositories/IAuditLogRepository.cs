using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task LogAsync(AuditLog log, CancellationToken cancellationToken);
    }
}
