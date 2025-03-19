using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class UserConnection: Base<int>
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string ConnectionId { get; set; }  // ID connect SignalR
        public string Device { get; set; } 
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = true;
    }
}
