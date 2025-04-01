using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ResetPassword;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IUserService : IBaseService<UserViewModel>
    {
        new Task<IEnumerable<UserViewModel>> GetAll(CancellationToken cancellationToken);
        new Task<PaginatedData<UserViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<UserViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
        Task<ResponseViewModel> Create(UserCreateViewModel model, CancellationToken cancellationToken);
        Task<ResponseViewModel> Update(UserUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel> ResetPasswordAsync(string email, string token, string newPassword);
        Task<IEnumerable<PatientViewModel>> GetAllPatient(CancellationToken cancellationToken);
        Task<ResponseViewModel<EmailActivationViewModel>> GenerateEmailConfirmationTokenAsync(string email);
        Task<bool> UnlockAccountAsync(int userId);
        Task<ResponseViewModel<ResetPasswordViewModel>> GenerateResetPasswordTokenAsync(ForgotPasswordRequestViewModel model);
    }
}
