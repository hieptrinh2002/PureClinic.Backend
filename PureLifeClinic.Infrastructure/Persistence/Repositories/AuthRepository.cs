using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class AuthRepository : BaseRepository<RefreshToken>, IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResponseViewModel> ConfirmEmail(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(emailConfirmation);
            if (user == null)
            {
                return new ResponseViewModel { Success = false };
            }

            var confirmReult = await _userManager.ConfirmEmailAsync(user, activeToken);
            if (confirmReult.Succeeded)
            {
                return new ResponseViewModel { Success = true };
            }
            return new ResponseViewModel { Success = false };
        }

        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !user.IsActive)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Username not found"
                };
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Email is not confirmed, please check and activate your account !"
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Account is locked. Please try again later."
                };

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
                if (role != null)
                {
                    return new ResponseViewModel<UserViewModel>
                    {
                        Success = true,
                        Data = new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = role.Name },
                    };
                }
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "user role is not found"
                };
            }
            else
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "login failed"
                };
            }

        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
