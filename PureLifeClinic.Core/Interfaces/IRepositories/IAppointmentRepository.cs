using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<ResponseViewModel<List<Appointment>>> GetAllFilterAppointments(FilterAppointmentRequestViewModel model, CancellationToken cancellationToken);

        Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken);
    }
}
