using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    public class AppointmentHealthServiceRepository : BaseRepository<AppointmentHealthService>, IAppointmentHealthServiceRepository
    {
        public AppointmentHealthServiceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        Task<List<AppointmentHealthService>> IAppointmentHealthServiceRepository.GetAllByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            return _dbContext.AppointmentHealthServices
                .Where(x => x.AppointmentId == appointmentId)
                .Include(x => x.HealthService)
                .Include(x => x.Room)
                .ToListAsync(cancellationToken);
        }
    }
}
