using Hangfire;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Core.BackgroundServices.Schedulers
{
    public static class DelayedJobScheduler
    {
        public static void ScheduleEmailConfirmationJob()
        {
            //BackgroundJob.Schedule<IEmailConfirmationJob>(job => job.SendConfirmationEmailsAsync(), TimeSpan.FromMinutes(30));
            //BackgroundJob.Enqueue<IEmailConfirmationJob>(job => job.SendConfirmationEmailsAsync());
        }
    }
}
