using Microsoft.AspNetCore.Identity;
using Moq;
using PeruLife.Clinic.Application.Services;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using Xunit;
using Assert = Xunit.Assert;


namespace PureLifeClinic.Application.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<SignInManager<User>> _signInManager;
        private readonly Mock<IRoleRepository> _roleRepo = new();
        private readonly Mock<IAuthRepository> _authRepo = new();

        public AuthServiceTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManager = new Mock<SignInManager<User>>(
                _userManager.Object,
                contextAccessor.Object,
                userClaimsPrincipalFactory.Object,
            null, null, null, null);

            _unitOfWork.Setup(u => u.Roles).Returns(_roleRepo.Object);
            _unitOfWork.Setup(u => u.Auth).Returns(_authRepo.Object);
        }

        [Fact]
        public async Task LoginProcess_UserNotFound_ThrowsNotFoundException()
        {
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync((User)null);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => service.LoginProcess("user", "pass"));
        }

        [Fact]
        public async Task LoginProcess_UserNotActive_ThrowsNotFoundException()
        {
            var user = new User { IsActive = false };
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync(user);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => service.LoginProcess("user", "pass"));
        }

        [Fact]
        public async Task LoginProcess_EmailNotConfirmed_ThrowsBadRequestException()
        {
            var user = new User { IsActive = true };
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync(user);
            _userManager.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(false);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            await Assert.ThrowsAsync<BadRequestException>(() => service.LoginProcess("user", "pass"));
        }

        [Fact]
        public async Task LoginProcess_UserLockedOut_ThrowsBadRequestException()
        {
            var user = new User { IsActive = true };
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync(user);
            _userManager.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _userManager.Setup(m => m.IsLockedOutAsync(user)).ReturnsAsync(true);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            await Assert.ThrowsAsync<BadRequestException>(() => service.LoginProcess("user", "pass"));
        }

        [Fact]
        public async Task LoginProcess_PasswordInvalid_ThrowsBadRequestException()
        {
            var user = new User { IsActive = true };
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync(user);
            _userManager.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _userManager.Setup(m => m.IsLockedOutAsync(user)).ReturnsAsync(false);
            _signInManager.Setup(s => s.PasswordSignInAsync(user, "pass", false, true))
                .ReturnsAsync(SignInResult.Failed);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            await Assert.ThrowsAsync<BadRequestException>(() => service.LoginProcess("user", "pass"));
        }

        [Fact]
        public async Task LoginProcess_Success_ReturnsUserViewModel()
        {
            var user = new User { Id = 1, UserName = "user", Email = "email", IsActive = true, RoleId = 2 };
            var role = new Role { Id = 2, Name = "Admin" };
            _userManager.Setup(m => m.FindByNameAsync("user")).ReturnsAsync(user);
            _userManager.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _userManager.Setup(m => m.IsLockedOutAsync(user)).ReturnsAsync(false);
            _signInManager.Setup(s => s.PasswordSignInAsync(user, "pass", false, true))
                .ReturnsAsync(SignInResult.Success);
            _roleRepo.Setup(r => r.GetById(user.RoleId, default)).ReturnsAsync(role);

            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            var result = await service.LoginProcess("user", "pass");

            Assert.True(result.Success);
            Assert.Equal("user", result.Data.UserName);
            Assert.Equal("Admin", result.Data.Role);
        }

        [Fact]
        public async Task Login_Success_ReturnsResponseViewModel()
        {
            var userViewModel = new UserViewModel { Id = 1, UserName = "user" };
            var response = new ResponseViewModel<UserViewModel> { Success = true, Data = userViewModel };
            var service = new Mock<AuthService>(_unitOfWork.Object, _userManager.Object, _signInManager.Object) { CallBase = true };
            service.Setup(s => s.LoginProcess("user", "pass")).ReturnsAsync(response);

            var result = await service.Object.Login("user", "pass");

            Assert.True(result.Success);
            Assert.Equal("Login successful", result.Message);
            Assert.Equal(userViewModel, result.Data);
        }

        [Fact]
        public async Task Login_Failure_ThrowsBadRequestException()
        {
            var response = new ResponseViewModel<UserViewModel> { Success = false, Message = "fail" };
            var service = new Mock<AuthService>(_unitOfWork.Object, _userManager.Object, _signInManager.Object) { CallBase = true };
            service.Setup(s => s.LoginProcess("user", "pass")).ReturnsAsync(response);

            await Assert.ThrowsAsync<BadRequestException>(() => service.Object.Login("user", "pass"));
        }

        [Fact]
        public async Task Logout_CallsUnitOfWorkAuthLogout()
        {
            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);
            await service.Logout();
            _authRepo.Verify(a => a.Logout(), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmailAsync_Valid_ReturnsSuccess()
        {
            _authRepo.Setup(a => a.ConfirmEmail("email", "token", It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            var result = await service.ConfirmEmailAsync("email", "token", CancellationToken.None);
            Assert.True(result.Success);
            Assert.Contains("successfully", result.Message);
        }

        [Fact]
        public async Task ConfirmEmailAsync_Invalid_ReturnsFailure()
        {
            _authRepo.Setup(a => a.ConfirmEmail("email", "token", It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var service = new AuthService(_unitOfWork.Object, _userManager.Object, _signInManager.Object);

            var result = await service.ConfirmEmailAsync("email", "token", CancellationToken.None);

            Assert.False(result.Success);
            Assert.Contains("failed", result.Message);
        }
    }
}
