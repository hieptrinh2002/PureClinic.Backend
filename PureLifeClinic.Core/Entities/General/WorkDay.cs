using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class WorkDay : Base<int>
    {
        public int WorkWeekId { get; set; }
        [ForeignKey(nameof(WorkWeekId))]
        public WorkWeek WorkWeek { get; set; }

        public DateTime Date { get; set; }

        public DayOfWeek DayOfWeek { get; set; } // weekday (VD: monday...)

        [Required]
        [ValidateStartTimeAndEndTime]
        public TimeSpan StartTime { get; set; }

        [Required]
        [ValidateStartTimeAndEndTime]
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

    public class ValidateStartTimeAndEndTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var time = (TimeSpan)value;

            // [8AM - 12PM] and [1PM - 10PM] is valid time
            var morningStart = new TimeSpan(8, 0, 0);  // 8 AM
            var morningEnd = new TimeSpan(12, 0, 0);   // 12 PM
            var afternoonStart = new TimeSpan(13, 0, 0); // 1 PM
            var afternoonEnd = new TimeSpan(21, 0, 0);  // 9 PM

            if ((time >= morningStart && time <= morningEnd) || (time >= afternoonStart && time <= afternoonEnd))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("StartTime and EndTime must be within [8 AM - 12 PM] or [1 PM - 9 PM].");
        }
    }
}
