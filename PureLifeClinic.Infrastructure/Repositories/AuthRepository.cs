using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Interfaces.IMapper;
using Project.Core.Interfaces.IRepositories;
using Project.Infrastructure.Data;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace Project.Infrastructure.Repositories
{
    public class AuthRepository : BaseRepository<RefreshToken>, IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBaseMapper<RefreshToken, RefreshTokenViewModel> _refreshTokenViewModelMapper;

        public AuthRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IBaseMapper<RefreshToken, RefreshTokenViewModel> refreshTokenViewModelMapper,
            ApplicationDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _refreshTokenViewModelMapper = refreshTokenViewModelMapper;
        }
     
        public async Task<ResponseViewModel<UserViewModel>> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !user.IsActive)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                };
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = true,
                    Data = new UserViewModel { Id = user.Id, UserName = user.UserName },
                };
            }
            else
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false
                };
            }

        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

      
        public async Task<ResponseViewModel<RefreshTokenViewModel>> ValidateRefreshToken(string refreshToken)
        {
            var users = _userManager.Users;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
            {
                return new ResponseViewModel<RefreshTokenViewModel>
                {
                    Success = false,
                };
            }
            try
            {
                var rfToken = user.RefreshTokens?.Single(t => t.Token == refreshToken);
                if (rfToken == null || !rfToken.IsActive)
                {
                    return new ResponseViewModel<RefreshTokenViewModel>
                    {
                        Success = false,
                    };
                }

                rfToken.RevokedOn = DateTime.UtcNow;
                _dbContext.SaveChanges();

                return new ResponseViewModel<RefreshTokenViewModel>
                {
                    Success = true,
                    Data = _refreshTokenViewModelMapper.MapModel(rfToken)
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<RefreshTokenViewModel>
                {
                    Success = true,
                };
            }
        }
    }
}
