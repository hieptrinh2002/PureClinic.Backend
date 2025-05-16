using FluentValidation;
using Newtonsoft.Json;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
            catch (ValidationException ex)
            {
                errorCode = "VALIDATE_ERROR";
                _logger.LogError($"ValidationException: {ex.Message}");
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, errorCode, ex.Message);
            }
            catch (ErrorException ex)
            {
                errorCode = "ERROR";
                _logger.LogError($"Error: {ex.Message}");
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, errorCode, ex.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred";
                var innerExMes = ex.InnerException == null ? string.Empty : ex.InnerException.Message;

                _logger.LogError(ex, $"Exception occurred: {ex.Message}");
                if (!string.IsNullOrEmpty(innerExMes))
                {
                    _logger.LogError(ex, $"Inner Exception occurred: {innerExMes}");
                    message += $"- {innerExMes}";
                }
                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR", message);
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
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                })
            );
        }
    }
}
