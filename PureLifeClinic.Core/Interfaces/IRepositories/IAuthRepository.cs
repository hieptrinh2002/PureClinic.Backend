
using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace Project.Core.Interfaces.IRepositories
{
    public interface IAuthRepository : IBaseRepository<RefreshToken>
    {
        Task<ResponseViewModel<RefreshTokenViewModel>> ValidateRefreshToken(string refreshToken);

        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);

        Task Logout();
    }
}
