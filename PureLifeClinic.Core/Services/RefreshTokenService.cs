using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
        {
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken)
        {
            var refreshToken = _mapper.Map<RefreshToken>(refreshTokenModel);
            refreshToken.UserId = userId;

            await _refreshTokenRepository.Create(refreshToken, cancellationToken);

            return new ResponseViewModel<RefreshTokenViewModel>
            {
                Success = true,
                Message = "create refresh token successful",
                Data = _mapper.Map<RefreshTokenViewModel>(refreshToken)
            };
        }


        public async Task<ResponseViewModel<RefreshTokenViewModel>> RefreshTokenCheckAsync(string refreshToken)
        {
            //find the user that match the sent refresh token
            var result = await _refreshTokenRepository.ValidateRefreshToken(refreshToken);
            if (!result.Success)
            {
                throw new NotFoundException("Invalid refresh token");
            }
            return result;
        }
    }
}
