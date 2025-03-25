using Microsoft.Extensions.Logging;

namespace PureLifeClinic.UnitTest
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }
        public EventId EventId { get; set; }
        public object State { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
