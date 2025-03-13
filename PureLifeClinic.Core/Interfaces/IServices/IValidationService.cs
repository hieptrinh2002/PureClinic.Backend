using FluentValidation.Results;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync(object model);
    }
}
