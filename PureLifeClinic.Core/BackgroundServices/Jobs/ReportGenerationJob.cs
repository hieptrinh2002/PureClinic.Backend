using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Core.BackgroundServices.Jobs
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
