﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Attributes;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.PermissionEnums;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorService _doctorService;

        public DoctorController(ILogger<DoctorController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        [PermissionAuthorize(ResourceConstants.Patient, PermissionAction.View)]
        [HttpGet("{doctorId}/patients/paginated-data")]
        public async Task<IActionResult> GetPatients(
            int doctorId,
            int? pageNumber,
            int? pageSize,
            string? search,
            string? sortBy,
            string? sortOrder,
            PatientStatus patientStatus,
            CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;
                sortBy = sortBy ?? "Id";
                sortOrder = sortOrder ?? "desc";

                var filters = new List<ExpressionFilter>()
                {
                    new()
                    {
                        PropertyName = "PatientStatus",
                        Value = patientStatus,
                        Comparison = Comparison.Equal
                    },
                    new()
                    {
                        PropertyName = "User.Name",
                        Value = search,
                        Comparison = Comparison.Contains
                    },
                };

                var patientViewModels = await _doctorService.GetPagtinatedPatientData(doctorId, pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedData<PatientViewModel>>
                {
                    Success = true,
                    Message = "doctors retrieved successfully",
                    Data = patientViewModels
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving patients of doctor - {doctorId}");
                throw;
            }
        }

        [PermissionAuthorize(ResourceConstants.Doctor, PermissionAction.View)]
        [HttpGet("paginated-data")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;

                var users = await _doctorService.GetPaginatedData(pageNumberValue, pageSizeValue, cancellationToken);
                var response = new ResponseViewModel<PaginatedData<DoctorViewModel>>
                {
                    Success = true,
                    Message = "Doctor retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctors");
                throw;
            }
        }

        [PermissionAuthorize(ResourceConstants.Appointment, PermissionAction.View)]
        [HttpGet("{doctorId}/available-time-slots")]
        public async Task<IActionResult> GetDoctorAvailableTimeSlots(int doctorId, [FromQuery] DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var slots = await _doctorService.GetDoctorAvailableTimeSlots(doctorId, weekStartDate, cancellationToken);
            return Ok(slots);
        }

        [PermissionAuthorize(ResourceConstants.Doctor, PermissionAction.View)]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _doctorService.GetAll(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<DoctorViewModel>>
                {
                    Success = true,
                    Message = "doctors retrieved successfully",
                    Data = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving doctors");
                throw;
            }
        }

        //[PermissionAuthorize(ResourceConstants.Doctor, PermissionAction.View)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var doctorId = User.Claims.FirstOrDefault(c => c.Type == "userRoleId")?.Value;
            if (doctorId == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Doctor ID is null."
                });
            }
            if (!int.TryParse(doctorId, out int parsedDoctorId))
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Invalid Doctor ID format."
                });
            }

            try
            {
                var data = await _doctorService.GetById(parsedDoctorId, cancellationToken);

                var response = new ResponseViewModel<DoctorViewModel>
                {
                    Success = true,
                    Message = "Doctor retrieved successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the doctor with id = {doctorId}");
                throw;
            }
        }

        //impl api update doctor profile
        [PermissionAuthorize(ResourceConstants.Doctor, PermissionAction.Update)]
        [HttpPut("profile")]
        public async Task<IActionResult> Update([FromForm] DoctorUpdateProfileRequestVM model, CancellationToken cancellationToken)
        {
            var doctorId = User.Claims.FirstOrDefault(c => c.Type == "userRoleId")?.Value;
            if (doctorId == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Doctor ID is null."
                });
            }
            if (!int.TryParse(doctorId, out int parsedDoctorId))
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Invalid Doctor ID format."
                });
            }
            try
            {
                DoctorViewModel doctorVM = await _doctorService.UpdateProfile(parsedDoctorId, model, cancellationToken);

                var response = new ResponseViewModel<DoctorViewModel>
                {
                    Success = true,
                    Message = "Updated successfully",
                    Data = doctorVM
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the doctor with id = {doctorId}");
                throw;
            }
        }
    }
}
