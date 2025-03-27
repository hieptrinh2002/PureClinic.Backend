using PureLifeClinic.API.Extensions;
using PureLifeClinic.Application.Interfaces.IServices;
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
            //var permissionsIdentity = await permissionService.GetUserPermissionsIdentity(userSub, cancellationToken);

            var fullPermission = await permissionService.GetUserPermissionsIdentityAsync(int.Parse(userSub), cancellationToken);
            var claims = fullPermission.Select(rc => new Claim(rc.Key, rc.Value.ToString()));


            if (fullPermission == null) //permissionsIdentity == null)
            {
                _logger.LogWarning("User {sub} does not have permissions", userSub);
                await context.WriteAccessDeniedResponse(cancellationToken: cancellationToken);
                return;
            }

            //context.User.AddIdentity(permissionsIdentity);
            var claimsIdentity = (ClaimsIdentity)context.User.Identity;//add (claims); 
            claimsIdentity.AddClaims(claims);
            context.User = new ClaimsPrincipal(claimsIdentity);

            await _next(context);
        }
    }
}
