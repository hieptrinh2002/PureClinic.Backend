using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.Schedule.ValidateAttributes
{
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
}
