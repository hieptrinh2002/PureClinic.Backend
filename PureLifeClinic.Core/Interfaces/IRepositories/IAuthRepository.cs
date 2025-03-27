using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAuthRepository : IBaseRepository<RefreshToken>
    {
        Task Logout();
        Task<bool> ConfirmEmail(string emailConfirmation, string activeToken, CancellationToken cancellationToken);
    }
}
