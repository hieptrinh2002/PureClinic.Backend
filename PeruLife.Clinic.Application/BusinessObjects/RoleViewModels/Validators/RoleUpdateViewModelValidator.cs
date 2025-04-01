using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.RoleViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.RoleViewModels.Validators
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateViewModel>
    {
        public RoleUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Role Code is required.")
                .MinimumLength(2).WithMessage("Role Code must be at least 2 characters long.")
                .MaximumLength(10).WithMessage("Role Code must not exceed 10 characters.")
                .Matches("^[A-Z0-9_]+$").WithMessage("Role Code can only contain uppercase letters, numbers, and underscores.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role Name is required.")
                .MinimumLength(2).WithMessage("Role Name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Role Name must not exceed 100 characters.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive status must be specified.");
        }
    }
}
