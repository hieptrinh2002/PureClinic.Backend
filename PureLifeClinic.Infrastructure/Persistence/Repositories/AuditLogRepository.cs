using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task LogAsync(AuditLog log, CancellationToken cancellationToken)
        {
            await _dbContext.AuditLogs.AddAsync(log);
        }
    }
}
