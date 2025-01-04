﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IMapper;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
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

        public async Task<ResponseViewModel> ConfirmEmail(string emailConfirmation, string activeToken, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(emailConfirmation);
            if (user == null) {
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
                    Message = "User is not active or username not found"
                };
            }
            if (! await _userManager.IsEmailConfirmedAsync(user))
            {
                return new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = "Email is not confirmed"
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
                    Success = false,
                    Message = "login failed"
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

            //lazy loading => refreshToken will not be loaded
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));

            //eager loading 
            var user = await _userManager.Users.Include(token => token.RefreshTokens).FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));

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
                    Success = false,
                };
            }
        }
    }
}
