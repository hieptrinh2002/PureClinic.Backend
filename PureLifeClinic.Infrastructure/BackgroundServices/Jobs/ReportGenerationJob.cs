using PureLifeClinic.Application.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Jobs
{
    public class ReportGenerationJob : IReportGenerationJob
    {

        public async Task GenerateMonthlyReportAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GenerateWeeklyReportAsync()
        {
            throw new NotImplementedException();
        }
    }
}
