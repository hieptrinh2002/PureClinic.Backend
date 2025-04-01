using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Validators
{
    public sealed class AppointmentUpdateValidator : AbstractValidator<AppointmentUpdateViewModel>
    {
        public AppointmentUpdateValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Appointment date must be in the future.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(255).WithMessage("Reason cannot exceed 255 characters.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).When(x => x.DoctorId.HasValue)
                .WithMessage("DoctorId must be greater than zero.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid appointment status.");
        }
    }
}
