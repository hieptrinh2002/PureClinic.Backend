using Microsoft.Extensions.Logging;

namespace PureLifeClinic.UnitTest
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<LogEntry> Logs { get; } = new List<LogEntry>();

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Logs.Add(new LogEntry
            {
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                Exception = exception,
                Message = formatter(state, exception)
            });
            throw new NotImplementedException();
        }
    }

}
