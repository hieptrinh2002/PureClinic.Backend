namespace PureLifeClinic.Application.Interfaces.IBackgroundJobs
{
    public interface IAppointmentReminderJob
    {
        Task SendRemindersAsync(int hoursBefore);
    }
}
