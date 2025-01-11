using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken);
    }
}
