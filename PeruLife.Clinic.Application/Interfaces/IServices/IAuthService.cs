using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ResponseViewModel<UserViewModel>> Login(string userName, string password);
        Task Logout();
        Task<ResponseViewModel> ConfirmEmailAsync(string emailConfirmation, string activeToken, CancellationToken cancellationToken);
    }
}
