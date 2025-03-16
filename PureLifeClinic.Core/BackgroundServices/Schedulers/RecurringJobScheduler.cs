using Hangfire;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Core.BackgroundServices.Schedulers
{
    public static class RecurringJobScheduler
    {
        public static void ConfigureJobs()
        {
            RecurringJob.AddOrUpdate<IAppointmentReminderJob>("appointment-reminder", job => job.SendRemindersAsync(), Cron.Daily);
            //RecurringJob.AddOrUpdate<IDataCleanupJob>("data-cleanup", job => job.CleanupOldRecordsAsync(), Cron.Monthly);
            RecurringJob.AddOrUpdate<IReportGenerationJob>("report-generation", job => job.GenerateMonthlyReportAsync(), Cron.Monthly);
        }
    }
}
