using PureLifeClinic.Core.Entities.General;
using System.Security.Claims;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IPermissionRepository: IBaseRepository<Permission>
    {
        Task<List<Claim>> GetUserClaimsPermissions(string sub, CancellationToken cancellationToken);
        Task<List<Claim>> GetRoleClaimsPermissions(string sub, CancellationToken cancellationToken);
        Task<List<string>> GetUserPermissions(string sub, CancellationToken cancellationToken);
    }
}
