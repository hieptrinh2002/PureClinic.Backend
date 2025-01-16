using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<List<string>> UploadFilesAsync(List<IFormFile> files);
        Task<bool> DeleteFileAsync(string publicId);
        Task<bool> DeleteFilesAsync(List<string> publicIds);
    }
}
