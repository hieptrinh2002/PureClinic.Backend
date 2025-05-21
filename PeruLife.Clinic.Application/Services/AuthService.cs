using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PeruLife.Clinic.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager )
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager; 
        }

        public async Task<ResponseViewModel<UserViewModel>> LoginProcess(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !user.IsActive)
                throw new NotFoundException("Username not found");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new BadRequestException("Email is not confirmed, please check and activate your account !");

            if (await _userManager.IsLockedOutAsync(user))
                throw new BadRequestException("Account is locked. Please try again later.");

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded)
                throw new BadRequestException("invalid password.");

            var role = await _unitOfWork.Roles.GetById(user.RoleId, default);
            return new ResponseViewModel<UserViewModel>
            {
                Success = true,
                Data = new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = role.Name },
            };
        }

        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            var result = await LoginProcess(userName, password);
            if (!result.Success)
                throw new BadRequestException(result.Message);

            return new ResponseViewModel<UserViewModel>
            {
                Success = true,
                Message = "Login successful",
                Data = result.Data
            };
        }

        public async Task Logout()
        {
            await _unitOfWork.Auth.Logout();
        }

        public async Task<ResponseViewModel> ConfirmEmailAsync(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var isValid = await _unitOfWork.Auth.ConfirmEmail(emailConfirmation, activeToken, cancellationToken);
            if (isValid)
            {
                return new ResponseViewModel
                {
                    Success = true,
                    Message = "Confirm email successfully "
                };
            }
            return new ResponseViewModel
            {
                Success = false,
                Message = "Confirm email failed "
            };
        }
    }
}
