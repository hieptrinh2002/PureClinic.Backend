using FluentValidation;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;

namespace PureLifeClinic.Application.Validations.InputViewModel
{
    public class PatientViewModelValidator { }

    public sealed class PatientCreateViewModelValidator : AbstractValidator<PatientCreateViewModel>
    {
        public PatientCreateViewModelValidator()
        {
        }
    }
}