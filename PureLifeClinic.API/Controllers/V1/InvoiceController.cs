﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class InvoiceController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IInvoiceService _invoiceService;
        private ILogger<InvoiceController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IValidationService _validationService;


        public InvoiceController(
            ICloudinaryService cloudinaryService,
            IInvoiceService invoiceService,
            ILogger<InvoiceController> logger,
            IOptions<AppSettings> appSettings,
            IValidationService validationService)
        {
            _cloudinaryService = cloudinaryService;
            _invoiceService = invoiceService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _validationService = validationService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenarateInvoice(InvoiceFileCreateViewModel model, CancellationToken cancellationToken)
        {
            model.ClinicInfo.PhoneNumber = _appSettings.ClinicInfo.Phone;
            model.ClinicInfo.Address = _appSettings.ClinicInfo.Address;
            model.ClinicInfo.ClinicName = _appSettings.ClinicInfo.Name;

            var file = await _invoiceService.CreateInvoiceFileAsync(model, cancellationToken);
            if (file == null || file?.Data == null)
            {
                _logger.LogError("Error while generating invoice file");
                throw new Exception("Error while generating invoice file");
            }
            string fileName = $"invoice_{model.PatientInfo.PatientId}_{DateTime.UtcNow.Ticks}.pdf";
            var uploadedPath = await _cloudinaryService.UploadStreamFileAsync(file.Data, fileName);
            return Ok(new ResponseViewModel<string>
            {
                Message = "Invoice generated successfully",
                Success = true,
                Data = uploadedPath
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken)
        {
            var validationResult = await _validationService.ValidateAsync(model);

            if (!validationResult.IsValid)
                return new BadRequestObjectResult(ModelStateHelper.GetValidateProblemDetails(validationResult));

            try
            {
                var data = await _invoiceService.Create(model, cancellationToken);

                var response = new ResponseViewModel<InvoiceViewModel>
                {
                    Success = true,
                    Message = "Invoice created successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the invoice");
                throw;
            }
        }
    }
}