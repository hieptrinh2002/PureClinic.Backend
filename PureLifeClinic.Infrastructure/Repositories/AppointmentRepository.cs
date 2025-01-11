using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken)
        {
            return await _dbContext.Appointments.AsNoTracking().Where(a => a.DoctorId == doctorId && a.AppointmentDate == date).AnyAsync(cancellationToken);
        }
    }
}
