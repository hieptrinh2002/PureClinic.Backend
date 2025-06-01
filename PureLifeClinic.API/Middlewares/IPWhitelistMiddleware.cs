using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Common;
using System.Net;

namespace PureLifeClinic.API.Middlewares
{
    public class IPWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPWhitelistOptions _iPWhitelistOptions;
        private readonly ILogger<IPWhitelistMiddleware> _logger;
        public IPWhitelistMiddleware(RequestDelegate next, ILogger<IPWhitelistMiddleware> logger, IOptions<IPWhitelistOptions> applicationOptionsAccessor)
        {
            _iPWhitelistOptions = applicationOptionsAccessor.Value;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            List<string> whiteListIPList = _iPWhitelistOptions.WhitelistedIPs;
            var isIPWhitelisted = whiteListIPList
            .Where(ip => IPAddress.Parse(ip)
            .Equals(ipAddress))
            .Any();
            if (!isIPWhitelisted)
            {
                _logger.LogWarning("Request from Remote IP address: {RemoteIp} is forbidden.", ipAddress);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }
    }
}
