using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAuthRepository : IBaseRepository<RefreshToken>
    {
        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);

        Task Logout();
        Task<ResponseViewModel> ConfirmEmail(string emailConfirmation, string activeToken, CancellationToken cancellationToken);
    }
}
