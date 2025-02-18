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
        ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(string sub, CancellationToken cancellationToken);
    }
}
