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

        public string Resource { get; set; }  // table name

        // List of permissions to be evaluated  
        public int[] Permissions { get; }

        public PermissionRequirement(int[] permissions, string resource, PermissionOperator permissionOperator)
        {
            if (permissions.Length == 0)
                throw new ArgumentException("At least one permission is required.", nameof(permissions));
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentException("Resource is required.", nameof(resource));

            Permissions = permissions;
            PermissionOperator = permissionOperator;
            Resource = resource;    
        }
    }
}
