namespace PureLifeClinic.API.Helpers
{
    public interface IFileValidator
    {
        (bool isValid, string errorMessage) IsValid(IFormFile file);
    }
}
