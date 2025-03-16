namespace PureLifeClinic.Core.Interfaces.IBackgroundJobs
{
    public interface IReportGenerationJob
    {
        Task GenerateMonthlyReportAsync();
    }
}
