using PureLifeClinic.Application.BusinessObjects.Schedule.WorkDays;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.BusinessObjects.Schedule.WorkWeeks
{
    public class WorkWeekScheduleViewModel
    {
        public int UserId { get; set; }

        public DateTime WeekStartDate { get; set; }

        public DateTime WeekEndDate { get; set; }


        public WorkWeekStatus WorkWeekStatus { get; set; }
        public List<WorkDayViewModel> WorkDays { get; set; } = new List<WorkDayViewModel>();
    }
}
