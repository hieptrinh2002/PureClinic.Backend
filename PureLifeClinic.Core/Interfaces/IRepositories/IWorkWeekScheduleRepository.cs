using PureLifeClinic.Core.Entities.Business.Schedule;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IWorkWeekScheduleRepository : IBaseRepository<WorkWeek>
    {
        Task<bool> IsWorkScheduleExistsAsync(int userId, DateTime weekStartDate, CancellationToken cancellationToken);
        Task<WorkWeekScheduleViewModel?> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken);
    }
}
