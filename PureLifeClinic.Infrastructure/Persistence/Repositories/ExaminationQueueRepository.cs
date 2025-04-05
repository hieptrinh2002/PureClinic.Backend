using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class ExaminationQueueRepository : BaseRepository<ExaminationQueue>, IExaminationQueueRepository
    {
        public ExaminationQueueRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ExaminationQueue>> GetAllToday(int doctorId, CancellationToken cancellation)
        {
            var key = $"B{DateTime.Today:yyyyMMdd}";
            return await _dbContext.ExaminationQueues
                .Where(x => x.QueueNumber.Contains(key) && x.DoctorId == doctorId)
                .ToListAsync(cancellation);
        }

        public async Task<ExaminationQueue> GetFirstWithQueueNum(string queueNumber, CancellationToken cancellationToken)
        {
            return await _dbContext.ExaminationQueues.Where(x => x.QueueNumber == queueNumber).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
