using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

        private BaseParams CreateUploadParams(Stream fileStream, string fileName, string contentType)
        {
            var fileDescription = new FileDescription(fileName, fileStream);

            return contentType.ToLower() switch
            {
                "image/jpeg" or "image/png" or "image/gif" => new ImageUploadParams { File = fileDescription },
                "video/mp4" or "video/avi" or "video/mkv" => new VideoUploadParams { File = fileDescription },
                "application/pdf" or "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    => new RawUploadParams { File = fileDescription },
                _ => throw new NotSupportedException("Unsupported file type"),
            };
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = CreateUploadParams(stream, file.FileName, file.ContentType);

            UploadResult uploadResult = uploadParams switch
            {
                VideoUploadParams imageParams => await _cloudinary.UploadAsync(imageParams),
                ImageUploadParams imageParams => await _cloudinary.UploadAsync(imageParams),
                RawUploadParams rawParams => await _cloudinary.UploadAsync(rawParams),
                _ => throw new InvalidOperationException("Invalid upload parameters"),
            };

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
    }
}
