using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IWorkWeekScheduleRepository : IBaseRepository<WorkWeek>
    {
        Task<bool> IsWorkScheduleExistsAsync(int userId, DateTime weekStartDate, CancellationToken cancellationToken);

        Task<WorkWeek?> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken);
    }
}
