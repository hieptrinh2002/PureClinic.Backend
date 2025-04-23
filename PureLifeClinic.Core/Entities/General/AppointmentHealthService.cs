using PureLifeClinic.Core.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class AppointmentHealthService : Base<int>, ISortable
    {
        // FK to Appointment
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; }

        // FK to HealthService
        public int HealthServiceId { get; set; }
        [ForeignKey(nameof(HealthServiceId))]
        public HealthService HealthService { get; set; }

        // FK to HealthService
        public int RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }

        public AppointmentHealthServiceStatus Status { get; set; } = AppointmentHealthServiceStatus.NotStarted;

        public ICollection<MedicalFile> MedicalFiles { get; set; } = new List<MedicalFile>();

        // price at the present time
        public double Price { get; set; }
        public int SortOrder {get; set; } = 0;  
    }

    public enum AppointmentHealthServiceStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }
}
    