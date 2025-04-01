using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoicePatientInfoViewModelValidator : AbstractValidator<InvoicePatientInfoViewModel>
    {
        public InvoicePatientInfoViewModelValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient ID must not be empty.");

            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name must not be empty.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth must not be empty.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender must not be empty.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address must not be empty.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number must not be empty.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must not be empty.");
        }
    }
}
