using Microsoft.AspNetCore.Mvc;
using PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices.FeedBack;
using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Exceptions;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IDoctorFeedbackService _feedbackService;
        public FeedbackController(IDoctorFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Retrieves all feedbacks for a specific patient.
        /// </summary>
        /// <param name="patientId">Patient's unique identifier.</param>
        /// <returns>List of feedbacks related to the patient.</returns>r
        [HttpGet("patients/{patientId}/feedbacks")]
        public async Task<IActionResult> GetFeedbacksByPatient(int patientId, CancellationToken cancellationToken)
        {
            return Ok(new ResponseViewModel<List<DoctorFeedbackViewModel>>
            {
                Success = true,
                Message = "Feedbacks retrieved successfully",
                Data = (List<DoctorFeedbackViewModel>)await _feedbackService.GetFeedbacksByPatientAsync(patientId, cancellationToken)
            });
        }

        /// <summary>
        /// Retrieves all feedbacks for a specific doctor.
        /// </summary>
        /// <param name="doctorId">Doctor's unique identifier.</param>
        /// <returns>List of feedbacks related to the doctor.</returns>
        [HttpGet("doctors/{doctorId}/feedbacks")]
        public async Task<IActionResult> GetFeedbacksByDoctor(int doctorId, CancellationToken cancellationToken)
        {
            return Ok(new ResponseViewModel<List<DoctorFeedbackViewModel>>
            {
                Success = true,
                Message = "Feedbacks retrieved successfully",
                Data = (List<DoctorFeedbackViewModel>)await _feedbackService.GetFeedbacksByDoctorAsync(doctorId, cancellationToken)
            });
        }

        /// <summary>
        /// Retrieves a summary of feedbacks for a specific doctor.
        /// </summary>
        /// <param name="doctorId">Doctor's unique identifier.</param>
        /// <returns>Aggregated feedback data including ratings and counts.</returns>
        [HttpGet("doctors/{doctorId}/feedback-summary")]
        public async Task<IActionResult> GetDoctorFeedbackSummary(int doctorId, CancellationToken cancellationToken)
        {
            return Ok(new ResponseViewModel<DoctorFeedbackSummaryViewModel>
            {
                Success = true,
                Message = "Feedbacks retrieved successfully",
                Data = await _feedbackService.GetDoctorFeedbackSummaryAsync(doctorId, cancellationToken)
            });
        }

        /// <summary>
        /// Retrieves feedbacks within a specified date range.
        /// </summary>
        /// <param name="startDate">Start date for filtering feedbacks.</param>
        /// <param name="endDate">End date for filtering feedbacks.</param>
        /// <returns>List of feedbacks within the date range.</returns>
        [HttpGet("date-range")]
        public async Task<IActionResult> GetFeedbacksByDateRange(int doctorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(new ResponseViewModel<List<DoctorFeedbackViewModel>>
            {
                Success = true,
                Message = "Feedbacks retrieved successfully",
                Data = (List<DoctorFeedbackViewModel>)await _feedbackService.GetDoctorFeedbacksByDateRangeAsync(doctorId, startDate, endDate)
            });
        }

        /// <summary>
        /// Creates a new feedback entry.
        /// </summary>
        /// <param name="feedback">Feedback object containing details.</param>
        /// <returns>Response indicating the result of the operation.</returns>
        [HttpPost]
        public IActionResult CreateFeedback([FromBody] Feedback feedback)
        {
            return Ok();
        }

        /// <summary>
        /// Reports a specific feedback as inappropriate.
        /// </summary>
        /// <param name="id">Feedback's unique identifier.</param>
        /// <returns>Response indicating the result of the report action.</returns>
        [HttpPost("{id}/report")]
        public async Task<IActionResult> ReportFeedback(int feedbackId)
        {
            if (!await _feedbackService.ReportFeedbackAsync(feedbackId))
                throw new BadRequestException("Failed to report feedback.");

            return Ok(new ResponseViewModel
            {
                Success = true,
                Message = "report successfully",
            });
        }
    }
}
