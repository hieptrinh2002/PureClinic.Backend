using FluentValidation;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels.Validators
{
    public class UserUpdateVMValidator: AbstractValidator<UserUpdateViewModel>
    {
        public UserUpdateVMValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(20).WithMessage("UserName must not exceed 20 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId must be a valid number.");
        }
    }
}
