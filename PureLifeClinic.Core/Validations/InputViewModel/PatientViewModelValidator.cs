using FluentValidation;
using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Validations.InputViewModel
{
    public class PatientViewModelValidator { }

    public sealed class PatientCreateViewModelValidator : AbstractValidator<PatientCreateViewModel>
    {
        public PatientCreateViewModelValidator()
        {
        }
    }
}