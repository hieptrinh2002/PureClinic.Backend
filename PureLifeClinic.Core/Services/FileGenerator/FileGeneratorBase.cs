﻿using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices.IFileGenarator;

namespace PureLifeClinic.Core.Services.FileGenerator;
public abstract class FileGeneratorBase<TCreateViewModel> : IFileGenerator<TCreateViewModel> where TCreateViewModel : class
{
    public async Task<ResponseViewModel<Stream>> GenerateFileAsync(TCreateViewModel model, CancellationToken cancellationToken)
    {
        // create common MemoryStream
        MemoryStream ms = new();
        await CreatePdfAsync(ms, model, cancellationToken);
        ms.Position = 0;

        return new ResponseViewModel<Stream>
        {
            Success = true,
            Data = ms,
            Message = "File created successfully"
        };
    }

    protected abstract Task CreatePdfAsync(Stream stream, TCreateViewModel model, CancellationToken cancellationToken);
}
