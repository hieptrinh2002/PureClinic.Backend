using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Interfaces.IServices;

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

        [HttpGet("{doctorId}/patients")]
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
                    new ExpressionFilter
                    {
                        PropertyName = "PatientStatus",
                        Value = patientStatus,
                        Comparison = Comparison.Equal
                    },
                    new ExpressionFilter
                    {
                        PropertyName = "User.Name",
                        Value = search,
                        Comparison = Comparison.Contains
                    },
                };

                var patientViewModels = await _doctorService.GetPagtinatedPatientData(doctorId, pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

                var response = new ResponseViewModel<PaginatedDataViewModel<PatientViewModel>>
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

        [HttpGet("paginated")]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, CancellationToken cancellationToken)
        {
            try
            {
                int pageSizeValue = pageSize ?? 10;
                int pageNumberValue = pageNumber ?? 1;

                var users = await _doctorService.GetPaginatedData(pageNumberValue, pageSizeValue, cancellationToken);
                var response = new ResponseViewModel<PaginatedDataViewModel<DoctorViewModel>>
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

        [HttpGet("{doctorId}/available-time-slots")]
        public async Task<IActionResult> GetDoctorAvailableTimeSlots(int doctorId, [FromQuery] DateTime weekStartDate, CancellationToken cancellationToken)
        {
            var slots = await _doctorService.GetDoctorAvailableTimeSlots(doctorId, weekStartDate, cancellationToken);
            return Ok(slots);
        }


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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _doctorService.GetById(id, cancellationToken);

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
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<DoctorViewModel>
                    {
                        Success = false,
                        Message = "Doctor not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Doctor not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while retrieving the user");

                var errorResponse = new ResponseViewModel<DoctorViewModel>
                {
                    Success = false,
                    Message = "Error retrieving Doctor",
                    Error = new ErrorViewModel
                    {
                        Code = "ERROR_CODE",
                        Message = ex.Message
                    }
                };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
