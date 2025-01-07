using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            var result = await _unitOfWork.Auth.Login(userName, password);
            if (result.Success)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result.Data
                };
            }
            else
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Login failed",
                    Error = new ErrorViewModel
                    {
                        Code = "LOGIN_ERROR",
                        Message = result.Message
                    }
                };
            }
        }

        public async Task Logout()
        {
            await _unitOfWork.Auth.Logout();
        }

        public async Task<ResponseViewModel> ConfirmEmailAsync(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Auth.ConfirmEmail(emailConfirmation, activeToken, cancellationToken);
            if (result.Success) {
                return new ResponseViewModel
                {
                    Success = true,
                    Message = "Confirm email successfully "
                };
            }
            return  new ResponseViewModel
            {
                Success = false,
                Message = "Confirm email failed "
            };
        }
    }
}
