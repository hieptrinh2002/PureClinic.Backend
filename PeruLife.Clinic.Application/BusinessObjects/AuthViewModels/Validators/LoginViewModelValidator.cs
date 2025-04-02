using FluentValidation;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Validators
{
    public sealed class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .NotNull().WithMessage("UserName is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(2).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
        }
    }
}