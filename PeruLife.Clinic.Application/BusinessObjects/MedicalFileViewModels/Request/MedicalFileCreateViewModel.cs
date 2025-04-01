using Microsoft.AspNetCore.Http;
using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.MedicalFileViewModels.Request
{
    public class MedicalFileCreateViewModel
    {
        public int MedicalReportId { get; set; }

        public IFormFile File { get; set; }

        [StringLength(200, ErrorMessage = "File Name cannot exceed 200 characters.")]
        public string? FileName { get; set; }

        public FileType FileType { get; set; }
    }
}
