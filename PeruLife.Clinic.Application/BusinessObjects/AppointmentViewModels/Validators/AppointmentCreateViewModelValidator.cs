using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Validators
{
    public sealed class AppointmentCreateViewModelValidator : AbstractValidator<AppointmentCreateViewModel>
    {
        public AppointmentCreateViewModelValidator()
        {
            RuleFor(model => model.AppointmentDate).NotNull().WithMessage("Appointment date is not null")
                   .NotEmpty().WithMessage("Appointment date is not empty")
                   .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Appointment date must be after now")
                   .Must(BeInAllowedTimeRange).WithMessage("Appointment time must be between 07:00-12:00 or 13:00-21:00");
        }
        private bool BeInAllowedTimeRange(DateTime date)
        {
            var time = date.TimeOfDay;
            return time >= TimeSpan.FromHours(7) && time <= TimeSpan.FromHours(12) ||
                   time >= TimeSpan.FromHours(13) && time <= TimeSpan.FromHours(21);
        }
    }
}
