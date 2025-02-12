using Newtonsoft.Json;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;   
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            string errorCode = string.Empty;
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                errorCode = ex.ErrorCode ?? "BAD_REQUEST";
                _logger.LogError($"BadRequestException: {ex.Message}");
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, errorCode, ex.Message);
            }
            catch (NotFoundException ex)
            {
                errorCode = ex.ErrorCode ?? "NOT_FOUND";
                _logger.LogError($"NotFoundException: {ex.Message}");
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, errorCode, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                errorCode = "KEY_NOT_FOUND";
                _logger.LogError($"KeyNotFoundException: {ex.Message}");
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, errorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message} - " + ex.Message);
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR", "An unexpected error occurred.");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, int statusCode, string errorCode, string message)
        {
            var response = new ResponseViewModel
            {
                Success = false,
                Message = message,
                Error = new ErrorViewModel
                {
                    Code = errorCode,
                }
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
