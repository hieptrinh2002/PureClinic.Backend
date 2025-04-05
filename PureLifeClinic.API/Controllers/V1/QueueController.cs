using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.WaitingQueues.Request;
using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Core.Enums.Queues;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IWaitingQueueService _queueService;

        public QueueController(IWaitingQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost("in-person/checkin")]
        public async Task<IActionResult> PatientCheckIn(CancellationToken cancellation)
        {
            string queueNumber = await _queueService.CheckInPatient(cancellation);
            return Ok(new ResponseViewModel<string>
            {
                Success = true,
                Message = "Get queue number sucessfully !",
                Data = queueNumber
            });
        }

        [HttpPost("consultation/call-next")]
        public async Task<IActionResult> CallNext(int counterId, CancellationToken cancellation)
        {
            var queueNumber = await _queueService.CallNextConsultationQueueNumber(counterId, cancellation);
            return queueNumber != null ?
                Ok(new ResponseViewModel<string>
                {
                    Success = true,
                    Data = queueNumber,
                    Message = "Called !"
                }) 
                : throw new NotFoundException("Waiting queue is empty now !");
        } 

        [HttpPut("consultation/update-status/{queueNumber}")]
        public async Task<IActionResult> UpdateConsultationQueueStatus(
            [FromRoute] string queueNumber, [FromBody] QueueStatus status, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(QueueStatus), status))
                throw new ErrorException("Invalid QueueStatus value");

            await _queueService.UpdateConsultationQueueStatus(queueNumber, status, cancellationToken);
            return Ok(new ResponseViewModel
            {
                Success = true,
                Message = "Queue status updated successfully."
            });
        }

        [HttpGet("consultation/current")]
        public async Task<IActionResult> GetCurrentConsultationQueue(int pageSize , int pageNumber = 1)
        {
            if(pageNumber < 1 || pageSize < 1) 
                throw new BadRequestException("Invalid pagesize or page number !");
            var queueNumbers = await _queueService.GetCurrentConsultationQueue(pageNumber, pageSize);
            return Ok(new { queueNumbers });
        }

        [HttpPost("examination/add")]
        [ServiceFilter(typeof(ValidateInputViewModelFilter))]
        public async Task<IActionResult> AddToExaminationQueue([FromBody] AddToExaminationQueueRequest request, CancellationToken cancellationToken)
        {
            var queueNumber = await _queueService.AddToExaminationQueue(request.PatientId, request.DoctorId, request.Type, cancellationToken);
            return Ok(new { QueueNumber = queueNumber });
        }

        [HttpGet("examination/doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorExaminationQueue([FromRoute] int doctorId)
        {
            var queueNumbers = await _queueService.GetDoctorExaminationQueue(doctorId);
            return Ok(new { queueNumbers });
        }

        [HttpGet("examination/call-next/{doctorId}")]
        public async Task<IActionResult> CallNextExaminationQueue([FromRoute] int doctorId, CancellationToken cancellationToken)
        {
            var queueNumber = await _queueService.CallNextExaminationQueueNumber(doctorId, cancellationToken);
            if (queueNumber == null)
                return NotFound("No more patients in the queue.");
            return Ok(new { QueueNumber = queueNumber });
        }

        [HttpPut("examination/{queueNumber}/status")]
        public async Task<IActionResult> UpdateStatus(string queueNumber, [FromBody] QueueStatus status, CancellationToken cancellationToken)
        {
            await _queueService.UpdateExaminationQueueStatus(queueNumber, status, cancellationToken);
            return Ok("Update sucessfully.");
        }
    }
}
