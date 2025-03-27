using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.Schedule.ValidateAttributes
{
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
