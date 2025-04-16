using PureLifeClinic.Core.Entities.General.Feedback;

namespace PureLifeClinic.Application.Interfaces.IServices.FeedBack
{
    public interface IFeedbackService<TViewModel>
    {
        Task<IEnumerable<TViewModel>> GetByPatientAsync(int patientId, CancellationToken cancellationToken);
        Task<IEnumerable<TViewModel>> GetByTargetIdAsync(int targetId); // e.g., doctorId or clinicId
        Task<TViewModel> GetSummaryAsync(int targetId, CancellationToken cancellationToken);
        Task<IEnumerable<TViewModel>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Feedback> CreateAsync(object createModel); // dynamic input type
        Task<bool> ReportAsync(int feedbackId);
    }
}
