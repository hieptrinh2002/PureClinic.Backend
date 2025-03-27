using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public RefreshTokenRepository(
            ApplicationDbContext dbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
        }

        public async Task<bool> RevokeRefreshToken(string rfToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == rfToken);

            if (token == null || !token.IsActive)
            {
                return false;
            }

            token.RevokedOn = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var users = _userManager.Users;

            //lazy loading => refreshToken will not be loaded
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));

            //eager loading 
            var user = await GetUserByRefreshTokenAsync(refreshToken);

            if (user == null)
                return false;

            var result = await RevokeRefreshToken(refreshToken);
            if (!result)
                throw new Exception(" Revoke refresh token failed");
            return true;
        }
    }
}
