using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business.Schedule
{
    public class WorkWeekScheduleViewModel
    {
        public int UserId { get; set; }

        public DateTime WeekStartDate { get; set; }

        public DateTime WeekEndDate { get; set; }

        public List<WorkDayViewModel> WorkDays { get; set; } = new List<WorkDayViewModel>();
    }

    public class WorkDayViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }
}
