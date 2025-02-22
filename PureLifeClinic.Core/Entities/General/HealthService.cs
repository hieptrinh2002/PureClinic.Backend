namespace PureLifeClinic.Core.Entities.General
{
    public class HealthService: Base<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HierarchyPath { get; set; }   
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AppointmentHealthService>? HealthServices { get; set; } = new List<AppointmentHealthService>();
    }
}
