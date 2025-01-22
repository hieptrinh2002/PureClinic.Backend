using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MedicalReportController : ControllerBase
    {
        private readonly IMedicalReportService _medicalReportService;
        private readonly ILogger<MedicalReportController> _logger;  
        public MedicalReportController(IMedicalReportService medicalReportService, ILogger<MedicalReportController> logger)
        {
            _medicalReportService = medicalReportService;
            _logger = logger;
        }

        [HttpGet("paginated-data")]
        public async Task<IActionResult> GetPaginated(int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalReportById(int id, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetMedicalReportByPatientId(string patientId, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicalReportCreateViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                try
                {
                    // check apointment is exists
                    // medicineId is valid

                    var data = await _medicalReportService.Create(model, cancellationToken);
                    var response = new ResponseViewModel<MedicalReportViewModel>
                    {
                        Success = true,
                        Message = "medical report created successfully",
                        Data = data
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while adding the medical report");
                    message = $"An error occurred while adding the medical report- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<MedicalReportViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "ADD_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<MedicalReportViewModel>
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorViewModel
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        [HttpPut]
        public async Task<IActionResult> Edit(MedicalReportUpdateViewModel model, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
