using PureLifeClinic.Application.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Jobs
{
    public class DataCleanupJob : IDataCleanupJob
    {
        public Task CleanupOldRecordsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
