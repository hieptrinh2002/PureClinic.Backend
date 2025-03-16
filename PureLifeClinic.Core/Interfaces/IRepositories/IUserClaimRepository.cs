using Microsoft.AspNetCore.Identity;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUserClaimRepository : IBaseRepository<IdentityUserClaim<int>>
    {
    }
}
