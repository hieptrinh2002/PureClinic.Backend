namespace PureLifeClinic.Core.Entities.General.Queues
{
    public class Counter : Base<int>
    {
        public string Name { get; set; } = string.Empty;
        public int CurrentQueueNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
