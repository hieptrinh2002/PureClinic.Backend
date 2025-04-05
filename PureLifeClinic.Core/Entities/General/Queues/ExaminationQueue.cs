using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.Queues;

namespace PureLifeClinic.Core.Entities.General.Queues
{
    public class ExaminationQueue : Base<int>
    {
        public required int PatientId { get; set; } // FK tới Patient
        public int DoctorId { get; set; } // FK tới Doctor
        public required string QueueNumber { get; set; } // VD: B001
        public required ConsultationType Type { get; set; } // Booking / WalkIn
        public Priority Priority { get; set; } // Mức độ ưu tiên (0 - thấp, 1 - cao,...)
        public QueueStatus Status { get; set; } // Waiting, InProgress, Testing, Completed

        // Navigation properties (nếu dùng Entity Framework)
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
