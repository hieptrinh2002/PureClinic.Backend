using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Controllers.V1;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Controllers.V1
{
    public class AuthControllerTests
    {
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IRefreshTokenService> _mockRefreshTokenService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUserContext> _mockUserContext;
        private readonly AuthController _controller;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<IRequestCookieCollection> _mockCookies;
        public AuthControllerTests()
        {
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockAuthService = new Mock<IAuthService>();
            _mockRefreshTokenService = new Mock<IRefreshTokenService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockUserContext = new Mock<IUserContext>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockCookies = new Mock<IRequestCookieCollection>();


            _controller = new AuthController(
                _mockLogger.Object,
                _mockAuthService.Object,
                _mockRefreshTokenService.Object,
                _mockTokenService.Object,
                _mockUserContext.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var model = new LoginViewModel { UserName = "test", Password = "password" };
            var authResult = new ResponseViewModel<UserViewModel>
            {
                Success = true,
                Data = new UserViewModel
                {
                    Id = 1,
                    FullName = "John Doe",
                    UserName = "johndoe",
                    Email = "johndoe@example.com",
                    Role = "Admin",
                    Address = "123 Main Street, New York, NY",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1990, 5, 15)
                }
            };

            var tokenResult = new ResponseViewModel<GenarateTokenViewModel>
            {
                Data = new GenarateTokenViewModel
                {
                    AccessToken = "accessToken",
                    RefreshToken = new RefreshToken
                    {
                        Token = "refreshToken",
                        CreateOn = DateTime.UtcNow,
                        ExpireOn = DateTime.UtcNow.AddMinutes(5)
                    }
                }
            };

            var refreshTokenResult = new ResponseViewModel<RefreshTokenViewModel>
            {
                Success = true,
                Data = new RefreshTokenViewModel
                {
                    Token = "refreshToken",
                    ExpireOn = DateTime.UtcNow.AddMinutes(5)
                }
            };

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _mockAuthService.Setup(s => s.Login(model.UserName, model.Password))
                .ReturnsAsync(authResult);
            _mockTokenService.Setup(s => s.GenerateJwtToken(authResult.Data.Id))
                .ReturnsAsync(tokenResult);
            _mockRefreshTokenService.Setup(
                s => s.InsertRefreshToken(authResult.Data.Id, It.IsAny<RefreshTokenCreateViewModel>(), default)
                ).ReturnsAsync(refreshTokenResult);

            // Act
            var result = await _controller.Login(model, CancellationToken.None);

            // Assert
            Assert.NotNull(authResult.Data);
            Assert.NotNull(tokenResult.Data);
            Assert.NotNull(refreshTokenResult.Data);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<AuthResultViewModel>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Login successfully", response.Message);
        }

        [Fact]
        public async Task RefreshTokenCheckAsync_ShouldReturnBadRequest_WhenCookieIsMissing()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockCookies = new Mock<IRequestCookieCollection>();

            mockCookies.Setup(c => c.TryGetValue("refreshTokenKey", out It.Ref<string>.IsAny))
                       .Returns(false); 

            mockRequest.Setup(r => r.Cookies).Returns(mockCookies.Object);
            mockHttpContext.Setup(ctx => ctx.Request).Returns(mockRequest.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            // Act
            var result = await Assert.ThrowsAsync<BadRequestException>(() => _controller.RefreshTokenCheckAsync());

            // Assert
            Assert.Equal("RefreshToken key not found", result.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginFails()
        {
            // Arrange
            var model = new LoginViewModel { UserName = "test", Password = "password" };
            var authResult = new ResponseViewModel<UserViewModel> { Success = false };

            _mockAuthService.Setup(s => s.Login(model.UserName, model.Password))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Login(model, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<UserViewModel>>(okResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Login_ShouldLogError_WhenExceptionIsThrown()
        {
            // Arrange
            var model = new LoginViewModel { UserName = "test", Password = "password" };
            var exception = new Exception("Test exception");

            _mockAuthService.Setup(s => s.Login(model.UserName, model.Password))
                .ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Login(model, CancellationToken.None));
        }


        [Fact]
        public async Task Logout_ShouldReturnOk()
        {
            // Arrange
            _mockAuthService.Setup(s => s.Logout()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Logout successfully", response.Message);
        }

        [Fact]
        public async Task RefreshTokenCheckAsync_ShouldReturnOk_WhenRefreshTokenIsValid()
        {
            // Arrange
            var tokenResult = new ResponseViewModel<GenarateTokenViewModel>
            {
                Data = new GenarateTokenViewModel
                {
                    AccessToken = "accessToken",
                    RefreshToken = new RefreshToken
                    {
                        Token = "refreshToken",
                        CreateOn = DateTime.UtcNow,
                        ExpireOn = DateTime.UtcNow.AddMinutes(5),
                        AccessTokenId = "AccessTokenId"
                    }
                }
            };

            var refreshTokenModel = new RefreshToken
            {
                Token = "newRefreshToken",
                CreateOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddMinutes(5),
                UserId = 1,

            };

            var createdTokenResult = new ResponseViewModel<RefreshTokenViewModel>
            {
                Success = true,
                Data = new RefreshTokenViewModel()
            };

            string validToken = "valid_refresh_token";
            _mockRequest.Setup(r => r.Cookies).Returns(_mockCookies.Object);
            var mockResponse = new Mock<HttpResponse>();
            _mockHttpContext.Setup(ctx => ctx.Response).Returns(mockResponse.Object);

            _mockCookies.Setup(c => c.TryGetValue("refreshTokenKey", out validToken)).Returns(true);
            _mockHttpContext.Setup(ctx => ctx.Request).Returns(_mockRequest.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = _mockHttpContext.Object };

            // Mock Cookies in Response
            var responseCookies = new Mock<IResponseCookies>();
            mockResponse.Setup(r => r.Cookies).Returns(responseCookies.Object);

            _mockRefreshTokenService.Setup(s => s.RefreshTokenCheckAsync(validToken))
                .ReturnsAsync(true);
            _mockTokenService.Setup(s => s.GenerateJwtToken(It.IsAny<int>()))
                .ReturnsAsync(tokenResult);
            _mockTokenService.Setup(s => s.GenerateRefreshToken())
                .Returns(refreshTokenModel);
            _mockRefreshTokenService.Setup(
                s => s.InsertRefreshToken(It.IsAny<int>(), It.IsAny<RefreshTokenCreateViewModel>(), default))
                .ReturnsAsync(createdTokenResult);

            // Act
            var result = await _controller.RefreshTokenCheckAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<AuthResultViewModel>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task RefreshTokenCheckAsync_ShouldReturnBadRequest_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            string validToken = "valid_refresh_token";
            _mockCookies.Setup(c => c.TryGetValue("refreshTokenKey", out validToken)).Returns(true);
            _mockRequest.Setup(r => r.Cookies).Returns(_mockCookies.Object);
            _mockHttpContext.Setup(ctx => ctx.Request).Returns(_mockRequest.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = _mockHttpContext.Object };
            _mockRefreshTokenService.Setup(s => s.RefreshTokenCheckAsync(validToken)).ReturnsAsync(false);

            // Act
            var result = await _controller.RefreshTokenCheckAsync();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invalid refresh token.", response.Message);
        }
    }
}
