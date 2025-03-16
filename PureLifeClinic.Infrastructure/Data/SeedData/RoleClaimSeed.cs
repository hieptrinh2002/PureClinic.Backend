using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public class RoleClaimSeed
    {
        public static async Task SeedRoleClaims(ApplicationDbContext context)
        {
            var roles = context.Roles.ToList();
            var roleClaims = new List<IdentityRoleClaim<int>>();

            foreach (var role in roles)
            {
                foreach (var permission in ResourceConstants.Permissions)
                {
                    int permissionValue = 0;

                    switch (role.Code)
                    {
                        case "ADMIN":
                            permissionValue = (int)(PermissionAction.View | PermissionAction.CreateDelete | PermissionAction.Update |
                                                    PermissionAction.ActiveDeactive | PermissionAction.Approve |
                                                    PermissionAction.Send | PermissionAction.ImportExport | PermissionAction.LockUnlock);
                            break;

                        case "EMPLOYEE":
                            if (permission == ResourceConstants.User ||
                                permission == ResourceConstants.Patient ||
                                permission == ResourceConstants.Appointment ||
                                permission == ResourceConstants.Invoice)
                            {
                                permissionValue = (int)(PermissionAction.View | PermissionAction.CreateDelete | PermissionAction.Update |
                                                        PermissionAction.Approve | PermissionAction.ImportExport);
                            }
                            break;

                        case "DOCTOR":
                            if (permission == ResourceConstants.Patient || permission == ResourceConstants.MedicalReport)
                            {
                                permissionValue = (int)(PermissionAction.View | PermissionAction.Update);
                            }
                            break;

                        case "PATIENT":
                            if (permission == ResourceConstants.Patient)
                            {
                                permissionValue = (int)PermissionAction.View;
                            }
                            break;
                    }

                    if (permissionValue > 0)
                    {
                        var existingClaim = context.RoleClaims
                            .FirstOrDefault(rc => rc.RoleId == role.Id && rc.ClaimType == permission);

                        if (existingClaim == null)
                        {
                            var roleClaim = new IdentityRoleClaim<int>
                            {
                                RoleId = role.Id,
                                ClaimType = permission,
                                ClaimValue = permissionValue.ToString()
                            };

                            roleClaims.Add(roleClaim);
                        }
                    }
                }
            }

            if (roleClaims.Any())
            {
                await context.RoleClaims.AddRangeAsync(roleClaims);
                await context.SaveChangesAsync();
            }
        }
    }
}
