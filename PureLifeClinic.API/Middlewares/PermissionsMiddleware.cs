using PureLifeClinic.API.Extensions;
using PureLifeClinic.Core.Interfaces.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PureLifeClinic.API.Middlewares
{
    public class PermissionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionsMiddleware> _logger;

        public PermissionsMiddleware(RequestDelegate next, ILogger<PermissionsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IPermissionService permissionService)
        {
            if(context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var cancellationToken = context.RequestAborted;
            var userSub = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;


            if (string.IsNullOrEmpty(userSub))
            {
                await context.WriteAccessDeniedResponse("User 'sub' claim is required", cancellationToken: cancellationToken);
                return;
            }
            var permissionsIdentity = await permissionService.GetUserPermissionsIdentity(userSub, cancellationToken);

            if (permissionsIdentity == null)
            {
                _logger.LogWarning("User {sub} does not have permissions", userSub);

                //await context.WriteAccessDeniedResponse(cancellationToken: cancellationToken);
                //return;
            }
            else
            {
                context.User.AddIdentity(permissionsIdentity);
            }

            // User has permissions, so we add the extra identity containing the "permissions" claims
            await _next(context);
        }
    }
}
