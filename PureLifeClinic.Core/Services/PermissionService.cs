using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Security.Claims;

namespace PureLifeClinic.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async  ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(string sub, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Permissions.GetUserPermissions(sub, cancellationToken);
       
            List<Claim> claimPermissions = permissions.Select(p => new Claim(AppClaimTypes.Permission, p)).ToList();

            if (!claimPermissions.Any())
                return null;
            var permissionsIdentity = new ClaimsIdentity("PermissionsMiddleware", "name", "role");
            permissionsIdentity.AddClaims(claimPermissions);

            return permissionsIdentity;
        }

        public async Task<Dictionary<string, int>> GetUserPermissionsIdentityAsync(string sub, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            var roleClaims = await _unitOfWork.Permissions.GetRoleClaimsPermissions(sub, cancellationToken);
            var userClaims = await _unitOfWork.Permissions.GetUserClaimsPermissions(sub, cancellationToken);

            // Role Claims
            foreach (var roleClaim in roleClaims)
            {
                string resourceName =roleClaim.Type;
                int rolePermission = int.Parse(roleClaim.Value);

                if (rolePermission == PermissionConstants.Deny) continue; 

                if (!result.ContainsKey(resourceName))
                {
                    result[resourceName] = rolePermission;
                }
                else
                {
                    result[resourceName] |= rolePermission;
                }
            }

            // User Claims
            foreach (var userClaim in userClaims)
            {
                string resourceName = userClaim.Type;
                int userPermission = int.Parse(userClaim.Value);

                if (userPermission == PermissionConstants.Deny) continue; 

                if (!result.ContainsKey(resourceName))
                {
                    result[resourceName] = userPermission;
                }
                else
                {
                    result[resourceName] |= userPermission;
                }
            }

            return result;
        }

    }
}
