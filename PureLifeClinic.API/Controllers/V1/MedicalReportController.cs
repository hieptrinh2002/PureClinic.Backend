using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.MedicalReportViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MedicalReportController : ControllerBase
    {
        private readonly IMedicalReportService _medicalReportService;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<MedicalReportController> _logger;
        private readonly IUserService _userService;

        public MedicalReportController(IMedicalReportService medicalReportService, ILogger<MedicalReportController> logger, IAppointmentService appointmentService, IUserService userService)
        {
            _medicalReportService = medicalReportService;
            _logger = logger;
            _appointmentService = appointmentService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalReportById(int id, CancellationToken cancellationToken)
        {
            var mrp = await _medicalReportService.GetById(id, cancellationToken);
            if (mrp == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<RoleViewModel>
                {
                    Success = false,
                    Message = "Medical report not found",
                    Error = new ErrorViewModel
                    {
                        Code = "NOT_FOUND",
                        Message = "Medical report not found"
                    }
                });
            }
            string message = "";
            try
            {
                var result = await _medicalReportService.GetById(id, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting the medical report by id-{id}");
                message = $"An error occurred while getting the medical report- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<MedicalReportViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "GET_ERROR",
                        Message = message
                    }
                });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetMedicalReportByPatientId(int patientId, CancellationToken cancellationToken)
        {
            string message = "";
            try
            {
                var result = await _medicalReportService.GetByPatientId(patientId, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No data found")
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<RoleViewModel>
                    {
                        Success = false,
                        Message = "Patinent not found",
                        Error = new ErrorViewModel
                        {
                            Code = "NOT_FOUND",
                            Message = "Patinent not found"
                        }
                    });
                }

                _logger.LogError(ex, $"An error occurred while getting the medical report by patient id-{patientId}");
                message = $"An error occurred while getting the medical report- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<MedicalReportViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "GET_ERROR",
                        Message = message
                    }
                });
            }
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
                    var appointment = await _appointmentService.GetById(model.AppointmentId, cancellationToken) ?? throw new NotFoundException("appointment not found");
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
                    _logger.LogError(ex, $"An error occurred while adding the medical report => {ex.Message}");
                    throw;
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
            string message = string.Empty;

            if(!await _medicalReportService.IsExists("Id", model.Id, cancellationToken))
            {
                message = $"Medical report Id - '{model.Id}' not found";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<UserViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "DUPLICATE_CODE",
                        Message = message
                    }
                });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _medicalReportService.UpdateMedicalReportAsync(model, cancellationToken);

                    var response = new ResponseViewModel
                    {
                        Success = true,
                        Message = "Medical report updated successfully"
                    };

                    return Ok(response);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating the medical report");
                    message = $"An error occurred while updating the medical report- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPDATE_ROLE_ERROR",
                            Message = message
                        }
                    });
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
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
    }
}
