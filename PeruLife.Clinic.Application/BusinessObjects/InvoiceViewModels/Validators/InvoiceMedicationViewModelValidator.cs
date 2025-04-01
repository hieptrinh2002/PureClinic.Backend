using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceMedicationViewModelValidator : AbstractValidator<InvoiceMedicationViewModel>
    {
        public InvoiceMedicationViewModelValidator()
        {
            RuleFor(x => x.MedicationName)
                .NotEmpty().WithMessage("Medication name must not be empty.");

            RuleFor(x => x.UsageInstructions)
                .NotEmpty().WithMessage("Usage instructions must not be empty.");

            RuleFor(x => x.Dosage)
                .GreaterThan(0).WithMessage("Dosage must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must not be negative.");
        }
    }
}
