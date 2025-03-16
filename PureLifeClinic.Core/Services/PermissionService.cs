using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Security.Claims;

namespace PureLifeClinic.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserContext _userContext;

        public PermissionService(
            IUnitOfWork unitOfWork,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _userContext = userContext;
        }

        public async Task AddNewPermissions(
            int id, PermissionType type, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken)
        {
            if (type == PermissionType.RolePermission)
            {
                var role = await _roleManager.FindByIdAsync(id.ToString())
                ?? throw new Exception("Role not found");
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id.ToString())
                ?? throw new Exception("User not found");
            }


            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            foreach (var permission in models)
            {
                var filters = new List<ExpressionFilter>() {
                    new()
                    {
                        PropertyName = "ClaimType",
                        Value = permission.ResourceName,
                        Comparison = Comparison.Equal
                    }
                };

                if (type == PermissionType.RolePermission)
                {
                    var existingClaim = await _unitOfWork.RoleClaims.GetAll(filters, cancellationToken);

                    if (existingClaim.Any())
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        throw new ErrorException($"Permission on resource - {permission.ResourceName} of role with id - {id} is already exists");
                    }

                    var identityRoleClaim = new IdentityRoleClaim<int>
                    {
                        RoleId = id,
                        ClaimType = permission.ResourceName,
                        ClaimValue = permission.PermissionValue.ToString()
                    };

                    await _unitOfWork.RoleClaims.Create(identityRoleClaim, cancellationToken);
                }
                else
                {
                    var existingClaim = await _unitOfWork.UserClaims.GetAll(filters, cancellationToken);

                    if (existingClaim.Any())
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        throw new ErrorException($"Permission on resource - {permission.ResourceName} of user with id - {id} is already exists");
                    }

                    var identityUserClaim = new IdentityUserClaim<int>
                    {
                        UserId = id,
                        ClaimType = permission.ResourceName,
                        ClaimValue = permission.PermissionValue.ToString()
                    };

                    await _unitOfWork.UserClaims.Create(identityUserClaim, cancellationToken);
                }
            }
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<Dictionary<string, int>> GetRolePermissions(int roleId, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString())
                ?? throw new Exception("Role not found");

            var roleClaims = await _unitOfWork.Permissions.GetRoleClaimsPermissions(role.Id, cancellationToken);
            return roleClaims.ToDictionary(rc => rc.Type, rc => int.Parse(rc.Value));
        }

        public async ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(int sub, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Permissions.GetUserPermissions(sub, cancellationToken);

            List<Claim> claimPermissions = permissions.Select(p => new Claim(AppClaimTypes.Permission, p)).ToList();

            if (!claimPermissions.Any())
                return null;
            var permissionsIdentity = new ClaimsIdentity("PermissionsMiddleware", "name", "role");
            permissionsIdentity.AddClaims(claimPermissions);

            return permissionsIdentity;
        }

        public async Task<Dictionary<string, int>> GetUserPermissionsIdentityAsync(int sub, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            var user = await _unitOfWork.Users.GetById(sub, cancellationToken)
                ?? throw new Exception("User not found");

            if (user.IsLockPermission)
            {
                return result;
            }

            var roleClaims = await _unitOfWork.Permissions.GetRoleClaimsPermissions(user.RoleId, cancellationToken);

            var userClaims = await _unitOfWork.Permissions.GetUserClaimsPermissions(sub, cancellationToken);

            // Role Claims
            foreach (var roleClaim in roleClaims)
            {
                string resourceName = roleClaim.Type;
                int rolePermission = int.Parse(roleClaim.Value);

                if (rolePermission == ResourceConstants.Deny) continue;

                if (!result.ContainsKey(resourceName))
                {
                    result[resourceName] = rolePermission;
                }
                else
                {
                    result[resourceName] |= rolePermission;
                }
            }

            // User Claims
            foreach (var userClaim in userClaims)
            {
                string resourceName = userClaim.Type;
                int userPermission = int.Parse(userClaim.Value);

                if (userPermission == ResourceConstants.Deny) continue;

                if (!result.ContainsKey(resourceName))
                {
                    result[resourceName] = userPermission;
                }
                else
                {
                    result[resourceName] |= userPermission;
                }
            }

            return result;
        }

        public async Task LockUserPermissions(int userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new Exception("User not found");

            user.IsLockPermission = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to lock user permissions");
        }

        public async Task UnLockUserPermissions(int userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new Exception("User not found");

            user.IsLockPermission = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to unlock user permissions");
        }

        public async Task UpdateRolePermission(int roleId, ResourcePermissionViewModel model, CancellationToken cancellationToken)
        {
            var roleClaims = await _unitOfWork.Permissions.GetRoleClaimsPermissions(roleId, cancellationToken);

            var existingClaim = roleClaims.FirstOrDefault(rc => rc.Type == model.ResourceName)
                ?? throw new ErrorException($"Permission on resource - {model.ResourceName} of role with id - {roleId} is not found");

            var identityRoleClaim = new IdentityRoleClaim<int>
            {
                RoleId = roleId,
                ClaimType = model.ResourceName,
                ClaimValue = model.PermissionValue.ToString()
            };
            await _unitOfWork.RoleClaims.Update(identityRoleClaim, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRolePermissions(int roleId, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            foreach (var model in models)
            {
                var roleClaims = await _unitOfWork.Permissions.GetRoleClaimsPermissions(roleId, cancellationToken);

                var existingClaim = roleClaims.FirstOrDefault(rc => rc.Type == model.ResourceName);
                if (existingClaim == null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw new ErrorException($"Permission on resource - {model.ResourceName} of role with id - {roleId} is not found");
                }

                var identityRoleClaim = new IdentityRoleClaim<int>
                {
                    RoleId = roleId,
                    ClaimType = model.ResourceName,
                    ClaimValue = model.PermissionValue.ToString()
                };
                await _unitOfWork.RoleClaims.Update(identityRoleClaim, cancellationToken);
            }
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserPermissions(int userId, List<ResourcePermissionViewModel> models, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            foreach (var model in models)
            {
                var userClaims = await _unitOfWork.Permissions.GetUserClaimsPermissions(userId, cancellationToken);

                var existingClaim = userClaims.FirstOrDefault(rc => rc.Type == model.ResourceName);
                if (existingClaim == null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw new ErrorException($"Permission on resource - {model.ResourceName} of user with id - {userId} is not found");
                }

                var identityUserClaim = new IdentityUserClaim<int>
                {
                    UserId = userId,
                    ClaimType = model.ResourceName,
                    ClaimValue = model.PermissionValue.ToString()
                };
                await _unitOfWork.UserClaims.Update(identityUserClaim, cancellationToken);
            }
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
