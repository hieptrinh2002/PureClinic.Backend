using Microsoft.AspNetCore.Http;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<MedicalFile> UploadFileAsync(IFormFile file);
        Task<string> UploadStreamFileAsync(Stream streamFile, string fileName, string contentType = "application/pdf");
        Task<bool> DeleteFileAsync(string publicId);
        Task<bool> DeleteFilesAsync(List<string> publicIds);
        Task<List<MedicalFile>> UploadFilesAsync(FileMultiUploadViewModel model);
    }
}
