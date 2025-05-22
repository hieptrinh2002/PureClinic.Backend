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
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\@\$\!\%\*\?\&\#\^\.\-_]+").WithMessage("Password must contain at least one special character (e.g., @, $, !, %, *, ?, &, #).")
                .Matches(@"^(?!.*(?:hospital|admin|123456|qwerty)).*$").WithMessage("Password cannot contain common weak words like 'hospital', 'admin', etc.")
                .Must(p => !p.ToLower().Contains("password")).WithMessage("Password cannot contain the word 'password'.");
        }
    }
}