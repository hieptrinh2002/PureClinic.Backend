using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class WorkWeek : Base<int>
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        [Required]
        public DateTime WeekEndDate { get; set; }

        public virtual ICollection<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
    }
}

