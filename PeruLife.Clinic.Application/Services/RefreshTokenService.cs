using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken)
        {
            var refreshToken = _mapper.Map<RefreshToken>(refreshTokenModel);
            refreshToken.UserId = userId;

            await _unitOfWork.RefreshTokens.Create(refreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel<RefreshTokenViewModel>
            {
                Success = true,
                Message = "create refresh token successful",
                Data = _mapper.Map<RefreshTokenViewModel>(refreshToken)
            };
        }

        public async Task<bool> RefreshTokenCheckAsync(string refreshToken)
        {
            //find the user that match the sent refresh token
            bool result = await _unitOfWork.RefreshTokens.ValidateRefreshToken(refreshToken);
            if (!result)
            {
                throw new NotFoundException("Invalid refresh token");
            }
            return result;
        }
    }
}
