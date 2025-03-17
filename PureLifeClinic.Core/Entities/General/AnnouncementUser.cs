using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class AnnouncementUser: Base<int>
    {
        public int AnnouncementId { get; set; }

        public int UserId { get; set; }

        public bool? HasRead { get; set; }

        [ForeignKey(nameof(AnnouncementId))]
        public Announcement Announcement { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
