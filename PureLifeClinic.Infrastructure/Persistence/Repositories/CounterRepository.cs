using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class CounterRepository : BaseRepository<Counter>, ICounterRepository
    {
        public CounterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
