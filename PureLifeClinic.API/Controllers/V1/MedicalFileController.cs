using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.API.Controllers.V1
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MedicalFileController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public MedicalFileController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Create([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadResult = await _cloudinaryService.UploadFileAsync(file);
            return Ok(new { url = uploadResult });
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId( int appointmentId, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
