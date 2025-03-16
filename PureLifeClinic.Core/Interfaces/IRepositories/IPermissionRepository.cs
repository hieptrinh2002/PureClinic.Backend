using PureLifeClinic.Core.Entities.General;
using System.Security.Claims;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IPermissionRepository: IBaseRepository<Permission>
    {
        Task<List<Claim>> GetUserClaimsPermissions(int sub, CancellationToken cancellationToken);
        Task<List<Claim>> GetRoleClaimsPermissions(int roleId, CancellationToken cancellationToken);
        Task<List<string>> GetUserPermissions(int sub, CancellationToken cancellationToken);
    }
}
