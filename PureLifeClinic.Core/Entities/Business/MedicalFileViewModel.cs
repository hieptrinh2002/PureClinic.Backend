using Microsoft.AspNetCore.Http;
using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class MedicalFileViewModel
    {
        public string url { get; set; } 
    }
    public class MedicalFileCreateViewModel
    {
        public int MedicalReportId { get; set; }    

        public IFormFile File { get; set; }

        [StringLength(200, ErrorMessage = "File Name cannot exceed 200 characters.")]
        public string? FileName { get; set; }

        public FileType FileType { get; set; }
    }
    public class MedicalFileMultiCreateViewModel
    {
        public int MedicalReportId { get; set; }
        public List<FileUploadViewModel> Files { get; set; } = new List<FileUploadViewModel>();
    }

    public class FileUploadViewModel
    {
        public IFormFile FileDetails { get; set; }

        [StringLength(200, ErrorMessage = "File Name cannot exceed 200 characters.")]
        public string? FileName { get; set; }
        public FileType FileType { get; set; }
    }

    public class FileMultiUploadViewModel
    {
        public int MedicalReportId { get; set; }

        public List<FileUploadViewModel> Files { get; set; } = new List<FileUploadViewModel>();
    } 

    public class MedicalFileUpdateViewModel
    {
    }
}
