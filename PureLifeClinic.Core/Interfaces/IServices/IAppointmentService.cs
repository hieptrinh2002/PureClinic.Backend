using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IAppointmentService : IBaseService<AppointmentViewModel>
    {
        new Task<IEnumerable<AppointmentViewModel>> GetAll(CancellationToken cancellationToken);
        Task<AppointmentViewModel> Create(AppointmentCreateViewModel model, CancellationToken cancellationToken);
        Task<AppointmentViewModel> CreateInPersonAppointment(InPersonAppointmentCreateViewModel model, CancellationToken cancellationToken);
        Task<bool> IsExists(int doctorId, DateTime date, CancellationToken cancellationToken);
        Task<ResponseViewModel> UpdateAppointmentAsync(int id, AppointmentUpdateViewModel model, CancellationToken cancellationToken);
        Task<ResponseViewModel> UpdateAppointmentStatusAsync(int id, AppointmentStatus status, CancellationToken cancellationToken);
        Task<ResponseViewModel<IEnumerable<DoctorAppointmentViewModel>>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken);
        Task<ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken);
    }
}
