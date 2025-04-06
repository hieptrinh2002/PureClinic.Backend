using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<int> CountConsecutiveMissedAppointments(int patientId);
        Task<List<Appointment>> GetAllFilterAppointments(
            DateTime? StartTime,
            string? DoctorId,
            string? PatientId,
            DateTime? EndTime,
            int Top,
            AppointmentStatus Status,
            string SortBy,
            string SortOrder,
            CancellationToken cancellationToken
        );
        Task<List<Appointment>> GetLateAppointments(DateTime now);
        Task<List<Appointment>> GetUpcomingAppointmentsBatchAsync(int pageIndex, int batchSize, int hoursBefore);
        Task<bool> IsExistsAppointment(int doctorId, DateTime date, CancellationToken cancellationToken);
    }
}
