using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Core.BackgroundServices.Jobs
{
    public class DataCleanupJob : IDataCleanupJob
    {
        public Task CleanupOldRecordsAsync()
        {
            throw new NotImplementedException();    
        }
    }
}
