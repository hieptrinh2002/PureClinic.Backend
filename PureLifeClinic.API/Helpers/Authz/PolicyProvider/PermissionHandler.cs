using Microsoft.AspNetCore.Authorization;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Helpers.Authz.PolicyProvider
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string resource = requirement.Resource;
            //1. Get all claims related to the resource   
            var permissionClaims = context.User.FindAll(resource)
                .Select(c => int.TryParse(c.Value, out var value) ? value : 0)
                .ToList();

            if (!permissionClaims.Any())
            {
                context.Fail();
                return Task.CompletedTask;
            }

            //2. Check if the user has the required permissions
            bool hasPermission = requirement.PermissionOperator switch
            {
                PermissionOperator.And => requirement.Permissions.All(reqValue => permissionClaims.Any(p => (p & reqValue) == reqValue)),

                PermissionOperator.Or => requirement.Permissions.Any(reqValue => permissionClaims.Any(p => (p & reqValue) != 0)),

                _ => false
            };

            if (hasPermission)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }

        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        //{
        //    string resource = requirement.Resource;
        //    if (requirement.PermissionOperator == PermissionOperator.And)
        //    {
        //        foreach (var permission in requirement.Permissions)
        //        {
        //            //if (!context.User.HasClaim(PermissionRequirement.ClaimType, permission))
        //            if (!context.User.HasClaim(c => c.Type == AppClaimTypes.Permission && c.Value == resource))
        //            {
        //                // If the user lacks ANY of the required permissions => mark it as failed.
        //                context.Fail();
        //                return Task.CompletedTask;
        //            }
        //        }
        //        context.Succeed(requirement);
        //        return Task.CompletedTask;
        //    }
        //    if (requirement.PermissionOperator == PermissionOperator.Or)
        //    {
        //        foreach (var permission in requirement.Permissions)
        //        {
        //            if (context.User.HasClaim(c => c.Type == AppClaimTypes.Permission && c.Value == resource))
        //            {
        //                // If the user has ANY of the required permissions => mark it as success.
        //                context.Succeed(requirement);
        //                return Task.CompletedTask;
        //            }
        //        }
        //    }
        //    // identity does not have any of the required permissions
        //    context.Fail();
        //    return Task.CompletedTask;
        //}
    }
}
