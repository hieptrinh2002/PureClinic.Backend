namespace PureLifeClinic.Core.Interfaces.IBackgroundJobs
{
    public interface IDataCleanupJob
    {
        Task CleanupOldRecordsAsync();
    }
}
