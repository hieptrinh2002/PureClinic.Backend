using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using PureLifeClinic.Application.BusinessObjects.FileViewModels;
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
        private MedicalFile CreateUploadParams(Stream fileStream, string fileName, string contentType)
        {
            var fileDescription = new FileDescription(fileName, fileStream);
            MedicalFile medicalFile = new MedicalFile
            {
                FileName = fileName,
                FileSize = fileStream.Length / 1024f, 
                FilePath = null, 
                FileType = contentType.ToLower() switch
                {
                    "image/jpeg" or "image/png" or "image/gif" => FileType.Image,
                    "video/mp4" or "video/avi" or "video/mkv" => FileType.Video,
                    "application/pdf" => FileType.PDF,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => FileType.Excel,
                    _ => FileType.Other
                },
                MedicalReportId = 0,
            };

            return medicalFile;
        }

        public async Task<MedicalFile> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var medicalFile = CreateUploadParams(stream, file.FileName, file.ContentType);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };
            UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
            medicalFile.FilePath = uploadResult.SecureUrl?.ToString();
            return medicalFile;
        }
        public async Task<string> UploadStreamFileAsync(Stream streamFile, string fileName, string contentType = "application/pdf")
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, streamFile)
            };
            UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl?.ToString();
        }

        // Upload multiple files
        public async Task<List<MedicalFile>> UploadFilesAsync(FileMultiUploadViewModel model)
        {
            var files = model.Files.Select(f => f.FileDetails).ToList();
            var medicalFiles = new List<MedicalFile>();

            foreach (var file in files)
            {
                var medicalFile = await UploadFileAsync(file); 
                medicalFiles.Add(medicalFile);
            }

            return medicalFiles;
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
