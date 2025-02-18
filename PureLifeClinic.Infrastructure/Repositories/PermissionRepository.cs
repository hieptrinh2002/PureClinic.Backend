using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;
using System.Security.Claims;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(string sub, CancellationToken cancellationToken)
        {
            var userPermissions = await
                                (from up in _dbContext.UserPermissions
                                 join perm in _dbContext.Permissions on up.PermissionId equals perm.Id
                                 join user in _dbContext.Users on up.UserId equals user.Id
                                 where user.Id == int.Parse(sub)
                                 select new Claim(AppClaimTypes.Permission, perm.Name)).ToListAsync(cancellationToken);
            return CreatePermissionsIdentity(userPermissions);
        } 

        private static ClaimsIdentity? CreatePermissionsIdentity(IReadOnlyCollection<Claim> claimPermissions)
        {
            if (!claimPermissions.Any())
                return null;

            var permissionsIdentity = new ClaimsIdentity("PermissionsMiddleware", "name", "role");
            permissionsIdentity.AddClaims(claimPermissions);

            return permissionsIdentity;
        }
    }
}
