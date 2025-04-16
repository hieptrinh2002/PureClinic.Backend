using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Interfaces.IRepositories.FeedBack;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories.Feedback
{
    public class DoctorFeedbackRepository : BaseRepository<DoctorFeedback>, IDoctorFeedbackRepository
    {
        public DoctorFeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<double> GetAverageRatingByDoctorAsync(int doctorId)
        {
            var value = await _dbContext.DoctorFeedbacks
            .Where(f => f.DoctorId == doctorId).AverageAsync(f => (double?)f.Rating);
            return value ?? 0;  
        }

        public async Task<IEnumerable<DoctorFeedback>> GetByDateRangeAsync(int doctorId, DateTime startDate, DateTime endDate)
        {
            var feedbacks = await _dbContext.DoctorFeedbacks
                .Where(f => f.DoctorId == doctorId && f.EntryDate >= startDate && f.EntryDate <= endDate && f.IsDeleted == false)
                .ToListAsync();

            return feedbacks;
        }

        public async Task<IEnumerable<DoctorFeedback>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken)
        {
            var feedbacks = await _dbContext.DoctorFeedbacks
                .Where(f => f.DoctorId == doctorId && f.IsDeleted == false)
                .ToListAsync(cancellationToken);

            return feedbacks;
        }

        public async Task<IEnumerable<DoctorFeedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken)
        {
            var feedbacks = await _dbContext.DoctorFeedbacks
                .Where(f => f.PatientId == patientId && f.IsDeleted == false)
                .ToListAsync(cancellationToken);

            return feedbacks;
        }

        public async Task<int> GetFeedbackCountByDoctorAsync(int doctorId)
        {
            var count = await _dbContext.DoctorFeedbacks
                .Where(f => f.DoctorId == doctorId && f.IsDeleted == false)
                .CountAsync();
            return count;
        }
    }
}
