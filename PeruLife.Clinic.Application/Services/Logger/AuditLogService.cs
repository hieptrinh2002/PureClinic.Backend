using Microsoft.AspNetCore.Http;
using PureLifeClinic.Application.Interfaces.IServices.ILogger;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.Logger
{
    public class AuditLogService : IAuditLogger
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;   
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task LogAsync(string action, string entityName, string entityId, string performedBy, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                PerformedBy = performedBy,
                Timestamp = DateTime.UtcNow,
                Details = details,
                IPAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
            };

            await _unitOfWork.AuditLog.Create(log, default);
            await _unitOfWork.SaveChangesAsync();   
        }
    }
}
