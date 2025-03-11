using Microsoft.AspNetCore.Authorization;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Helpers.Authz.PolicyProvider
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        public static string ClaimType => AppClaimTypes.Permission;

        // Operator to be used for multiple permissions 
        public PermissionOperator PermissionOperator { get; set; }

        // List of permissions to be evaluated  
        public string[] Permissions { get; }

        public PermissionRequirement(string[] permissions, PermissionOperator permissionOperator)
        {
            if (permissions.Length == 0)
                throw new ArgumentException("At least one permission is required.", nameof(permissions));
            Permissions = permissions;
            PermissionOperator = permissionOperator;
        }
    }
}
