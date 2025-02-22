using Microsoft.AspNetCore.Authorization;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Helpers.Authz.PolicyProvider
{

    //// multiple permissions
    //[PermissionAuthorize(PermissionOperator.Or, Permissions.Create, Permissions.Update)]

    //// single permission
    //[PermissionAuthorize("Create")]
    public class PermissionAuthorizeAttribute: AuthorizeAttribute
    {
        internal const string POLICY_PREFIX = "PERMISSION_";
        private const string Separator = "_";

        public PermissionAuthorizeAttribute(
           PermissionOperator permissionOperator, params string[] permissions)
        {
            // E.g: PERMISSION_1_Create_Update..
            Policy = $"{POLICY_PREFIX}{(int)permissionOperator}{Separator}{string.Join(Separator, permissions)}";
        }

        public PermissionAuthorizeAttribute(string permission)
        {
            // E.g: PERMISSION_1_Create..
            Policy = $"{POLICY_PREFIX}{(int)PermissionOperator.And}{Separator}{permission}";
        }

        public static PermissionOperator GetOperatorFromPolicy(string policyName)
        {
            var @operator = int.Parse(policyName.AsSpan(POLICY_PREFIX.Length, 1));
            return (PermissionOperator)@operator;
        }

        public static string[] GetPermissionsFromPolicy(string policyName)
        {
            return policyName.Substring(POLICY_PREFIX.Length + 2)
                .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
