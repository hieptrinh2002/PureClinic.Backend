using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Validators
{
    public sealed class DoctorUpdateViewModelValidator : AbstractValidator<DoctorUpdateViewModel>
    {
        public DoctorUpdateViewModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .Length(2, 100).WithMessage("Full Name must be between 2 and 100 characters");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required")
                .Length(2, 20).WithMessage("User Name must be between 2 and 20 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required")
                .MaximumLength(100).WithMessage("Specialty must not exceed 100 characters");

            RuleFor(x => x.Qualification)
                .MaximumLength(500).WithMessage("Qualification must not exceed 500 characters");

            RuleFor(x => x.ExperienceYears)
                .NotNull().WithMessage("Experience Years is required")
                .GreaterThanOrEqualTo(0).WithMessage("Experience Years must be a positive number");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.RegistrationNumber)
                .NotEmpty().WithMessage("Registration Number is required")
                .MaximumLength(50).WithMessage("Registration Number must not exceed 50 characters");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required");
        }
    }
}
