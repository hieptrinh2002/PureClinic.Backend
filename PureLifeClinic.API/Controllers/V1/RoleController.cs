using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Response;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(ILogger<RoleController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;

                var roles = await _roleService.GetPaginatedData(pageNumberValue, pageSizeValue, cancellationToken);

                var response = new ResponseViewModel<PaginatedData<RoleViewModel>>
                {
                    Success = true,
                    Message = "Roles retrieved successfully",
                    Data = roles
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving roles");
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _roleService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<RoleViewModel>>
                {
                    Success = true,
                    Message = "Roles retrieved successfully",
                    Data = roles
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving roles");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _roleService.GetById(id, cancellationToken);

                var response = new ResponseViewModel<RoleViewModel>
                {
                    Success = true,
                    Message = "Role retrieved successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the role");
                throw;
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        public async Task<IActionResult> Create(RoleCreateViewModel model, CancellationToken cancellationToken)
        {

            string message = "";
            if (await _roleService.IsExists("Name", model.Name, cancellationToken))
            {
                message = $"The role name- '{model.Name}' already exists";
                _logger.LogError(message);
                throw new BadRequestException(message);
            }

            if (await _roleService.IsExists("Code", model.Code, cancellationToken))
            {
                message = $"The role code- '{model.Code}' already exists";
                _logger.LogError(message);
                throw new BadRequestException(message);
            }

            try
            {
                var data = await _roleService.Create(model, cancellationToken);

                var response = new ResponseViewModel<RoleViewModel>
                {
                    Success = true,
                    Message = "Role created successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the role");
                throw;
            }
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        public async Task<IActionResult> Edit(RoleUpdateViewModel model, CancellationToken cancellationToken)
        {

            string message = "";
            if (await _roleService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
            {
                message = $"The role name- '{model.Name}' already exists";
                _logger.LogError(message);
                throw new BadRequestException(message);
            }

            if (await _roleService.IsExistsForUpdate(model.Id, "Code", model.Code, cancellationToken))
            {
                message = $"The role code- '{model.Code}' already exists";
                _logger.LogError(message);
                throw new BadRequestException(message);
            }

            try
            {
                await _roleService.Update(model, cancellationToken);

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Role updated successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the role");
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _roleService.Delete(id, cancellationToken);

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Role deleted successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "An error occurred while deleting the role");
                throw;
            }
        }
    }
}
