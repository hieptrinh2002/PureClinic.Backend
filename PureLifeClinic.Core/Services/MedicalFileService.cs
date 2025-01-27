using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
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
                throw new Exception("Upload medical failed");
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
            var uploadedFiles = await _cloudinaryService.UploadFilesAsync(new FileMultiUploadViewModel
            {
                Files = model.Files
            });

            foreach (var medicalFile in uploadedFiles)
            {
                medicalFile.MedicalReportId = model.MedicalReportId;
                medicalFile.EntryDate = DateTime.Now;
                medicalFile.EntryBy = Convert.ToInt32(_userContext.UserId);
            }

            foreach (var medicalFile in uploadedFiles)
            {
                await _unitOfWork.MedicalFiles.Create(medicalFile, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResponseViewModel<List<MedicalFile>>
            {
                Success = true,
                Data = uploadedFiles
            };
        }
    }
}
