using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IdentityResult> Create(User model, string password);
        Task<IdentityResult> Update(User model);
        Task<User> GetByEmail(string email, CancellationToken cancellationToken);
        Task<(int UserId, string Token)> GenerateEmailConfirmationTokenAsync(string email);
        Task<IEnumerable<User>> GetAllDoctor(CancellationToken cancellationToken);
        Task<IdentityResult> CreateDoctor(Doctor model);
        Task<IdentityResult> CreatePatient(Patient model);
        Task<User> GetDoctorById(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllPatient(CancellationToken cancellationToken);
        Task<bool> UnlockAccountAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<string> GenerateResetPasswordTokenAsync(User user);
    }
}
