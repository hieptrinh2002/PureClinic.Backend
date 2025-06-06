﻿using Hangfire;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.BackgroundServices
{
    public class RecurringJobService : IRecurringJobService
    {
        public void AddOrUpdate<T>(string jobId, Expression<Action<T>> methodCall, Func<string> cronExpression)
        {
            RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
        }

        public void Remove(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }
    }
}
