using PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Request;
using PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Response;
using PureLifeClinic.Core.Entities.General.Feedback;

namespace PureLifeClinic.Application.Interfaces.IServices.FeedBack
{
    public interface IDoctorFeedbackService : IBaseService<DoctorFeedbackViewModel>
    {
        Task<Feedback> CreateFeedbackAsync(FeedbackDoctorCreateViewModel feedback, CancellationToken cancellationToken);
        Task<DoctorFeedbackSummaryViewModel> GetDoctorFeedbackSummaryAsync(int doctorId, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorFeedbackViewModel>> GetDoctorFeedbacksByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate);
        Task<bool> ReportFeedbackAsync(int feedbackId);
        Task<IEnumerable<DoctorFeedbackViewModel>> GetFeedbacksByPatientAsync(int patientId, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorFeedbackViewModel>> GetFeedbacksByDoctorAsync(int patientId, CancellationToken cancellationToken);
    }
}
