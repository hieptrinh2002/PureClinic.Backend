using PureLifeClinic.API.Extensions;
using PureLifeClinic.Application.Interfaces.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PureLifeClinic.API.Middlewares
{
    public class PermissionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionHandlerMiddleware> _logger;

        public PermissionHandlerMiddleware(RequestDelegate next, ILogger<PermissionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IPermissionService permissionService)
        {
            if (context == null) {
                _logger.LogError("Context is null");
                return;
            }

            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var cancellationToken = context.RequestAborted;
            var userSub = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? context?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(userSub))
            {
                await context.WriteAccessDeniedResponse("User 'sub' claim is required", cancellationToken: cancellationToken);
                return;
            }
            //var permissionsIdentity = await permissionService.GetUserPermissionsIdentity(userSub, cancellationToken);

            if (!int.TryParse(userSub, out int userId))
            {
                await context.WriteAccessDeniedResponse("User 'sub' claim is not a valid integer", cancellationToken: cancellationToken);
                return;
            }

            var fullPermission = await permissionService.GetUserPermissionsIdentityAsync(userId, cancellationToken);

            if (!fullPermission.Any()) //permissionsIdentity == null)
            {
                _logger.LogWarning($"User {userId} does not have permissions", userSub);
                await context.WriteAccessDeniedResponse(cancellationToken: cancellationToken);
                return;
            }
            var claims = fullPermission.Select(rc => new Claim(rc.Key, rc.Value.ToString()));

            //context.User.AddIdentity(permissionsIdentity);
            var claimsIdentity = (ClaimsIdentity)context.User.Identity;//add (claims); 
            claimsIdentity.AddClaims(claims);
            context.User = new ClaimsPrincipal(claimsIdentity);

            await _next(context);
        }
    }
}
