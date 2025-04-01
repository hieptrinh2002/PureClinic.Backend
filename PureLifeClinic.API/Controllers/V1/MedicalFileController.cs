using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ProductViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MedicalFileController : ControllerBase
    {
        private readonly IFileValidator _fileValidator;
        private readonly ILogger<MedicalFileController> _logger;   
        private readonly IMedicalReportService _medicalReportService;
        private readonly IMedicalFileService _medicalFileService;
        public MedicalFileController(
            IFileValidator fileValidator, 
            ILogger<MedicalFileController> logger, 
            IMedicalReportService medicalReportService, 
            IMedicalFileService medicalFileService)
        {
            _fileValidator = fileValidator;
            _logger = logger;
            _medicalReportService = medicalReportService;
            _medicalFileService = medicalFileService;   
        }

        [HttpPost("upload/medical-report/{medicalReportId}")]
        public async Task<IActionResult> Create(int medicalReportId, [FromForm] MedicalFileCreateViewModel medicalFile, CancellationToken cancellationToken)
        {
            if (await _medicalReportService.GetById(medicalReportId, cancellationToken) == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Medical report is not found"
                });
            }
            var validateResult = _fileValidator.IsValid(medicalFile.File);
            if (!validateResult.isValid)
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = validateResult.errorMessage
                });

            if (ModelState.IsValid)
            {
                string message = string.Empty;
                try
                {
                    var uploadUrl = await _medicalFileService.Create(medicalFile, cancellationToken);
                    return Ok(new ResponseViewModel<MedicalFileViewModel>
                    {
                        Success = false,
                        Message = "Upload medical file successfully",
                        Data = new MedicalFileViewModel {  url = uploadUrl.Data }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while uploading the medical file");
                    message = $"An error occurred while uploading the medical file- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<ProductViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPLOAD_ERROR",
                            Message = message
                        }
                    });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
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

      [HttpPost("muilti-upload/medical-report/{medicalReportId}")]
        public async Task<IActionResult> PostMultipleFile(int medicalReportId, [FromForm] MedicalFileMultiCreateViewModel files, CancellationToken cancellationToken)
        {
            if(files.Files.Count < 1)
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Medical report file is not empty"
                });
            }    
            if (await _medicalReportService.GetById(medicalReportId, cancellationToken) == null)
            {
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "Medical report is not found"
                });
            }

            bool checkValid = files.Files.Where(f => _fileValidator.IsValid(f.FileDetails).isValid == false ).Any();
     
            if (!checkValid)
                return BadRequest(new ResponseViewModel
                {
                    Success = false,
                    Message = "File size exceeds the 4MB limit for images and documents"
                });

            if (ModelState.IsValid)
            {
                string message = string.Empty;
                try
                {
                    var uploadUrls = await _medicalFileService.CreateMultipleAsync(files, cancellationToken);
                    return Ok(new
                    {
                        Success = false,
                        Message = "Upload medical file successfully",
                        Data = uploadUrls
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while uploading the medical file");
                    message = $"An error occurred while uploading the medical file- " + ex.Message;

                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<ProductViewModel>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorViewModel
                        {
                            Code = "UPLOAD_ERROR",
                            Message = message
                        }
                    });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
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

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            return Ok();
        } 
    }
}
