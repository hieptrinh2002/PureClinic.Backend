using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Entities.General.Queues
{
    public class ConsultationQueue : Base<int>
    {
        public int? PatientId { get; set; } 

        public int? CounterId { get; set; } 

        public required string QueueNumber { get; set; } // Ex: A001

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public QueueStatus Status { get; set; } // Waiting, InProgress, Completed

        public  Patient? Patient { get; set; }

        public  Counter Counter { get; set; }
    }
}
