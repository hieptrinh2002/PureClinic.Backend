using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.MedicineViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.MedicineViewModels.Validators
{
    public class MedicineUpdateValidator : AbstractValidator<MedicineUpdateViewModel>
    {
        public MedicineUpdateValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Medicine Code is required.")
                .Length(2, 8).WithMessage("Medicine Code must be between 2 and 8 characters.")
                .Matches("^[A-Z0-9_]+$").WithMessage("Medicine Code can only contain uppercase letters, numbers, and underscores.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Medicine Name is required.")
                .Length(2, 100).WithMessage("Medicine Name must be between 2 and 100 characters.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .LessThanOrEqualTo(float.MaxValue).WithMessage("Price exceeds the maximum value.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");

            RuleFor(x => x.Description)
                .MaximumLength(350).WithMessage("Description must not exceed 350 characters.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive status must be specified.");
        }
    }
}