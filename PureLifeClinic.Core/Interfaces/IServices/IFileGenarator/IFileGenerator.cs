using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices.IFileGenarator
{
    public interface IFileGenerator<TCreateViewModel> where TCreateViewModel : class
    {
        Task<ResponseViewModel<Stream>> GenerateFileAsync(TCreateViewModel model, CancellationToken cancellationToken);
    }
}
