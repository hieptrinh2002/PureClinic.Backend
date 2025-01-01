using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class DoctorWorkWeek : Base<int>
    {
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        [Required]
        public DateTime WeekEndDate { get; set; }

        public virtual ICollection<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
    }

    public class WorkDay : Base<int>
    {
        public int DoctorWorkWeekId { get; set; }
        [ForeignKey(nameof(DoctorWorkWeekId))]
        public virtual DoctorWorkWeek WorkWeek { get; set; }

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

