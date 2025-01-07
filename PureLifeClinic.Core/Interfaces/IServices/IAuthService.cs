using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
        Task Logout();
        Task<ResponseViewModel> ConfirmEmailAsync(string emailConfirmation, string activeToken, CancellationToken cancellationToken);
    }
}
