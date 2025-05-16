using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels.Validators
{
    public sealed class UserCreateVMValidator: AbstractValidator<UserCreateViewModel>
    {
        public UserCreateVMValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .NotNull().WithMessage("UserName cannot be null.")
                .Length(2, 20).WithMessage("UserName must be between 2 and 20 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.")
                .Length(6, 50).WithMessage("Password must be between 6 and 50 characters.");

            RuleFor(x => x.RoleId).NotNull().WithMessage("RoleId is required.");
        }
    }
}
