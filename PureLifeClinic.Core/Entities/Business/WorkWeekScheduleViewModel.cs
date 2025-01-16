using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class WorkWeekScheduleViewModel
    {
        public int UserId { get; set; }

        public DateTime WeekStartDate { get; set; }

        public DateTime WeekEndDate { get; set; }

        public List<WorkDayViewModel> WorkDays { get; set; } = new List<WorkDayViewModel>();
    }

    public class WorkDayViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }
    public class WorkScheduleRequestViewModel
    {
        public int UserId { get; set; }

        [Required]
        [WeekStartDateMustBeMonday]
        public DateTime WeekStartDate { get; set; }

        [Required]
        [WeekEndDateMustBeSunday]
        public DateTime WeekEndDate { get; set; }

        [Required]
        [MaxLength(14, ErrorMessage = "WorkDays cannot exceed 14 sessions per week.")]
        public List<WorkDayRequestViewModel> WorkDays { get; set; } = new List<WorkDayRequestViewModel>();
    }

    public class WorkDayRequestViewModel
    {
        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }
    }

    public class WeekEndDateMustBeSundayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime weekEndDate && weekEndDate.DayOfWeek != DayOfWeek.Sunday)
            {
                return new ValidationResult("WeekEndDate must be a Sunday.");
            }

            return ValidationResult.Success;
        }
    }
    public class WeekStartDateMustBeMondayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime weekStartDate && weekStartDate.DayOfWeek != DayOfWeek.Monday)
            {
                return new ValidationResult("WeekStartDate must be a Monday.");
            }

            return ValidationResult.Success;
        }
    }
}
