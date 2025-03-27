namespace PureLifeClinic.Application.Interfaces.IBackgroundJobs
{
    public interface IDataCleanupJob
    {
        Task CleanupOldRecordsAsync();
    }
}
