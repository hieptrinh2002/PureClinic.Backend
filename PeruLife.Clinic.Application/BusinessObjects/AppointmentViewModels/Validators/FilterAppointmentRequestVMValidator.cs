using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Validators
{
    public sealed class FilterAppointmentRequestVMValidator : AbstractValidator<FilterAppointmentRequestViewModel>
    {
        public FilterAppointmentRequestVMValidator()
        {
            RuleFor(x => x.Top)
                .GreaterThan(0)
                .WithMessage("Top must be greater than zero.");
            RuleFor(x => x.SortBy)
                .NotEmpty()
                .WithMessage("SortBy is required.");
            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("StartTime must be less than EndTime.");

            RuleFor(x => x.EndTime)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("EndTime must be less than or equal to now.");
            RuleFor(x => x.Status)
              .IsInEnum()
              .WithMessage("Invalid appointment status.");
        }
    }
}
