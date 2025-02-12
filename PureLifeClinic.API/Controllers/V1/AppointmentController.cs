﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IServices;

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

        public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
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
        [AllowAnonymous]
        public async Task<IActionResult> GetbyFilterCondition(int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
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

                var appointmnets = await _appointmentService.GetPaginatedData(pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedDataViewModel<AppointmentViewModel>>
                {
                    Success = true,
                    Message = "Appointmnets retrieved successfully",
                    Data = appointmnets
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointmnets");
                throw;
            }
        }

        [HttpPost("filter")]
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
        [HttpPost("app/create")]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);
            
            string message = string.Empty;
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
        [HttpPost("in-person/create")]
        public async Task<IActionResult> CreateInPersonAppointment(InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);

            string message = string.Empty;
            if (await _appointmentService.IsExists(model.DoctorId, model.AppointmentDate, cancellationToken))
            {
                message = $"The appoinment at {model.AppointmentDate} with doctor already exists";
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
                message = $"An error occurred while adding the appoinment- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<AppointmentViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "ADD_APPOINTMENT_ERROR",
                        Message = message
                    }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);
            }
            string message = string.Empty;

            try
            {
                var result = await _appointmentService.UpdateAppointmentAsync(id, model, cancellationToken);

                if (!result.Success)
                {
                    return Ok(new ResponseViewModel
                    {
                        Success = false,
                        Message = result.Message,
                    });
                }

                return Ok(new ResponseViewModel
                {
                    Success = true,
                    Message = "Appointment updated successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the appointment");
                message = $"An error occurred while updating the appointment- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<AppointmentViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "UPDATE_APPOINTMENT_ERROR",
                        Message = message
                    }
                });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] AppointmentStatusUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid input: " + ModelStateHelper.GetErrors(ModelState), ErrorCode.InputValidateError);
            }
            try
            {
                var result = await _appointmentService.UpdateAppointmentStatusAsync(id, model.Status, cancellationToken);

                if (!result.Success)
                {
                    return Ok(new ResponseViewModel
                    {
                        Success = false,
                        Message = result.Message,
                    });
                }

                return Ok(new ResponseViewModel
                {
                    Success = true,
                    Message = "Appointment status updated successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the appointment status");
               throw;
            }
        }
    }
}
