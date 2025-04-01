using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceUsedServiceViewModelValidator : AbstractValidator<InvoiceUsedServiceViewModel>
    {
        public InvoiceUsedServiceViewModelValidator()
        {
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Service name must not be empty.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Service price must not be negative.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Service quantity must be greater than 0.");
        }
    }
}
