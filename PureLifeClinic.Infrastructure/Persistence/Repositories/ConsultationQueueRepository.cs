using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class ConsultationQueueRepository : BaseRepository<ConsultationQueue>, IConsultationQueueRepository
    {
        public ConsultationQueueRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ConsultationQueue>> GetAllToday(CancellationToken cancellation)
        {
            var key = $"A{DateTime.Today:yyyyMMdd}";
            return await _dbContext.ConsultationQueues.Where(x => x.QueueNumber.Contains(key)).ToListAsync();
        }

        public async Task<ConsultationQueue> GetFirstWithQueueNum(string queueNumber)
        {
            return await _dbContext.ConsultationQueues.Where(c => c.QueueNumber == queueNumber).FirstOrDefaultAsync(default);
        }
    }
}
