using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.CounterViewModels.Response
{
    public class CounterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CurrentQueueNumber { get; set; } = null;
        public bool IsActive { get; set; }
        public CounterType CounterType { get; set; }
    }
}
