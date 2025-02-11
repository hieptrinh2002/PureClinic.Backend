using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentOfWeek(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken);

        Task<IEnumerable<TimespanWorkDayViewModel>> GetDoctorWorkDaysTimespanOfWeek(int doctorId, DateTime weekStartDate, CancellationToken cancellationToken);

        Task<int> GetMaxAppointmentsPerDay(int doctorId, DateTime workDate);

        Task<int> GetDoctorWorkingHours(int doctorId, DateTime workDate);
    }
}
