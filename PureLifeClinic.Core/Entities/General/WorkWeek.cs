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

    public class WorkDay : Base<int>
    {
        public int WorkWeekId { get; set; }
        [ForeignKey(nameof(WorkWeekId))]
        public virtual WorkWeek WorkWeek { get; set; }

        public DayOfWeek DayOfWeek { get; set; } // weekday (VD: monday...)

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [NotMapped]
        public bool IsAvailable
        {
            get
            {
                var currentTime = DateTime.Now;
                var currentDayOfWeek = currentTime.DayOfWeek;
                var currentTimeOfDay = currentTime.TimeOfDay;

                return currentDayOfWeek == DayOfWeek
                    && currentTimeOfDay >= StartTime
                    && currentTimeOfDay <= EndTime;
            }
        }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}

