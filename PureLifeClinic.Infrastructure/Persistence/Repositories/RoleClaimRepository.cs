using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    class RoleClaimRepository : BaseRepository<IdentityRoleClaim<int>>, IRoleClaimRepository
    {
        public RoleClaimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
