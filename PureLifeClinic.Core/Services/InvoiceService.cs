using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Interfaces.IServices.IFileGenarator;

namespace PureLifeClinic.Core.Services
{
    public class InvoiceService : BaseService<Invoice, InvoiceViewModel>, IInvoiceService
    {
        private readonly IFileGenerator<InvoiceFileCreateViewModel> _fileGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        public InvoiceService(
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IUserContext userContext,   
            IFileGenerator<InvoiceFileCreateViewModel> fileGenerator) : base(mapper, unitOfWork.Invoices)
        {
            _fileGenerator = fileGenerator;
            _unitOfWork = unitOfWork;   
            _mapper = mapper;
            _userContext = userContext; 
        }

        public async Task<InvoiceViewModel> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken)
        {
            var invoice = _mapper.Map<Invoice>(model);
            invoice.EntryDate = DateTime.Now;
            invoice.EntryBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Invoices.Create(invoice, cancellationToken);
            return _mapper.Map<InvoiceViewModel>(result);
        }

        public async Task<ResponseViewModel<Stream>> CreateInvoiceFileAsync(InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken)
        {
            return await _fileGenerator.GenerateFileAsync(invoice, cancellationToken);
        }

        public async Task UpdateFilePathToInvoiceAsync(int appoinmentId, string uploadedPath)
        {
            // Get invoice by appointment id
            var invoice = await _unitOfWork.Invoices.GetInvoiceByAppoinmentId(appoinmentId);
            if(invoice == null)
                throw new NotFoundException("Invoice not found, please create the Invoice before update");   
            
            invoice.UpdatedDate = DateTime.Now;
            invoice.UpdatedBy = Convert.ToInt32(_userContext.UserId);   
            invoice.FilePath = uploadedPath;
            await _unitOfWork.Invoices.Update(invoice, default);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
