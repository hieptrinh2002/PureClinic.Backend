using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General.Notification
{
    public class Announcement : Base<int>
    {
        public Announcement()
        {
            AnnouncementUsers = new List<AnnouncementUser>();
        }
        public Announcement(string title, string content, int userId, Status status)
        {
            Title = title;
            Content = content;
            UserId = userId;
            Status = status;
        }

        [Required]
        [MaxLength(250)]
        public string Title { set; get; }

        [MaxLength(250)]
        public string Content { set; get; }

        public int UserId { set; get; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public ICollection<AnnouncementUser> AnnouncementUsers { get; set; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}
