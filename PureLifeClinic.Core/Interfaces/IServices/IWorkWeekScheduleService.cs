
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.Business.Schedule;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IWorkWeekScheduleService
    {
        Task<ResponseViewModel> RegisterWorkScheduleAsync(WorkScheduleRequestViewModel request, CancellationToken cancellationToken);
        Task<ResponseViewModel<WorkWeekScheduleViewModel>> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken);
    }
}
