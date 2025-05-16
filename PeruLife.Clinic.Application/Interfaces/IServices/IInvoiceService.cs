using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IInvoiceService : IBaseService<InvoiceViewModel>
    {
        ResponseViewModel<Stream> CreateInvoiceFileAsync(InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken);
        Task<InvoiceViewModel> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken);
        Task UpdateFilePathToInvoiceAsync(int appoinmentId, string uploadedPath, string uploadedPathPublicId);
        Task ProcessInvoiceAsync(InvoiceFileCreateViewModel model, Stream fileStream, CancellationToken cancellationToken);
    }
}
