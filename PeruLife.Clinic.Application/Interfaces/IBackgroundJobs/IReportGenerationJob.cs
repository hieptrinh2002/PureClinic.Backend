namespace PureLifeClinic.Application.Interfaces.IBackgroundJobs
{
    public interface IReportGenerationJob
    {
        Task GenerateMonthlyReportAsync();
        Task GenerateWeeklyReportAsync();
    }
}
