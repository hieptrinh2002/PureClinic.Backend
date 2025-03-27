using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> ConfirmEmail(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(emailConfirmation);
            if (user == null)
            {
                return false;
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, activeToken);
            if (confirmResult.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
