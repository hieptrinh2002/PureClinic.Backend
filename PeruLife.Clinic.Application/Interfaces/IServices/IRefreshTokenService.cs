using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IRefreshTokenService
    {
        Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken);
        Task<bool> RefreshTokenCheckAsync(string refreshToken);
    }
}
