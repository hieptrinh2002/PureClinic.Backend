using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;

public class GenerateTokenValidator : AbstractValidator<GenerateTokenViewModel>
{
    public GenerateTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("AccessToken is required.")
            .MinimumLength(10).WithMessage("AccessToken must be at least 10 characters long.")
            .MaximumLength(500).WithMessage("AccessToken must not exceed 500 characters.");

        RuleFor(x => x.RefreshToken)
            .NotNull().WithMessage("RefreshToken is required.");

        RuleFor(x => x.AccessTokenId)
            .NotEmpty().WithMessage("AccessTokenId is required.");

        RuleFor(x => x.CreateOn)
            .LessThan(x => x.ExpireOn)
            .WithMessage("Creation date must be before expiration date.");

        RuleFor(x => x.ExpireOn)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}
