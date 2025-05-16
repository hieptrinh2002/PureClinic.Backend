using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.FileViewModels.Response
{
    public class FileInfoVM
    {
        public FileType FileType { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public float FileSize { get; set; } = 0;

        public string FileName { get; set; } = string.Empty;

        public string cloudinaryFileUrl { get; set; } = string.Empty;

    }
}
