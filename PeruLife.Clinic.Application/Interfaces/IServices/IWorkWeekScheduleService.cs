using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.Schedule;
using PureLifeClinic.Application.BusinessObjects.Schedule.WorkWeeks;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IWorkWeekScheduleService
    {
        Task<ResponseViewModel> RegisterWorkScheduleAsync(WorkScheduleRequestViewModel request, CancellationToken cancellationToken);
        Task<ResponseViewModel<WorkWeekScheduleViewModel>> GetWorkSchedule(int userId, DateTime weekStartDate, CancellationToken cancellationToken);
    }
}
