using FluentValidation.Results;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T model);
    }
}
