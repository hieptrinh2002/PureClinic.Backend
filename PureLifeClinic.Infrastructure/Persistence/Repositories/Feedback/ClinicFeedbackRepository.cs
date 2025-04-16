using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Interfaces.IRepositories.FeedBack;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories.Feedback
{
    public class ClinicFeedbackRepository : BaseRepository<ClinicFeedBack>, IClinicFeedbackRepository
    {
        public ClinicFeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
