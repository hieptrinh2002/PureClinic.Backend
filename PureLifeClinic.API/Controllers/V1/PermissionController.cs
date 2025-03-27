using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.BusinessObjects.PermissionViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary> Lock all permissions of a user </summary>
        [HttpPost("permission/user/{userId}/lock-all")]
        public async Task<IActionResult> LockUserPermissions(int userId)
        {
            await _permissionService.LockUserPermissions(userId);
            return Ok(new ResponseViewModel { Success = true, Message = "User permissions locked successfully" });
        }

        [HttpPost("permission/user/{userId}/unlock-all")]
        public async Task<IActionResult> UnLockUserPermissions(int userId)
        {
            await _permissionService.UnLockUserPermissions(userId);
            return Ok(new ResponseViewModel { Success = true, Message = "Unlock User permissions successfully" });
        }

        // Update Role Permission on resoure
        [HttpPut("role/{roleId}/resource/permission/update")]
        public async Task<IActionResult> UpdateRolePermission(
            int roleId, [FromBody] ResourcePermissionViewModel model, CancellationToken cancellationToken)
        {
            await _permissionService.UpdateRolePermission(roleId, model, cancellationToken);
            return Ok(new ResponseViewModel { Success = true, Message = "Role permission updated" });
        }

        // Update List of Role Permission on list resoure
        [HttpPut("role/{roleId}/resources/permissions/update")]
        public async Task<IActionResult> UpdateRolePermissions(
            int roleId, [FromBody] List<ResourcePermissionViewModel> models, CancellationToken cancellationToken)
        {
           await _permissionService.UpdateRolePermissions(roleId, models, cancellationToken);
            return Ok(new ResponseViewModel { Success = true, Message = "Role permissions updated" });
        }

        // Update List of user Permission on list resoure
        [HttpPut("user/{userId}/resources/permissions/update")]
        public async Task<IActionResult> UpdateRolePermission(
            int userId,[FromBody] List<ResourcePermissionViewModel> models, CancellationToken cancellationToken)
        {
            await _permissionService.UpdateUserPermissions(userId, models, cancellationToken);
            return Ok(new ResponseViewModel { Success = true,  Message = "user permissions updated" });
        }

        /// <summary> Get all permissions of a user </summary>
        [HttpGet("user/{userId}/permissions")]
        public async Task<IActionResult> GetUserPermissions(int userId, CancellationToken cancellationToken)
        {
            var permissions = await _permissionService.GetUserPermissionsIdentityAsync(userId, cancellationToken);
            return Ok(
                new ResponseViewModel<Dictionary<string, int>>
                {
                    Success = true,
                    Message = "Get all permissions successfully",
                    Data = permissions
                });
        }

        /// <summary> Get all permissions of a role </summary>
        [HttpGet("role/{roleId}/permissions")]
        public async Task<IActionResult> GetRolePermissions(int roleId, CancellationToken cancellationToken)
        {
            var permissions = await _permissionService.GetRolePermissions(roleId, cancellationToken);
            return Ok(
                new ResponseViewModel<Dictionary<string, int>>
                {
                    Success = true,
                    Message = "Get all permissions successfully",
                    Data = permissions
                });
        }

        /// <summary> Add new role permissions</summary>
        [HttpPost("role/{roleId}/permission/add")]
        public async Task<ResponseViewModel> AddNewPermissionsToRole
            (int roleId, List<ResourcePermissionViewModel> model, CancellationToken cancellationToken)
        {
            await _permissionService.AddNewPermissions(roleId, PermissionType.RolePermission, model, cancellationToken);
            var response = new ResponseViewModel
            {
                Success = true,
                Message = "role permission created successfully",
            };
            return response;
        }

        /// <summary> Add new role permissions</summary>
        [HttpPost("user/{userId}/permission/add")]
        public async Task<ResponseViewModel> AddNewPermissionsToUser
            (int userId, List<ResourcePermissionViewModel> model, CancellationToken cancellationToken)
        {
            await _permissionService.AddNewPermissions(userId, PermissionType.UserPermission, model, cancellationToken);
            var response = new ResponseViewModel
            {
                Success = true,
                Message = "user permission created successfully",
            };
            return response;
        }
    }
}
