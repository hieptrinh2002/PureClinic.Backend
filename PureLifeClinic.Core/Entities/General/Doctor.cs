using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Entities.General
{
    public class Doctor : User
    {
        [Required, StringLength(100)]
        public string Specialty { get; set; } // Chuyên môn của bác sĩ (VD: Nội khoa, Nhi khoa).

        [StringLength(500)]
        public string Qualification { get; set; } // Bằng cấp và chứng chỉ (VD: MD, PhD).

        [Required]
        public int ExperienceYears { get; set; } // Số năm kinh nghiệm.

        [StringLength(500)]
        public string Description { get; set; } // Mô tả hoặc tiểu sử bác sĩ.

        [Required, StringLength(50)]
        public string RegistrationNumber { get; set; } // Số đăng ký y tế (VD: do bộ y tế cấp).

    }
}
