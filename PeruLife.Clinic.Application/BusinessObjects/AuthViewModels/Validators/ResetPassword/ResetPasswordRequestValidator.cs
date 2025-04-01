using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ResetPassword;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Validators.ResetPassword
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(2).WithMessage("Email must be at least 2 characters long.")
                .MaximumLength(20).WithMessage("Email must not exceed 20 characters.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .MaximumLength(50).WithMessage("New password must not exceed 50 characters.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
