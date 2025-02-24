using FluentValidation;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Validations.InputViewModel
{
    public class InvoiceViewModelValidator
    {
    }

    public sealed class InvoiceCreateViewModelValidator : AbstractValidator<InvoiceCreateViewModel>
    {
        public InvoiceCreateViewModelValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("AppointmentId is required");
            RuleFor(x => x.TotalAmount)
                .NotEmpty().WithMessage("TotalAmount is required");
            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .WithMessage("PaymentMethod is not valid");
            RuleFor(x => x.IsPaid)
                .NotEmpty().WithMessage("IsPaid is required");
        }
    }

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
        }
    }

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
                    var expectedTotal = (model.MedicationTotal + model.ServiceTotal) - model.DiscountAmount + model.TaxAmount;
                    if (grandTotal != expectedTotal)
                    {
                        context.AddFailure("GrandTotal", "Grand total does not match the calculation: (medicationTotal + serviceTotal) - discountAmount + taxAmount.");
                    }
                });
        }
    }
}
