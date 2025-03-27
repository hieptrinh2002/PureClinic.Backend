using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices.IFileGenarator
{
    public interface IFileGenerator<TCreateViewModel> where TCreateViewModel : class
    {
        Task<ResponseViewModel<Stream>> GenerateFileAsync(TCreateViewModel model, CancellationToken cancellationToken);
    }
}
