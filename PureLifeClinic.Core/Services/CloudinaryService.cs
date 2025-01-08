using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        // Upload a single file
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl?.ToString();
        }

        // Upload multiple files
        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files)
        {
            var urls = new List<string>();
            foreach (var file in files)
            {
                var url = await UploadFileAsync(file);
                urls.Add(url);
            }
            return urls;
        }

        // Delete a single file
        public async Task<bool> DeleteFileAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        // Delete multiple files
        public async Task<bool> DeleteFilesAsync(List<string> publicIds)
        {
            var deletionResults = new List<bool>();
            foreach (var publicId in publicIds)
            {
                var success = await DeleteFileAsync(publicId);
                deletionResults.Add(success);
            }
            return deletionResults.All(r => r);
        }

        // Get file details (metadata)
        public async Task<string> GetFileDetailsAsync(string publicId)
        {
            var resourceParams = new GetResourceParams(publicId);
            var resource = await _cloudinary.GetResourceAsync(resourceParams);
            return resource?.SecureUrl;
        }
    }
}
