using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Response;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceClinicInfoViewModelValidator : AbstractValidator<InvoiceClinicInfoViewModel>
    {
        public InvoiceClinicInfoViewModelValidator()
        {
            RuleFor(x => x.ClinicName)
                .NotEmpty().WithMessage("Clinic name must not be empty.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Clinic address must not be empty.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Clinic phone number must not be empty.");
        }
    }
}
