using AutoMapper;
using Microsoft.AspNetCore.Http;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Interfaces.IServices.IFileGenarator;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class InvoiceService : BaseService<Invoice, InvoiceViewModel>, IInvoiceService
    {
        private readonly IFileGenerator<InvoiceFileCreateViewModel> _fileGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMailService _emailService;

        public InvoiceService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserContext userContext,
            IFileGenerator<InvoiceFileCreateViewModel> fileGenerator,
            ICloudinaryService cloudinaryService,
            IMailService mailService) : base(mapper, unitOfWork.Invoices)
        {
            _fileGenerator = fileGenerator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
            _cloudinaryService = cloudinaryService;
            _emailService = mailService;
        }

        public async Task<InvoiceViewModel> Create(InvoiceCreateViewModel model, CancellationToken cancellationToken)
        {
            var invoice = _mapper.Map<Invoice>(model);
            invoice.EntryDate = DateTime.Now;
            invoice.EntryBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Invoices.Create(invoice, cancellationToken);
            return _mapper.Map<InvoiceViewModel>(result);
        }

        public async Task<ResponseViewModel<Stream>> CreateInvoiceFileAsync(
            InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken)
        {
            return await _fileGenerator.GenerateFileAsync(invoice, cancellationToken);
        }

        private IFormFile ConvertStreamToIFormFile(Stream stream, string fileName, string contentType)
        {
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public async Task ProcessInvoiceAsync(InvoiceFileCreateViewModel model, Stream fileStream, CancellationToken cancellationToken)
        {
            // 1. upload file to cloudinary
            string fileName = $"invoice_{model.PatientInfo.PatientId}_{DateTime.UtcNow.Ticks}.pdf";
            var uploadedPath = await _cloudinaryService.UploadStreamFileAsync(fileStream, fileName);

            // 2. update invoice file path to db
            await UpdateFilePathToInvoiceAsync(model.AppoinmentId, uploadedPath);

            // 3. Send email to patient
            var mailRequest = new MailRequestViewModel
            {
                ToEmail = model.PatientInfo.Email,
                Subject = "Invoice",
                Body = "Please find the invoice attached.",
                Attachments = new List<IFormFile> { ConvertStreamToIFormFile(fileStream, fileName, "application/pdf") }
            };
            await _emailService.SendEmailAsync(mailRequest);
        }

        public async Task UpdateFilePathToInvoiceAsync(int appoinmentId, string uploadedPath)
        {
            // Get invoice by appointment id
            var invoice = await _unitOfWork.Invoices.GetInvoiceByAppoinmentId(appoinmentId)
                ?? throw new NotFoundException("Invoice not found, please create the Invoice before update");

            invoice.UpdatedDate = DateTime.Now;
            invoice.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            invoice.FilePath = uploadedPath;
            await _unitOfWork.Invoices.Update(invoice, default);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
