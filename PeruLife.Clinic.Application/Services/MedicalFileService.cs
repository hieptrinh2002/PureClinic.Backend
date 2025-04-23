using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.FileViewModels;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request;
using PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class MedicalFileService : BaseService<MedicalFile, MedicalFileViewModel>, IMedicalFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserContext _userContext;

        public MedicalFileService(IMapper mapper, IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IUserContext userContext)
            : base(mapper, unitOfWork.MedicalFiles)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _userContext = userContext;
        }

        public new async Task<ResponseViewModel<string>> Create(MedicalFileCreateViewModel model, CancellationToken cancellationToken)
        {
            var medicalfile = await _cloudinaryService.UploadFileAsync(model.File);
            medicalfile.EntryDate = DateTime.Now;
            medicalfile.EntryBy = Convert.ToInt32(_userContext.UserId);
            medicalfile.MedicalReportId = model.MedicalReportId;
            if (string.IsNullOrEmpty(medicalfile.FilePath))
            {
                throw new ErrorException("Upload medical failed");
            }

            var result = await _unitOfWork.MedicalFiles.Create(medicalfile, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel<string>
            {
                Success = true,
                Data = result.FilePath,
            };
        }

        public async Task<ResponseViewModel<List<MedicalFile>>> CreateMultipleAsync(MedicalFileMultiCreateViewModel model, CancellationToken cancellationToken)
        {
            var medicalReport = await _unitOfWork.MedicalReports.GetById(model.MedicalReportId, cancellationToken)
                ?? throw new NotFoundException("Medical report not found");

            var uploadedFiles = await _cloudinaryService.UploadFilesAsync(model.Files);

            foreach (var medicalFile in uploadedFiles)
            {
                medicalFile.MedicalReportId = model.MedicalReportId;
                medicalFile.EntryDate = DateTime.Now;
                medicalFile.EntryBy = Convert.ToInt32(_userContext.UserId);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var createTasks = uploadedFiles.Select(file => _unitOfWork.MedicalFiles.Create(file, cancellationToken));
            await Task.WhenAll(createTasks);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new ResponseViewModel<List<MedicalFile>>
            {
                Success = true,
                Data = uploadedFiles
            };
        }
    }
}
