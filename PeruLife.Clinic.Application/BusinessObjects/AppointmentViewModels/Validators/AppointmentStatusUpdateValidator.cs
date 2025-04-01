using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Validators
{
    public sealed class AppointmentStatusUpdateValidator : AbstractValidator<AppointmentStatusUpdateViewModel>
    {
        public AppointmentStatusUpdateValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid appointment status.");
        }
    }
}
