
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Patient : User
    {
        [Required, StringLength(100)]
        public string Address { get; set; } // Địa chỉ bệnh nhân.

        [Required, StringLength(15)]
        public string Gender { get; set; } // Giới tính (VD: Nam, Nữ, Khác).

        [Required]
        public DateTime DateOfBirth { get; set; } // Ngày sinh.

        [StringLength(500)]
        public string MedicalHistory { get; set; } // Lịch sử bệnh án tổng quát.

        [StringLength(1000)]
        public string Notes { get; set; } // Ghi chú thêm của bác sĩ hoặc hệ thống.

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Cuộc hẹn khám.
    }
}