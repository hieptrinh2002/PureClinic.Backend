using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Infrastructure.Data;
using System.Runtime.CompilerServices;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserContext _userContext;

        public UserRepository(
            ApplicationDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUserContext userContext
            ) : base(dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userContext = userContext;
        }

        public async Task<IdentityResult> Create(UserCreateViewModel model)
        {
            // Check if the role exists by Id, if not, return an error
            var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = $"Role with Id {model.RoleId} not found." });
            }

            if (!role.IsActive)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RoleInactive", Description = $"Inactive Role" });
            }

            var user = new User
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = true,
                RoleId = model.RoleId,
                EntryDate = DateTime.Now,
                EntryBy = Convert.ToInt32(_userContext.UserId)
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            // If user creation is successful, assign the role to the user
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return result;
        }

        public async Task<IdentityResult> Update(UserUpdateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Check if the role exists by Id, if not, return an error
            var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = $"Role with Id {model.RoleId} not found." });
            }

            if (!role.IsActive)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RoleInactive", Description = $"Inactive Role" });
            }

            // Update the user properties
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.RoleId = model.RoleId;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            var result = await _userManager.UpdateAsync(user);

            // If user update is successful, assign the role to the user
            if (result.Succeeded)
            {
                // Remove existing roles
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                // Add the new role
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return result;
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            return result;
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(email);
            return user;
        }

        public async Task<EmailActivationViewModel> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (token == null)
                return null;

            return new EmailActivationViewModel
            {
                UserId  = user.Id,
                ActivationToken = token,
            };   
        }
    }
}
