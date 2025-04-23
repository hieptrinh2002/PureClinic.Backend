using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices.IFileGenarator;

namespace PureLifeClinic.Application.Services.FileGenerator;
public abstract class FileGeneratorBase<TCreateViewModel> : IFileGenerator<TCreateViewModel> where TCreateViewModel : class
{
    public ResponseViewModel<Stream> GenerateFileAsync(TCreateViewModel model, CancellationToken cancellationToken)
    {
        // create common MemoryStream
        MemoryStream ms = new();
        if (!CreatePdfAsync(ms, model, cancellationToken))
        {
            return new ResponseViewModel<Stream>
            {
                Success = false,
                Message = "File creation failed"
            };  
        }
        ms.Position = 0;

        return new ResponseViewModel<Stream>
        {
            Success = true,
            Data = ms,
            Message = "File created successfully"
        };
    }

    protected abstract bool CreatePdfAsync(Stream stream, TCreateViewModel model, CancellationToken cancellationToken);
}
