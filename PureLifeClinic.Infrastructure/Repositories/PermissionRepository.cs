using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;
using System.Security.Claims;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public PermissionRepository(
            ApplicationDbContext dbContext,
            UserManager<User> userManager, 
            RoleManager<Role> roleManager) : base(dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<Claim>> GetUserClaimsPermissions(string sub, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(sub) ?? throw new Exception("User not found");
            var userPermissions = await _userManager.GetClaimsAsync(user);
            return (List<Claim>)userPermissions;
        }

        public async Task<List<Claim>> GetRoleClaimsPermissions(string sub, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(sub) ?? throw new Exception("User not found");
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null) throw new Exception("Role not found");

            var rolePermissions = await _roleManager.GetClaimsAsync(role);  
            return (List<Claim>)rolePermissions;    
        }

        public async Task<List<string>> GetUserPermissions(string sub, CancellationToken cancellationToken)
        {
            var userPermissions = await
                                (from up in _dbContext.UserPermissions
                                 join perm in _dbContext.Permissions on up.PermissionId equals perm.Id
                                 join user in _dbContext.Users on up.UserId equals user.Id
                                 where user.Id == int.Parse(sub)
                                 select perm.Name)
                                 .ToListAsync(cancellationToken);
            return userPermissions; 
        }
    }
}
