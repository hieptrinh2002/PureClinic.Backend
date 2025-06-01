using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.BusinessObjects.LabResultViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.API.Controllers.V1.Patients
{
    [Route("api/patients/{patientId}/labresults")]
    [ApiController]

    public class LabResultsController : ControllerBase
    {
        private readonly ILabResultService _labResultService;

        public LabResultsController(ILabResultService service)
        {
            _labResultService = service;
        }

        // GET /api/patients/{patientId}/labresults?testType=...&status=...&from=...&to=...
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int patientId,
            [FromQuery] string? testType,
            [FromQuery] LabTestStatus? status,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var results = await _labResultService.FilterAsync(patientId, testType, status, from, to);
            return Ok( new ResponseViewModel<List<LabResult>> {
                Success = true,
                Message = "Lab results retrieved successfully",
                Data = results.ToList() 
            });
        }

        // GET /api/patients/{patientId}/labresults/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int patientId, int id)
        {
            var result = await _labResultService.GetByIdAsync(patientId, id);
            return result == null ? NotFound() : Ok(new ResponseViewModel<LabResult>
            {
                Success = true,
                Message = "Lab result retrieved successfully",  
            });
        }

        // PUT /api/patients/{patientId}/labresults/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int patientId, int id, [FromBody] LabResultUpdateViewModel dto)
        {
            var success = await _labResultService.UpdateAsync(patientId, id, dto);
            return success ? NoContent() : NotFound();
        }
    }
}
