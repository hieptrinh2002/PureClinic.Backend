using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Middlewares;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Core.Exceptions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Middlewares
{
    public class GlobalExceptionHandlerMiddlewareTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandlerMiddleware>> _loggerMock;
        private readonly DefaultHttpContext _httpContext;
        private readonly GlobalExceptionHandlerMiddleware _middleware;

        public GlobalExceptionHandlerMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
            _httpContext = new DefaultHttpContext();
            _middleware = new GlobalExceptionHandlerMiddleware(async (ctx) => await Task.CompletedTask, _loggerMock.Object);
        }

        private async Task<string> InvokeMiddlewareWithException(Exception ex)
        {
            var middleware = new GlobalExceptionHandlerMiddleware((ctx) => throw ex, _loggerMock.Object);
            var responseBodyStream = new MemoryStream();
            _httpContext.Response.Body = responseBodyStream;

            await middleware.Invoke(_httpContext);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(_httpContext.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Fact]
        public async Task Invoke_ShouldReturn400_ForBadRequestException()
        {
            // Arrange
            var exception = new BadRequestException("Invalid input data", "BAD_REQUEST_TEST");

            // Act
            var response = await InvokeMiddlewareWithException(exception);
            var responseObject = JsonSerializer.Deserialize<ResponseViewModel>(
                response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, _httpContext.Response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Equal("Invalid input data", responseObject.Message);
            Assert.Equal("BAD_REQUEST_TEST", responseObject.Error.Code);
        }

        [Fact]
        public async Task Invoke_ShouldReturn404_ForNotFoundException()
        {
            var exception = new NotFoundException("Resource not found", "NOT_FOUND_TEST");

            var response = await InvokeMiddlewareWithException(exception);
            var responseObject = JsonSerializer.Deserialize<ResponseViewModel>(
                response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)HttpStatusCode.NotFound, _httpContext.Response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Equal("Resource not found", responseObject.Message);
            Assert.Equal("NOT_FOUND_TEST", responseObject.Error.Code);
        }

        [Fact]
        public async Task Invoke_ShouldReturn404_ForKeyNotFoundException()
        {
            var exception = new KeyNotFoundException("Key not found");

            var response = await InvokeMiddlewareWithException(exception);
            var responseObject = JsonSerializer.Deserialize<ResponseViewModel>(
                response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)HttpStatusCode.NotFound, _httpContext.Response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Equal("Key not found", responseObject.Message);
            Assert.Equal("KEY_NOT_FOUND", responseObject.Error.Code);
        }

        [Fact]
        public async Task Invoke_ShouldReturn400_ForValidationException()
        {
            var exception = new ValidationException("Validation failed");

            var response = await InvokeMiddlewareWithException(exception);
            var responseObject = JsonSerializer.Deserialize<ResponseViewModel>(
                response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)HttpStatusCode.BadRequest, _httpContext.Response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Equal("Validation failed", responseObject.Message);
            Assert.Equal("VALIDATE_ERROR", responseObject.Error.Code);
        }

        [Fact]
        public async Task Invoke_ShouldReturn500_ForGenericException()
        {
            var exception = new Exception("Unexpected error occurred");

            var response = await InvokeMiddlewareWithException(exception);
            var responseObject = JsonSerializer.Deserialize<ResponseViewModel>(
                response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Contains("An unexpected error occurred", responseObject.Message);
            Assert.Equal("INTERNAL_SERVER_ERROR", responseObject.Error.Code);
        }
    }
}
