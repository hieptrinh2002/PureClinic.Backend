using System.Linq.Expressions;

namespace PureLifeClinic.Core.Interfaces.IBackgroundJobs
{
    public interface IBackgroundJobService
    {
        void ScheduleImmediateJob<T>(Expression<Action<T>> methodCall);
        void ScheduleDelayedJob<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    }
}
