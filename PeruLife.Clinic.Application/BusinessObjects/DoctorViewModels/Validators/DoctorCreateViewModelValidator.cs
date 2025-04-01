using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Validators
{
    public sealed class DoctorCreateViewModelValidator : AbstractValidator<DoctorCreateViewModel>
    {
        public DoctorCreateViewModelValidator()
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

            // experience years must be greater than 0
            RuleFor(x => x.ExperienceYears)
                .GreaterThan(0).WithMessage("ExperienceYears must be greater than 0.");
        }
    }

}
