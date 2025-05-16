using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Validators
{
    public sealed class InPersonAppointmentCreateVMValidator : AbstractValidator<InPersonAppointmentCreateViewModel>
    {
        public InPersonAppointmentCreateVMValidator()
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
}
