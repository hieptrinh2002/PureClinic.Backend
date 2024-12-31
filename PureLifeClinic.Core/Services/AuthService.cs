using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IMapper;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class AuthService : BaseService<RefreshToken, RefreshTokenViewModel>, IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        private readonly IUserContext _userContext;
        private readonly IBaseMapper<RefreshTokenCreateViewModel, RefreshToken> _refreshTokenCreateMapper;
        private readonly IBaseMapper<RefreshToken, RefreshTokenViewModel> _refreshTokenViewModelMapper;

        public AuthService(
            IAuthRepository authRepository,
            IUserRepository userRepository,
            IUserContext userContext,
            IBaseMapper<RefreshTokenCreateViewModel, RefreshToken> refreshTokenCreateMapper,
            IBaseMapper<RefreshToken, RefreshTokenViewModel> refreshTokenViewModelMapper) : base(refreshTokenViewModelMapper,authRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _userContext = userContext;
            _refreshTokenCreateMapper = refreshTokenCreateMapper;
            _refreshTokenViewModelMapper = refreshTokenViewModelMapper;
        }

        public async Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken)
        {
            var refreshToken = _refreshTokenCreateMapper.MapModel(refreshTokenModel);
            refreshToken.UserId = userId;

            await _authRepository.Create(refreshToken, cancellationToken);

            return new ResponseViewModel<RefreshTokenViewModel>
            {
                Success = true,
                Message = "create refresh token successful",
                Data = _refreshTokenViewModelMapper.MapModel(refreshToken)
            };
        }

        public async Task<ResponseViewModel<RefreshTokenViewModel>> RefreshTokenCheckAsync(string refreshToken)
        {
            //find the user that match the sent refresh token
            var result = await _authRepository.ValidateRefreshToken(refreshToken);
            if (!result.Success)
            {
                throw new NotFoundException("Invalid refresh token");
            }
            return result;  
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
                        Message = "Incorrect username or password or not confirm email. Please check your credentials and try again."
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
