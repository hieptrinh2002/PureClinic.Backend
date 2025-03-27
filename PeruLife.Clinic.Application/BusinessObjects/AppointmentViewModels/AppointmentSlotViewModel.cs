namespace PureLifeClinic.Application.BussinessObjects.AppointmentViewModels
{
    public class AppointmentSlotViewModel
    {
        public DateTime WeekDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
