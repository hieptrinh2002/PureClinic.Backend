using System.Linq.Expressions;

namespace PureLifeClinic.Application.Interfaces.IBackgroundJobs
{
    public interface IRecurringJobService
    {
        void AddOrUpdate<T>(string jobId, Expression<Action<T>> methodCall, Func<string> cronExpression);
        void Remove(string jobId);
    }
}
