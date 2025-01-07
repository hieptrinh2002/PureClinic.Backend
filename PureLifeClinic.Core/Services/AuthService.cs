using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IAuthRepository authRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper)
        {
            _authRepository = authRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
        }

        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            var result = await _authRepository.Login(userName, password);
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
            await _authRepository.Logout();
        }

        public async Task<ResponseViewModel> ConfirmEmailAsync(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var result = await _authRepository.ConfirmEmail(emailConfirmation, activeToken, cancellationToken);
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
