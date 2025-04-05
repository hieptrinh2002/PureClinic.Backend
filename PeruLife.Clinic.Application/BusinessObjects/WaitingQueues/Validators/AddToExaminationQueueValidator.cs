using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.WaitingQueues.Request;

namespace PureLifeClinic.Application.BusinessObjects.WaitingQueues.Validators
{
    public class AddToExaminationQueueValidator : AbstractValidator<AddToExaminationQueueRequest>
    {
        public AddToExaminationQueueValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than 0.")
                .NotNull().WithMessage("PatientId is not null");


            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("DoctorId must be greater than 0.")
                .NotNull().WithMessage("DoctorId is not null");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Consultation Type is invalid.");
        }
    }
}
