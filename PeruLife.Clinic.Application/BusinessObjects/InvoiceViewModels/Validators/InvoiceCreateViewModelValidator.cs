using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Validators
{
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
}
