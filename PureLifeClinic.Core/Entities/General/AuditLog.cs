using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public required string Action { get; set; } // e.g., "CheckIn", "CallNumber"
        public required string EntityName { get; set; } // e.g., "Ticket"
        public string? EntityId { get; set; }   // e.g., "A001"
        public required string? PerformedBy { get; set; } // username or userId
        public DateTime Timestamp { get; set; }
        public required string Details { get; set; } // optional JSON of old/new data
        public required string? IPAddress { get; set; }
    }
}
