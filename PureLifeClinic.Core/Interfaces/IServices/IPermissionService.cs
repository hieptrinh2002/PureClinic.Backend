using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Enums;
using System.Security.Claims;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IPermissionService
    {

        /// <summary>
        /// Returns a new identity containing the user permissions as Claims
        /// </summary>
        /// <param name="sub">The user external id (sub claim)</param>
        /// <param name="cancellationToken"></param>
        ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(int sub, CancellationToken cancellationToken);
        Task<Dictionary<string, int>> GetUserPermissionsIdentityAsync(int sub, CancellationToken cancellationToken);
        Task<Dictionary<string, int>> GetRolePermissions(int roleId, CancellationToken cancellationToken);
        Task LockUserPermissions(int userId);
        Task UpdateRolePermission(int roleId, ResourcePermissionViewModel model, CancellationToken cancellationToken);
        Task UpdateRolePermissions(int roleId, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken);
        Task UnLockUserPermissions(int userId);
        Task UpdateUserPermissions(int userId, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken);
        Task AddNewPermissions(int id, PermissionType type, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken);
    }
}
