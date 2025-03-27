using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.Schedule.WorkDays
{
    public class WorkDayRequestViewModel
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }
}
