using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Interfaces.IServices.IFileGenarator;

namespace PureLifeClinic.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IFileGenerator<InvoiceFileCreateViewModel> _fileGenerator;

        public InvoiceService(IFileGenerator<InvoiceFileCreateViewModel> fileGenerator)
        {
            _fileGenerator = fileGenerator;
        }

        public async Task<ResponseViewModel<Stream>> CreateInvoiceFileAsync(InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken)
        {
            return await _fileGenerator.GenerateFileAsync(invoice, cancellationToken);
        }
    }
}
