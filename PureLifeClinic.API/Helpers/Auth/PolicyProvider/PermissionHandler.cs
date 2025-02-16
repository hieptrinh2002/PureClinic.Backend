using Microsoft.AspNetCore.Authorization;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Helpers.Auth.PolicyProvider
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            if (requirement.PermissionOperator == PermissionOperator.And)
            if (requirement.PermissionOperator == PermissionOperator.And)
            {
                foreach (var permission in requirement.Permissions)
                {
                    //if (!context.User.HasClaim(PermissionRequirement.ClaimType, permission))
                    if (!context.User.HasClaim(c => c.Type == AppClaimTypes.Permission && c.Value == permission))
                    {
                        // If the user lacks ANY of the required permissions => mark it as failed.
                        context.Fail();
                        return Task.CompletedTask;
                    }
                }
            }
            if (requirement.PermissionOperator == PermissionOperator.Or)
            {
                foreach (var permission in requirement.Permissions)
                {
                    if (context.User.HasClaim(c => c.Type == AppClaimTypes.Permission && c.Value == permission))
                    {
                        // If the user has ANY of the required permissions => mark it as success.
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }
            // identity does not have any of the required permissions
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
