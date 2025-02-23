using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IInvoiceService _invoiceService;
        private ILogger<InvoiceController> _logger;
        private readonly AppSettings _appSettings;

        public InvoiceController(
            ICloudinaryService cloudinaryService, 
            IInvoiceService invoiceService, 
            ILogger<InvoiceController> logger,
            IOptions<AppSettings> appSettings)
        {
            _cloudinaryService = cloudinaryService;
            _invoiceService = invoiceService;
            _logger = logger;
            _appSettings = appSettings.Value; 
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
            return Ok(uploadedPath);
        }
    }
}
