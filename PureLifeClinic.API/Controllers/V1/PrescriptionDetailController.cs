using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PrescriptionDetailController : ControllerBase
    {
        public PrescriptionDetailController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionDetailCreateViewModel model, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(PrescriptionDetailUpdateViewModel model, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}