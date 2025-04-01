using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
    public class InvoiceFileCreateViewModelValidator : AbstractValidator<InvoiceFileCreateViewModel>
    {
        public InvoiceFileCreateViewModelValidator()
        {
            RuleFor(x => x.CLinicLogoUrl)
                .NotEmpty()
                .WithMessage("Clinic logo URL must not be empty.");

            RuleFor(x => x.ClinicInfo)
                .NotNull().WithMessage("Clinic information must not be null.")
                .SetValidator(new InvoiceClinicInfoViewModelValidator());

            RuleFor(x => x.DoctorInfo)
                .NotNull().WithMessage("Doctor information must not be null.")
                .SetValidator(new InvoiceDoctorInfoViewModelValidator());

            RuleFor(x => x.PatientInfo)
                .NotNull().WithMessage("Patient information must not be null.")
                .SetValidator(new InvoicePatientInfoViewModelValidator());

            RuleFor(x => x.Medications)
                .NotNull().WithMessage("Medication list must not be null.")
                .ForEach(x => x.SetValidator(new InvoiceMedicationViewModelValidator()));

            RuleFor(x => x.Services)
                .NotNull().WithMessage("Service list must not be null.")
                .ForEach(x => x.SetValidator(new InvoiceUsedServiceViewModelValidator()));

            RuleFor(x => x.InvoiceBreakdown)
                .NotNull().WithMessage("Invoice breakdown must not be null.")
                .SetValidator(new InvoiceBreakdownViewModelValidator());

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Invalid payment method.");

            RuleFor(x => x.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date must not be empty.");

            RuleFor(x => x.CreateBy)
                .NotEmpty().WithMessage("CreatedBy must not be empty.");
        }
    }
}
