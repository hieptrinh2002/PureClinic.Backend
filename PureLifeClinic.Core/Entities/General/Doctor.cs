using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Doctor : Base<int>
    {
        [Required, MaxLength(100)]
        public string? Specialty { get; set; } 

        public string? Qualification { get; set; } // Bằng cấp và chứng chỉ (VD: MD, PhD).

        public int? ExperienceYears { get; set; }

        public string? Description { get; set; } 

        public string? RegistrationNumber { get; set; } // Số đăng ký y tế (VD: do bộ y tế cấp).

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }

        public ICollection<Patient> PrimaryPatients { get; set; } = new List<Patient>(); // Bệnh nhân chịu trách nhiệm chính

        public ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
    }
}
