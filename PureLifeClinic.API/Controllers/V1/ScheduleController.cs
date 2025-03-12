using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScheduleController : ControllerBase
    {
        private readonly IWorkWeekScheduleService _workWeekScheduleService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger, IWorkWeekScheduleService workWeekScheduleService)
        {
            _workWeekScheduleService = workWeekScheduleService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterWorkSchedule([FromBody] WorkScheduleRequestViewModel request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

            string message = string.Empty;
            try
            {
                var result = await _workWeekScheduleService.RegisterWorkScheduleAsync(request, cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while register Work Schedule");
                message = $"An error occurred while register Work Schedule - " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "ADD_WORK_SCHEDULE_ERROR",
                        Message = message
                    }
                });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWorkSchedule(int userId, [FromQuery] DateTime weekStartDate, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            try
            {
                var result = await _workWeekScheduleService.GetWorkSchedule(userId, weekStartDate, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the  Work Schedule");
                message = $"An error occurred while retrieving the Work Schedule - " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "GET_WORK_SCHEDULE_ERROR",
                        Message = message
                    }
                });
            }
        }
    }
}
