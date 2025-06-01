using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Controllers.V1.Patients
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PatientProfileController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;

        public PatientProfileController(ILogger<DoctorController> logger, IUserService userService, IPatientService patientService)
        {
            _logger = logger;
            _userService = userService;
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userService.GetAllPatient(cancellationToken);

                var response = new ResponseViewModel<IEnumerable<PatientViewModel>>
                {
                    Success = true,
                    Message = "Patients retrieved successfully",
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
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var patient = await _patientService.GetByIdAsync(id, cancellationToken);
            return Ok(new ResponseViewModel<PatientViewModel>
            {
                Message = "Patient retrieved successfully",
                Success = true,
                Data = patient
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateViewModel model, CancellationToken cancellationToken)
        {
            Patient patient = await _patientService.CreateAsync(model, cancellationToken);
            return Ok(new ResponseViewModel<Patient>
            {
                Message = "Patient retrieved successfully",
                Success = true,
                Data = patient
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateViewModel model, CancellationToken cancellationToken)
        {
            var updated = await _patientService.UpdateAsync(id, model, cancellationToken);
            return Ok(new ResponseViewModel
            {
                Message = "Patient updated successfully",
                Success = true,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _patientService.DeleteAsync(id, cancellationToken);
            return Ok(new ResponseViewModel
            {
                Message = "Patient deleted successfully",
                Success = true,
            });
        }
    }
}
