using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    class UserClaimRepository : BaseRepository<IdentityUserClaim<int>>, IUserClaimRepository
    {
        public UserClaimRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
