namespace PureLifeClinic.Application.BusinessObjects.Schedule.WorkDays
{
    public class WorkDayViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }
}
