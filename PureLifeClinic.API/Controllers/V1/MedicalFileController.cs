using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
                throw new NotFoundException("Medical report is not found");

            var validateResult = _fileValidator.IsValid(medicalFile.File);
            if (!validateResult.isValid)
                throw new BadRequestException(validateResult.errorMessage);

            try
            {
                var uploadUrl = await _medicalFileService.Create(medicalFile, cancellationToken);
                return Ok(new ResponseViewModel<MedicalFileViewModel>
                {
                    Success = false,
                    Message = "Upload medical file successfully",
                    Data = new MedicalFileViewModel { url = uploadUrl.Data }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while uploading the medical file");
                throw;
            }
        }

        [HttpPost("muilti-upload/medical-report/{medicalReportId}")]
        public async Task<IActionResult> PostMultipleFile(int medicalReportId, [FromForm] MedicalFileMultiCreateViewModel files, CancellationToken cancellationToken)
        {
            if (files.Files.Count < 1)
                throw new BadHttpRequestException("Medical report file is not empty");

            if (await _medicalReportService.GetById(medicalReportId, cancellationToken) == null)
                throw new BadHttpRequestException("Medical report is not found");

            bool checkValid = files.Files.Where(f => _fileValidator.IsValid(f.FileDetails).isValid == false).Any();

            if (!checkValid)
                throw new BadHttpRequestException("File size exceeds the 4MB limit for images and documents");

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
                throw;
            }
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            return Ok();
        } 
    }
}
