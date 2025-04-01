using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Validators
{
    public class DoctorViewModelValidator : AbstractValidator<DoctorUpdateViewModel>
    {
        public DoctorViewModelValidator() { }
    }
}