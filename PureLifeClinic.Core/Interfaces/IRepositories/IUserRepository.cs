using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IdentityResult> Create(UserCreateViewModel model);
        Task<IdentityResult> Update(UserUpdateViewModel model);
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
        Task<EmailActivationViewModel> GenerateEmailConfirmationTokenAsync(string email);
        Task<IEnumerable<User>> GetAllDoctor(CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllPatient(CancellationToken cancellationToken);
        Task<bool> UnlockAccountAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    }
}
