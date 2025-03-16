using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    class RoleClaimRepository : BaseRepository<IdentityRoleClaim<int>>, IRoleClaimRepository
    {
        public RoleClaimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
