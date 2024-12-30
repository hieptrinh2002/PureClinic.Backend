using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken);
        Task<ResponseViewModel<RefreshTokenViewModel>> RefreshTokenCheckAsync(string refreshToken);
        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
        Task Logout();
    }
}
