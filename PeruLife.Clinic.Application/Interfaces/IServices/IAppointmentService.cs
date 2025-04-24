using PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.Interfaces.IServices
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
        Task<ResponseViewModel<IEnumerable<AppointmentViewModel>>> GetAllFilterAppointments(FilterAppointmentRequestViewModel model, CancellationToken cancellationToken);
        Task<List<AppointmentHealthServiceViewModel>> AssignServiceToAppointment(int appointmentId, List<AppointmentHealthServiceCreateViewModel> model, CancellationToken cancellationToken);
        Task<List<AppointmentHealthServiceViewModel>> GetAppointmentHealthService(int appointmentId, int serviceId, CancellationToken cancellationToken);
        Task DeleteAppointmentHealthService(int appointmentId, List<int> serviceIds, CancellationToken cancellationToken);
        Task UpdateAppointmentHealthService(int appointmentId, AppointmentHealthServiceStatus status, List<int> serviceIds, CancellationToken cancellationToken);
    }
}
