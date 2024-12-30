using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IdentityResult> Create(UserCreateViewModel model);
        Task<IdentityResult> Update(UserUpdateViewModel model);
        Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);
    }
}
