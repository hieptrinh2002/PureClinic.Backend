using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public IAuthRepository Auth { get; private set; }
        public IProductRepository Products { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IUserRepository Users { get; private set; }
        public IWorkWeekScheduleRepository WorkWeeks { get; private set; }

        public UnitOfWork(
            ApplicationDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IUserContext userContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _userContext = userContext;
            _signInManager = signInManager;
            _mapper = mapper;

            Auth = new AuthRepository(_userManager, _signInManager, _dbContext);
            Products = new ProductRepository(_dbContext);
            Roles = new RoleRepository(_dbContext);
            Users = new UserRepository(_dbContext, _userManager, _roleManager, _userContext);
            RefreshTokens = new RefreshTokenRepository(_dbContext, _userManager, _signInManager, _mapper);
            WorkWeeks = new WorkWeekScheduleRepository(_dbContext, _userManager, _mapper);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
