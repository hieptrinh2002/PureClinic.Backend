using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Middlewares;
using PureLifeClinic.Application.Interfaces.IServices;
using System.Security.Claims;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Middlewares
{
    public class PermissionHandlerMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<PermissionHandlerMiddleware>> _mockLogger;
        private readonly Mock<IPermissionService> _mockPermissionService;
        private readonly PermissionHandlerMiddleware _middleware;

        public PermissionHandlerMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<PermissionHandlerMiddleware>>();
            _mockPermissionService = new Mock<IPermissionService>();
            _middleware = new PermissionHandlerMiddleware(_mockNext.Object, _mockLogger.Object);
        }

        private HttpContext CreateHttpContext(List<Claim>? claims = null, bool isAuthenticated = true)
        {
            var context = new DefaultHttpContext();
            var identity = new ClaimsIdentity(claims ?? new List<Claim>(), isAuthenticated ? "testAuth" : null);
            context.User = new ClaimsPrincipal(identity);
            return context;
        }

        [Fact]
        public async Task Invoke_UserNotAuthenticated_ShouldCallNext()
        {
            // Arrange
            var context = CreateHttpContext(isAuthenticated: false);

            // Act
            await _middleware.Invoke(context, _mockPermissionService.Object);

            // Assert
            _mockNext.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_UserMissingSubClaim_ShouldReturnAccessDenied()
        {
            // Arrange
            var context = CreateHttpContext();
            context.Response.Body = new MemoryStream(); // Capture response

            // Act
            await _middleware.Invoke(context, _mockPermissionService.Object);

            // Assert
            Assert.Equal(403, context.Response.StatusCode); // Forbidden
        }

        [Fact]
        public async Task Invoke_SubClaimNotInteger_ShouldReturnAccessDenied()
        {
            // Arrange
            var context = CreateHttpContext(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "invalid123") });
            context.Response.Body = new MemoryStream();

            // Act
            await _middleware.Invoke(context, _mockPermissionService.Object);

            // Assert
            Assert.Equal(403, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_UserWithoutPermissions_ShouldReturnAccessDenied()
        {
            // Arrange
            var context = CreateHttpContext(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") });
            context.Response.Body = new MemoryStream();
            var emptyPermissions = new Dictionary<string, int> { };

            _mockPermissionService.Setup(service => service.GetUserPermissionsIdentityAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyPermissions);

            // Act
            await _middleware.Invoke(context, _mockPermissionService.Object);

            // Assert
            Assert.Equal(403, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_ValidUser_ShouldAddClaimsAndCallNext()
        {
            // Arrange
            var userClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
            var context = CreateHttpContext(userClaims);
            context.Response.Body = new MemoryStream();

            var permissions = new Dictionary<string, int>
            {
                { "Medicine", 2 },
                { "Customer", 3 }
            };

            _mockPermissionService
                .Setup(service => service.GetUserPermissionsIdentityAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissions);

            // Act
            await _middleware.Invoke(context, _mockPermissionService.Object);

            // Assert
            Assert.Contains(context.User.Claims, c => c.Type == "Medicine" && c.Value == "2");
            Assert.Contains(context.User.Claims, c => c.Type == "Customer" && c.Value == "3");
            _mockNext.Verify(next => next(context), Times.Once);
        }
    }
}
