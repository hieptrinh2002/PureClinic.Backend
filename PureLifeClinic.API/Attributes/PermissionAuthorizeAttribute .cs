using Microsoft.AspNetCore.Authorization;
using PureLifeClinic.Core.Enums;
using System.Text.RegularExpressions;

namespace PureLifeClinic.API.Attributes
{
    //// multiple permissions
    //[PermissionAuthorize(PermissionConstants.Product, PermissionOperator.And, PermissionAction.View, PermissionAction.CreateDelete)]

    //// single permission
    //[PermissionConstants.Product, PermissionAction.View]

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string POLICY_PREFIX = "PERMISSION_";
        private const string Separator = "_";
        public PermissionAuthorizeAttribute(string resource, PermissionOperator permissionOperator, params PermissionAction[] permissions)
        {
            // E.g: PERMISSION_Customer_1_Create_Update.. ~ => PERMISSION_Customer_1_1_2..
            Policy = $"{POLICY_PREFIX}{resource}{Separator}{(int)permissionOperator}{Separator}{string.Join(
                Separator,
                permissions.Select(p => (int)p).ToArray())}";
        }

        public PermissionAuthorizeAttribute(string resource, PermissionAction permission)
        {
            Policy = $"{POLICY_PREFIX}{resource}{Separator}{(int)PermissionOperator.And}{Separator}{(int)permission}";
        }

        public static PermissionOperator GetOperatorFromPolicy(string policyName)
        {
            Match match = Regex.Match(policyName, @"_(\d+)_");
            if (match.Success)
                return (PermissionOperator)int.Parse(match.Groups[1].Value);

            return PermissionOperator.And;
        }

        public static string GetResourceFromPolicy(string policyName)
        {
            // E.g: PERMISSION_Customer_1_1_2.. => "Customer"
            string resource = policyName.Substring(POLICY_PREFIX.Length)
                .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
            return resource;
        }

        public static int[] GetPermissionsFromPolicy(string policyName)
        {
            // remove prefix
            string[] parts = policyName.Substring(POLICY_PREFIX.Length).Split(Separator, StringSplitOptions.RemoveEmptyEntries);

            // remove resource name and PermissionOperator (2 first elements)
            return parts.Skip(2).Select(int.Parse).ToArray();
        }
    }
}
