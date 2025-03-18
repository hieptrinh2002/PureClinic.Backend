using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Schedulers
{
    public static class RecurringJobScheduler
    {
        public static void ConfigureJobs(IServiceProvider serviceProvider)
        {
            var recurringJobService = serviceProvider.GetRequiredService<IRecurringJobService>();

            recurringJobService.AddOrUpdate<IAppointmentReminderJob>(
                "daily-appointment-reminder",
                x => x.SendRemindersAsync(24),
                Cron.Daily
            );

            recurringJobService.AddOrUpdate<IReportGenerationJob>(
                "monthly-report-generation",
                x => x.GenerateMonthlyReportAsync(),
                Cron.Weekly
            );
        }
    }
}
