using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public string? Specialty { get; set; } // Chuyên môn của bác sĩ (VD: Nội khoa, Nhi khoa).

        public string? Qualification { get; set; } // Bằng cấp và chứng chỉ (VD: MD, PhD).

        public int? ExperienceYears { get; set; } // Số năm kinh nghiệm.

        public string? Description { get; set; } // Mô tả hoặc tiểu sử bác sĩ.

        public string? RegistrationNumber { get; set; } // Số đăng ký y tế (VD: do bộ y tế cấp).
        public string? Role { get; set; }
    }

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

    public class DoctorCreateViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Specialty { get; set; } // Chuyên môn của bác sĩ (VD: Nội khoa, Nhi khoa).

        public string? Qualification { get; set; } // Bằng cấp và chứng chỉ (VD: MD, PhD).

        public int? ExperienceYears { get; set; } // Số năm kinh nghiệm.

        public string? Description { get; set; } // Mô tả hoặc tiểu sử bác sĩ.

        public string? RegistrationNumber { get; set; } // Số đăng ký y tế (VD: do bộ y tế cấp).
        public string? Role { get; set; }
    }
}
