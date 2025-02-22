using FluentValidation;
using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Validations.InputViewModel
{
    public class AppointmentViewModelValidator { }

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

    public sealed class InPersonAppointmentCreateViewModelValidator : AbstractValidator<InPersonAppointmentCreateViewModel>
    {
        public InPersonAppointmentCreateViewModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("PhoneNumber is not valid.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Gender is required.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("DateOfBirth must be in the past.")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(x => x.AppointmentDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("AppointmentDate must be today or in the future.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be a valid number.");
        }
    }
    public sealed class AppointmentUpdateValidator : AbstractValidator<AppointmentUpdateViewModel>
    {
        public AppointmentUpdateValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Appointment date must be in the future.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(255).WithMessage("Reason cannot exceed 255 characters.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).When(x => x.DoctorId.HasValue)
                .WithMessage("DoctorId must be greater than zero.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid appointment status.");
        }
    }

    public sealed class AppointmentStatusUpdateValidator : AbstractValidator<AppointmentStatusUpdateViewModel>
    {
        public AppointmentStatusUpdateValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid appointment status.");
        }
    }
}
