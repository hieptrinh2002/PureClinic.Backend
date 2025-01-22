using Microsoft.AspNetCore.Http;
using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class MedicalFileViewModel
    {
    }
    public class MedicalFileCreateViewModel
    {
        public IFormFile? File { get; set; }

        public float? FileSize { get; set; }

        [StringLength(200, ErrorMessage = "File Name cannot exceed 200 characters.")]
        public string? FileName { get; set; }

        public FileType FileType { get; set; }
    }
    public class MedicalFileUpdateViewModel
    {
    }
}
