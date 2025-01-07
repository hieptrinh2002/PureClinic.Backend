using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
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
                    Data = _mapper.Map<RefreshTokenViewModel>(rfToken)
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
