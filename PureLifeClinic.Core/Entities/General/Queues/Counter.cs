using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Entities.General.Queues
{
    public class Counter : Base<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? CurrentQueueNumber { get; set; }
        public bool IsActive { get; set; }
        public CounterType CounterType { get; set; }
    }
}
