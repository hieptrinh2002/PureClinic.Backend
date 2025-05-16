using Microsoft.AspNetCore.Http;
using PureLifeClinic.Application.BusinessObjects.FileViewModels;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<(string Url, string PublicId)> UploadImgAsync(IFormFile file, string folder);

        Task<MedicalFile> UploadMedicalFileAsync(IFormFile file);
        Task<(string Url, string PublicId)> UploadStreamFileAsync(Stream streamFile, string fileName, string contentType = "application/pdf");
        Task<bool> DeleteFileAsync(string publicId);
        Task<bool> DeleteFilesAsync(List<string> publicIds);
        Task<List<MedicalFile>> UploadMedicalFilesAsync(List<FileUploadViewModel> list);
    }
}
