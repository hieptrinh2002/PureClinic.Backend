using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Validators.ForgotPassword
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestViewModel>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is no empty.")
                .EmailAddress().WithMessage("invalid email.");

            RuleFor(x => x.ClientUrl)
                .NotEmpty().WithMessage("Client URL can't be empty.");
        }
    }
}
