using PureLifeClinic.Core.Entities.General;
using System.Security.Claims;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IPermissionRepository: IBaseRepository<Permission>
    {
        ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(string sub, CancellationToken cancellationToken);
    }
}
