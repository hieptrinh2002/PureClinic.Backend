using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class AppointmentHealthService : Base<int>
    {
        // FK to Appointment
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }

        // FK to HealthService
        public int HealthServiceId { get; set; }
        [ForeignKey(nameof(HealthServiceId))]
        public HealthService HealthService { get; set; }

        public int Quantity { get; set; } = 1;

        // price at the present time
        public double Price { get; set; }
    }
}
    