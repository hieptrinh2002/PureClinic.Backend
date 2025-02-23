using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IInvoiceService
    {
        Task<ResponseViewModel<Stream>> CreateInvoiceFileAsync(InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken);
    }
}
