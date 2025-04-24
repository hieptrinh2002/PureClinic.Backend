using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAppointmentHealthServiceRepository : IBaseRepository<AppointmentHealthService>
    {
        Task<List<AppointmentHealthService>> GetAllByAppointmentId(int appointmentId, CancellationToken cancellationToken);
    }
}
