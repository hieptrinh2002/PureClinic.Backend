using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<bool> ValidateRefreshToken(string refreshToken);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshToken(string rfToken);
    }
}
