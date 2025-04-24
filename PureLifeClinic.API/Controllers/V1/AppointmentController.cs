using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.API.Attributes;
using PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.PermissionEnums;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentService appointmentService,
            ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<AppointmentViewModel>>
                {
                    Success = true,
                    Message = "Appointment retrieved successfully",
                    Data = appointments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointment");
                throw;
            }
        }

        [HttpGet("paginated-data")]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.View)]
        public async Task<IActionResult> GetbyFilterCondition(
            int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;
                sortBy = sortBy ?? "EntryDate";
                sortOrder = sortOrder ?? "desc";

                var filters = new List<ExpressionFilter>();
                if (!string.IsNullOrWhiteSpace(search) && search != null)
                {
                    // Add filters for relevant properties
                    filters.AddRange(new[]
                    {
                        new ExpressionFilter
                        {
                            PropertyName = "ReferredPerson",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                    });
                }

                var appointments = await _appointmentService.GetPaginatedData(pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedData<AppointmentViewModel>>
                {
                    Success = true,
                    Message = "Appointments retrieved successfully",
                    Data = appointments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointments");
                throw;
            }
        }

        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPost("filter")]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.View)]

        public async Task<IActionResult> GetFilterAppointment(FilterAppointmentRequestViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentService.GetAllFilterAppointments(model, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointment");
                throw;
            }
        }

        [HttpGet("doctor/{doctorId}")]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.View)]

        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentService.GetAllAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointment");
                throw;
            }
        }

        [HttpGet("patient/{patientId}")]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.View)]

        public async Task<IActionResult> GetAppointmentsByPatient(int patientId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentService.GetAllAppointmentsByPatientIdAsync(patientId, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving appointment by Patient id - {patientId}");
                throw;
            }
        }

        // add new appointment
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPost("app/create")]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.CreateDelete)]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _appointmentService.Create(model, cancellationToken);

                var response = new ResponseViewModel<AppointmentViewModel>
                {
                    Success = true,
                    Message = "Appoinment created successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the appoinment");
                throw;
            }
        }

        // add new appointment
        [Authorize(Roles = RoleConstant.Employee)]
        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.CreateDelete)]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPost("in-person/create")]
        public async Task<IActionResult> CreateInPersonAppointment([FromBody] InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            if (await _appointmentService.IsExists(model.DoctorId, model.AppointmentDate, cancellationToken))
            {
                string message = $"The appoinment at {model.AppointmentDate} with doctor already exists";
                throw new BadRequestException(message);
            }
            try
            {
                var data = await _appointmentService.CreateInPersonAppointment(model, cancellationToken);

                var response = new ResponseViewModel<AppointmentViewModel>
                {
                    Success = true,
                    Message = "Appoinment created successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the appoinment");
                throw;
            }
        }

        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.Update)]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentService.UpdateAppointmentAsync(id, model, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the appointment");
                throw;
            }
        }

        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.Update)]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] AppointmentStatusUpdateViewModel model, int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appointmentService.UpdateAppointmentStatusAsync(id, model.Status, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the appointment status");
                throw;
            }
        }

        [HttpPost("{appointmentId}/health-services")]
        public async Task<IActionResult> AssignServiceToAppointment(
            int appointmentId, [FromBody] List<AppointmentHealthServiceCreateViewModel> model, CancellationToken cancellationToken)
        {
            List<AppointmentHealthServiceViewModel> result = await _appointmentService.AssignServiceToAppointment(appointmentId, model, cancellationToken);
            return Ok(new ResponseViewModel<List<AppointmentHealthServiceViewModel>>
            {
                Success = true,
                Message = "Services assigned successfully",
                Data = result
            });
        }

        [HttpGet("{appointmentId}/health-services/{serviceId}")]
        public async Task<IActionResult> GetAppointmentHealthService(int appointmentId, int serviceId, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAppointmentHealthService(appointmentId, serviceId, cancellationToken);
            return Ok(new ResponseViewModel<List<AppointmentHealthServiceViewModel>>
            {
                Success = true,
                Message = "Services retrieved successfully",
                Data = result
            });
        }

        [HttpDelete("{appointmentId}/health-services/{serviceId}")]
        public async Task<IActionResult> 
            DeleteAppointmentHealthService(int appointmentId, [FromBody] List<int> serviceIds, CancellationToken cancellationToken)
        {
            await _appointmentService.DeleteAppointmentHealthService(appointmentId, serviceIds, cancellationToken);
            return Ok(new ResponseViewModel
            {
                Success = true,
                Message = "Services deleted successfully",
            });
        }

        [HttpPatch("{appointmentId}/health-services")] 
        public async Task<IActionResult> UpdateAppointmentHealthService(
            int appointmentId, AppointmentHealthServiceStatus status, [FromBody] List<int> serviceIds, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(AppointmentHealthServiceStatus), status))
            {
                throw new BadRequestException("Invalid status value");
            }
            await _appointmentService.UpdateAppointmentHealthService(appointmentId, status, serviceIds, cancellationToken);
            return Ok(new ResponseViewModel
            {
                Success = true,
                Message = "Services updated successfully",
            });
        }
    }
}
