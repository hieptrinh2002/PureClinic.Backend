using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = string.Empty;   
        public string? FullName { get; set; }
        public string? Email { get; set; }

        public string? Specialty { get; set; }

        public string? Qualification { get; set; }

        public int? ExperienceYears { get; set; }

        public string? Description { get; set; }

        public string? RegistrationNumber { get; set; }
        public string? Role { get; set; }
    }
}
