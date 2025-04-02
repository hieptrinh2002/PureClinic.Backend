using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.API.Attributes;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Core.Enums.PermissionEnums;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IBackgroundJobService _backgroundJobService;

        public InvoiceController(
            IInvoiceService invoiceService,
            ILogger<InvoiceController> logger,
            IOptions<AppSettings> appSettings,
            IBackgroundJobService backgroundJobService)
        {
            _invoiceService = invoiceService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _backgroundJobService = backgroundJobService;
        }

        [PermissionAuthorize(ResourceConstants.Invoice, PermissionAction.CreateDelete)]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateInvoice(InvoiceFileCreateViewModel model, CancellationToken cancellationToken)
        {
            model.ClinicInfo.PhoneNumber = _appSettings.ClinicInfo.Phone;
            model.ClinicInfo.Address = _appSettings.ClinicInfo.Address;
            model.ClinicInfo.ClinicName = _appSettings.ClinicInfo.Name;

            var file = await _invoiceService.CreateInvoiceFileAsync(model, cancellationToken);

            if (file == null || file?.Data == null)
            {
                _logger.LogError("Error while generating invoice file");
                throw new ErrorException("Error while generating invoice file");
            }

            _backgroundJobService.ScheduleImmediateJob<IInvoiceService>
                (service => service.ProcessInvoiceAsync(model, file.Data, cancellationToken));

            return File(file.Data, "application/pdf", $"invoice_{model.PatientInfo.PatientId}.pdf");
        }

        [PermissionAuthorize(ResourceConstants.Invoice, PermissionAction.CreateDelete)]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        [HttpPost]
        public async Task<IActionResult> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken)
        {
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