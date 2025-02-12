using FluentValidation;
using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Validations
{
    public class AppointmentViewModelValidator
    {
    }

    public class AppointmentCreateViewModelValidator: AbstractValidator<AppointmentCreateViewModel>
    {
        public AppointmentCreateViewModelValidator()
        {
            RuleFor(model => model.AppointmentDate).NotNull().WithMessage("Appointment date is not null")
                   .NotEmpty().WithMessage("Appointment date is not empty")
                   .GreaterThan(DateTime.Now).WithMessage("Appointment date must be after now")
                   .Must(BeInAllowedTimeRange).WithMessage("Appointment time must be between 07:00-12:00 or 13:00-21:00");
        }
        private bool BeInAllowedTimeRange(DateTime date)
        {
            var time = date.TimeOfDay;
            return (time >= TimeSpan.FromHours(7) && time <= TimeSpan.FromHours(12)) ||
                   (time >= TimeSpan.FromHours(13) && time <= TimeSpan.FromHours(21));
        }
    }
}
