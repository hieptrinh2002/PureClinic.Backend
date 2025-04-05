using PureLifeClinic.Core.Enums.Queues;

namespace PureLifeClinic.Core.Entities.General.Queues
{
    public class ConsultationQueue : Base<int>
    {
        public int? PatientId { get; set; } 

        public int? CounterId { get; set; } 

        public required string QueueNumber { get; set; } // Ex: A001

        public QueueStatus Status { get; set; } // Waiting, InProgress, Completed

        public  Patient? Patient { get; set; }

        public  Counter? Counter { get; set; }
    }
}
