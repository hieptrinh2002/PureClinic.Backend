using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    [ValidateWeekStartAndEnd]
    public class WorkWeek : Base<int>
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public DateTime WeekStartDate { get; set; }

        [Required]
        public DateTime WeekEndDate { get; set; }
         
        public WorkWeekStatus WorkWeekStatus { get; set; } = WorkWeekStatus.Pending;
        public ICollection<WorkDay> WorkDays { get; set; } = new List<WorkDay>();
    }
 
    public class ValidateWeekStartAndEndAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var workWeek = validationContext.ObjectInstance as WorkWeek;

            if (workWeek == null)
            {
                return new ValidationResult("Invalid object type.");
            }

            // Check if WeekStartDate is Monday
            if (workWeek.WeekStartDate.DayOfWeek != DayOfWeek.Monday)
            {
                return new ValidationResult("WeekStartDate must be a Monday.");
            }
           
            // Check if WeekEndDate is Sunday
            if (workWeek.WeekEndDate.DayOfWeek != DayOfWeek.Sunday)
            {
                return new ValidationResult("WeekEndDate must be a Sunday.");
            }
            var workDays = workWeek.WorkDays;
            if (workDays != null && workDays.Count > 14)
            {
                return new ValidationResult("WorkDays must not exceed 14 sessions per week.");
            }
            return ValidationResult.Success;
        }
    }

    public enum WorkWeekStatus {
        Pending,
        Approved,
        Canceled
    }
}

