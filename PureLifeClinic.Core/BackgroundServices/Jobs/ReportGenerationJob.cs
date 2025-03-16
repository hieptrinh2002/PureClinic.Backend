using PureLifeClinic.Core.Interfaces.IBackgroundJobs;

namespace PureLifeClinic.Core.BackgroundServices.Jobs
{
    public class ReportGenerationJob : IReportGenerationJob
    {
        //private readonly IReportProcessor _reportProcessor;

        //public ReportGenerationJob(IReportProcessor reportProcessor)
        //{
        //    _reportProcessor = reportProcessor;
        //}

        public async Task GenerateMonthlyReportAsync()
        {
            //await _reportProcessor.GenerateReportAsync();
        }
    }
}
