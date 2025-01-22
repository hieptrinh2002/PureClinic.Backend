namespace PureLifeClinic.API.Helpers
{
    public class FileValidator : IFileValidator
    {
        private readonly IConfiguration _configuration;
        private readonly int _imageAndDocumentSizeLimit;
        private readonly int _videoSizeLimit;
        private readonly string[] _imageAndDocumentExtensions;
        private readonly string[] _videoExtensions;

        public FileValidator(IConfiguration configuration)
        {
            _configuration = configuration;
            _imageAndDocumentSizeLimit = 4 * 1024 * 1024; // 4MB
            _videoSizeLimit = 100 * 1024 * 1024; // 100MB
            _imageAndDocumentExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".xlsx" };
            _videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov" };
        }

        public (bool isValid, string errorMessage) IsValid(IFormFile file)
        {
            if (file?.Length == 0)
                return (false, "File is empty.");

            // Get the file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension))
                return (false, "File extension is missing.");

            // Check for image/document types
            if (_imageAndDocumentExtensions.Contains(extension))
            {
                if (file.Length > _imageAndDocumentSizeLimit)
                    return (false, "File size exceeds the 4MB limit for images and documents.");

                return (true, string.Empty);
            }

            // Check for video types
            if (_videoExtensions.Contains(extension))
            {
                if (file.Length > _videoSizeLimit)
                    return (false, "File size exceeds the 100MB limit for videos.");

                return (true, string.Empty);
            }

            // If the file extension doesn't match any of the allowed types
            return (false, "Invalid file extension.");
        }
    }
}
