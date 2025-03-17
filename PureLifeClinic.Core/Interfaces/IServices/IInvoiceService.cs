using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IInvoiceService : IBaseService<InvoiceViewModel>
    {
        Task<ResponseViewModel<Stream>> CreateInvoiceFileAsync(InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken);
        Task<InvoiceViewModel> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken);
        Task UpdateFilePathToInvoiceAsync(int appoinmentId, string uploadedPath);
        Task ProcessInvoiceAsync(InvoiceFileCreateViewModel model, Stream fileStream, CancellationToken cancellationToken);
    }
}
