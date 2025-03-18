using Hangfire;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.BackgroundServices
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BackgroundJobService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void ScheduleImmediateJob<T>(Expression<Action<T>> methodCall)
        {
            _backgroundJobClient.Enqueue(methodCall);
        }

        public void ScheduleDelayedJob<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            _backgroundJobClient.Schedule(methodCall, delay);
        }
    }
}
