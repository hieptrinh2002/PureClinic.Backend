namespace PureLifeClinic.Core.Interfaces.IBackgroundJobs
{
    public interface IAppointmentReminderJob
    {
        Task SendRemindersAsync();
    }
}
