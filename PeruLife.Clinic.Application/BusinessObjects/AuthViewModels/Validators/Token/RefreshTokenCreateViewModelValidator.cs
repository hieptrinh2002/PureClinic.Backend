using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token;

public class RefreshTokenCreateValidator : AbstractValidator<RefreshTokenCreateViewModel>
{
    public RefreshTokenCreateValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.")
            .MinimumLength(2).WithMessage("Token must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Token must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Token must contain only alphanumeric characters, underscores, or hyphens.");

        RuleFor(x => x.AccessTokenId)
            .Must(token => string.IsNullOrEmpty(token) || token.Length == 36)
            .WithMessage("AccessTokenId must be a valid GUID format (36 characters) if provided.");

        RuleFor(x => x.ExpireOn)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.");

        RuleFor(x => x.RevokedOn)
            .GreaterThan(x => x.CreateOn).When(x => x.RevokedOn.HasValue)
            .WithMessage("Revoked date must be after the creation date.");

        RuleFor(x => x.CreateOn)
            .LessThan(x => x.ExpireOn).WithMessage("Creation date must be before expiration date.");
    }
}
