using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace PureLifeClinic.API.ActionFilters
{
    public class ApiLoggingFilter : IAsyncActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var requestBody = await ReadRequestBodyAsync(request);
            _logger.LogInformation($"[API Request] {request.Method} {request.Path}\nHeaders: {string.Join(", ", request.Headers)}\nBody: {requestBody}");
            var t = $"[API Request] {request.Method} {request.Path}\nHeaders: {string.Join(", ", request.Headers)}\nBody: {requestBody}";
            var executedContext = await next();

            var response = executedContext.HttpContext.Response;
            _logger.LogInformation($"[API Response] {request.Method} {request.Path} => Status Code: {response.StatusCode}");
            t = $"[API Response] {request.Method} {request.Path} => Status Code: {response.StatusCode}";
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering(); 
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0; 
            return body;
        }
    }
}
