using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IAppointmentService : IBaseService<AppointmentViewModel>
    {
        Task<AppointmentViewModel> Create(AppointmentCreateViewModel model, CancellationToken cancellationToken);
        Task<AppointmentViewModel> CreateInPersonAppointment(InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken);
        Task<bool> IsExists(int doctorId, DateTime date, CancellationToken cancellationToken);
    }
}
