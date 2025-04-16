using PureLifeClinic.Core.Entities.General.Feedback;

namespace PureLifeClinic.Core.Interfaces.IRepositories.FeedBack
{
    public interface IDoctorFeedbackRepository : IBaseRepository<DoctorFeedback>
    {
        Task<IEnumerable<DoctorFeedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken);
        Task<IEnumerable<DoctorFeedback>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken);
        Task<double> GetAverageRatingByDoctorAsync(int doctorId);
        Task<int> GetFeedbackCountByDoctorAsync(int doctorId);
        Task<IEnumerable<DoctorFeedback>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate);
    }
}
