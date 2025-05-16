using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using PureLifeClinic.Application.BusinessObjects.FileViewModels;
using PureLifeClinic.Application.BusinessObjects.FileViewModels.Response;
using PureLifeClinic.Application.Extentions.Mapping;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.ExternalServices
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<(string Url, string PublicId)> UploadImgAsync(IFormFile file, string folder)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return (result.SecureUrl.ToString(), result.PublicId);
        }

        private FileInfoVM CreateUploadParams(Stream fileStream, string fileName, string contentType)
        {
            var file = new FileInfoVM()
            {
                FileName = fileName,
                FileSize = fileStream.Length / 1024f, 
                FileType = contentType.ToLower() switch
                {
                    "image/jpeg" or "image/png" or "image/gif" => FileType.Image,
                    "video/mp4" or "video/avi" or "video/mkv" => FileType.Video,
                    "application/pdf" => FileType.PDF,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => FileType.Excel,
                    _ => FileType.Other
                },
            };

            return file;
        }

        public async Task<MedicalFile> UploadMedicalFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var medicalFileInfo = CreateUploadParams(stream, file.FileName, file.ContentType);
            var medicalFile = medicalFileInfo.MapToMedicalFile();
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };
            UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
            medicalFile.FilePath = uploadResult.SecureUrl.ToString();
            medicalFile.FilePathPublicId = uploadResult.PublicId;
            return medicalFile;
        }

        public async Task<(string Url, string PublicId)> UploadStreamFileAsync(Stream streamFile, string fileName, string contentType = "application/pdf")
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, streamFile)
            };
            UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return (uploadResult.SecureUrl.ToString(), uploadResult.PublicId) ;
        }


        // Upload multiple files in parallel
        public async Task<List<MedicalFile>> UploadMedicalFilesAsync(List<FileUploadViewModel> modelFiles)
        {
            var files = modelFiles.Select(f => f.FileDetails).ToList();
            var uploadTasks = files.Select(file => UploadMedicalFileAsync(file)); // create list Task<MedicalFile>
            var medicalFiles = await Task.WhenAll(uploadTasks); // parallel upload all files

            return medicalFiles.ToList();
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
