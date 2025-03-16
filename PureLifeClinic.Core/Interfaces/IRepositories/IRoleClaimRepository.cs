using Microsoft.AspNetCore.Identity;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IRoleClaimRepository : IBaseRepository<IdentityRoleClaim<int>>
    {
    }
}
