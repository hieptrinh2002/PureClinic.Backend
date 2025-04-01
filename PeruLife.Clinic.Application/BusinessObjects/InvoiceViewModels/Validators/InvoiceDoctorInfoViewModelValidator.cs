using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceDoctorInfoViewModelValidator : AbstractValidator<InvoiceDoctorInfoViewModel>
    {
        public InvoiceDoctorInfoViewModelValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID must not be empty.");

            RuleFor(x => x.DoctorName)
                .NotEmpty().WithMessage("Doctor name must not be empty.");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization must not be empty.");
        }
    }
}
