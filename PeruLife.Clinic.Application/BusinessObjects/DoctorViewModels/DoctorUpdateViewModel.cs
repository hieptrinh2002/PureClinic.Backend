using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels
{
    public class DoctorUpdateViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string? FullName { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string? UserName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(100)]
        public string? Specialty { get; set; }

        [StringLength(500)]
        public string? Qualification { get; set; }

        [Required]
        public int? ExperienceYears { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(50)]

        public string? RegistrationNumber { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
