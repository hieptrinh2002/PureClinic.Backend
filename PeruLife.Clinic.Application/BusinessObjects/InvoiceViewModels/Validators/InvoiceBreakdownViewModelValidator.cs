using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceBreakdownViewModelValidator : AbstractValidator<InvoiceBreakdownViewModel>
    {
        public InvoiceBreakdownViewModelValidator()
        {
            RuleFor(x => x.MedicationTotal)
                .GreaterThanOrEqualTo(0).WithMessage("Medication total must not be negative.");

            RuleFor(x => x.ServiceTotal)
                .GreaterThanOrEqualTo(0).WithMessage("Service total must not be negative.");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount amount must not be negative.");

            RuleFor(x => x.TaxAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Tax amount must not be negative.");

            RuleFor(x => x.GrandTotal)
                .GreaterThanOrEqualTo(0).WithMessage("Grand total must not be negative.")
                .Custom((grandTotal, context) =>
                {
                    var model = context.InstanceToValidate;
                    var expectedTotal = model.MedicationTotal + model.ServiceTotal - model.DiscountAmount + model.TaxAmount;
                    if (grandTotal != expectedTotal)
                    {
                        context.AddFailure("GrandTotal", "Grand total does not match the calculation: (medicationTotal + serviceTotal) - discountAmount + taxAmount.");
                    }
                });
        }
    }
}
