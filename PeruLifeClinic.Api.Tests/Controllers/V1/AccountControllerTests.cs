using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Controllers.V1;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;
using PureLifeClinic.Core.Interfaces.IServices;
using Xunit;

namespace PureLifeClinic.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly Mock<IBackgroundJobService> _mockBackgroundService;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _mockBackgroundService = new Mock<IBackgroundJobService>();

            _controller = new AccountController(
                _mockLogger.Object,
                _mockUserService.Object,
                _mockAuthService.Object,
                _mockBackgroundService.Object
            );
        }

        #region [HttpPost("unlock-account")]
        [Fact]
        public async Task UnlockAccount_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var model = new UnlockAccountRequestViewModel { UserId = 1 };
            _mockUserService.Setup(s => s.UnlockAccountAsync(model.UserId)).ReturnsAsync(true);

            // Act
            var result = await _controller.UnlockAccount(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Account unlocked successfully.", okResult.Value);
        }

        [Fact]
        public async Task UnlockAccount_ShouldThrowBadRequestException_WhenFailed()
        {
            // Arrange
            var model = new UnlockAccountRequestViewModel { UserId = 1 };
            _mockUserService.Setup(s => s.UnlockAccountAsync(model.UserId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.UnlockAccount(model));
        }

        [Fact]
        public async Task UnlockAccount_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserId", "UserId is required");

            var model = new UnlockAccountRequestViewModel();

            // Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _controller.UnlockAccount(model));

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.UnlockAccount(model));
        }

        #endregion


        #region [HttpPost("register")]

        [Fact]
        public async Task SendActivationEmail_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var model = new UserCreateViewModel { UserName = "testuser", Email = "test@example.com" };
            var clientUrl = "http://example.com";
            var cancellationToken = new CancellationToken();
            var response = new ResponseViewModel { Success = true };

            _mockUserService.Setup(s => s.IsExists("UserName", model.UserName, cancellationToken)).ReturnsAsync(false);
            _mockUserService.Setup(s => s.IsExists("Email", model.Email, cancellationToken)).ReturnsAsync(false);
            _mockUserService.Setup(s => s.Create(model, cancellationToken)).ReturnsAsync(response);
            _mockUserService.Setup(s => s.GenerateEmailConfirmationTokenAsync(model.Email))
                .ReturnsAsync(
                new ResponseViewModel<EmailActivationViewModel>
                {
                    Success = true,
                    Message = "Email confirmation token generated successfully",
                    Data = new EmailActivationViewModel
                    {
                        UserId = 1,
                        ActivationToken = "token",
                        ActivationUrl = "http://example.com"
                    }
                }
            );

            // Act
            var result = await _controller.SendActivationEmail(model, clientUrl, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task SendActivationEmail_ShouldReturnBadRequest_WhenUserNameExists()
        {
            // Arrange
            var model = new UserCreateViewModel { UserName = "testuser", Email = "test@example.com" };
            var clientUrl = "http://example.com";
            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(s => s.IsExists("UserName", model.UserName, cancellationToken))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _controller.SendActivationEmail(model, clientUrl, cancellationToken));
        }

        [Fact]
        public async Task SendActivationEmail_ShouldReturnBadRequest_WhenEmailExists()
        {
            // Arrange
            var model = new UserCreateViewModel { UserName = "testuser", Email = "test@example.com" };
            var clientUrl = "http://example.com";
            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(s => s.IsExists("UserName", model.UserName, cancellationToken))
                .ReturnsAsync(false);
            _mockUserService.Setup(s => s.IsExists("Email", model.Email, cancellationToken))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _controller.SendActivationEmail(model, clientUrl, cancellationToken));
        }
        #endregion


        #region [HttpGet("activate-email")]

        [Fact]
        public async Task EmailConfirmation_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var emailConfirmation = "test@example.com";
            var activeToken = "token";
            var cancellationToken = new CancellationToken();
            var response = new ResponseViewModel { Success = true };

            _mockUserService.Setup(s => s.IsExists("Email", emailConfirmation, cancellationToken))
                .ReturnsAsync(true);
            _mockAuthService.Setup(s => s.ConfirmEmailAsync(emailConfirmation, activeToken, cancellationToken))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.EmailConfirmation(emailConfirmation, activeToken, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task EmailConfirmation_ShouldThrowBadRequestException_WhenEmailNotFound()
        {
            // Arrange
            var emailConfirmation = "test@example.com";
            var activeToken = "token";
            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(s => s.IsExists("Email", emailConfirmation, cancellationToken))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _controller.EmailConfirmation(emailConfirmation, activeToken, cancellationToken));
        }

        [Fact]
        public async Task EmailConfirmation_ShouldThrowBadRequestException_WhenFailedToConfirmEmail()
        {
            // Arrange
            var emailConfirmation = "test@example.com";
            var activeToken = "token";
            var cancellationToken = new CancellationToken();

            _mockUserService.Setup(s => s.IsExists("Email", emailConfirmation, cancellationToken))
                .ReturnsAsync(true);
            _mockAuthService.Setup(s => s.ConfirmEmailAsync(emailConfirmation, activeToken, cancellationToken))
                .ReturnsAsync(new ResponseViewModel { Success = false }
            );

            await Assert.ThrowsAsync<BadRequestException>(
                () => _controller.EmailConfirmation(emailConfirmation, activeToken, cancellationToken));
        }

        #endregion


        #region [HttpPost("reset-password")]    

        [Fact]
        public async Task ResetPassword_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var model = new ResetPasswordRequest
            {
                Email = "test@example.com",
                Token = "&*0f39030fdsss",
                NewPassword = "ROjfdo11..##"
            };
            var response = new ResponseViewModel { Success = true };

            _mockUserService.Setup(s => s.ResetPasswordAsync(model.Email, model.Token, model.NewPassword))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.ResetPassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ResponseViewModel>(okResult.Value);
            Assert.True(responseModel.Success);
            Assert.Equal("Password reset successfully.", responseModel.Message);
        }

        [Fact]
        public async Task ResetPassword_ShouldThrowBadRequestException_WhenFailed()
        {
            // Arrange
            var model = new ResetPasswordRequest
            {
                Email = "test@example.com",
                Token = "&*0f39030fdsss",
                NewPassword = "ROjfdo11..##"
            };
            var response = new ResponseViewModel { Success = false };

            _mockUserService.Setup(s => s.ResetPasswordAsync(model.Email, model.Token, model.NewPassword))
                .ReturnsAsync(response);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.ResetPassword(model));
        }
        #endregion


        #region [HttpPost("forgot-password")]

        [Fact]
        public async Task ChangePassword_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var model = new ForgotPasswordRequestViewModel
            {
                Email = "test@example.com",
                ClientUrl = "https://chatgpt.com/"
            };

            var user = new User
            {
                Email = model.Email,
                RoleId = 1,
                IsActive = true,
                FullName = "Test User"
            };
            var response = new ResponseViewModel<ResetPasswordViewModel>
            {
                Success = true,
                Data = new ResetPasswordViewModel
                {
                    EmailBody = "email body",
                    Token = "token",
                    Url = "https://chatgpt.com/"
                }
            };

            _mockUserService.Setup(s => s.GetByEmail(model.Email, default)).ReturnsAsync(user);
            _mockUserService.Setup(s => s.GenerateResetPasswordTokenAsync(model)).ReturnsAsync(response);

            // Act
            var result = await _controller.ChangePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ResponseViewModel>(okResult.Value); 
            Assert.True(responseModel.Success);
            Assert.Equal("Reset password mail was sent successfully.", responseModel.Message);
        }

        [Fact]
        public async Task ChangePassword_ShouldThrowNotFoundException_WhenEmailNotFound()
        {
            // Arrange
            var model = new ForgotPasswordRequestViewModel { Email = "test@example.com" };

            _mockUserService.Setup(s => s.GetByEmail(model.Email, default)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _controller.ChangePassword(model));
        }

        [Fact]
        public async Task ChangePassword_ShouldThrowErrorException_WhenFailedToGenerateToken()
        {
            // Arrange
            var model = new ForgotPasswordRequestViewModel
            {
                Email = "test@example.com",
                ClientUrl = "https://helo.vn"
            };
            var user = new User
            {
                Email = model.Email,
                RoleId = 1,
                IsActive = true,
                FullName = "Test User"
            };

            var response = new ResponseViewModel<ResetPasswordViewModel> { Success = false };

            _mockUserService.Setup(s => s.GetByEmail(model.Email, default)).ReturnsAsync(user);
            _mockUserService.Setup(s => s.GenerateResetPasswordTokenAsync(model)).ReturnsAsync(response);

            // Act & Assert
            await Assert.ThrowsAsync<ErrorException>(() => _controller.ChangePassword(model));
        }

        [Fact]
        public async Task ChangePassword_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Email is required");

            var model = new ForgotPasswordRequestViewModel();

            // Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _controller.ChangePassword(model));

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _controller.ChangePassword(model));
        }
        public async Task ChangePasswor_ResetTokenDataIsNull_ReturnsBadRequest()
        {
            // Arrange
            var model = new ForgotPasswordRequestViewModel
            {
                Email = "test@example.com",
                ClientUrl = "https://helo.vn"
            };
            var user = new User
            {
                Email = model.Email,
                RoleId = 1,
                IsActive = true,
                FullName = "Test User"
            };

            var response = new ResponseViewModel<ResetPasswordViewModel> { Success = true, Data = null};

            _mockUserService.Setup(s => s.GetByEmail(model.Email, default)).ReturnsAsync(user);
            _mockUserService.Setup(s => s.GenerateResetPasswordTokenAsync(model)).ReturnsAsync(response);

            // Act & Assert
            Exception ex =  await Assert.ThrowsAsync<ErrorException>(() => _controller.ChangePassword(model));
            Assert.Equal("Failed to generate reset password token", ex.Message);
        }
        #endregion
    }
}