using Project.Core.Entities.Business;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace Project.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ResponseViewModel<RefreshTokenViewModel>> InsertRefreshToken(int userId, RefreshTokenCreateViewModel refreshTokenModel, CancellationToken cancellationToken);
        Task<ResponseViewModel<RefreshTokenViewModel>> RefreshTokenCheckAsync(string refreshToken);
        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
        Task Logout();
    }
}
