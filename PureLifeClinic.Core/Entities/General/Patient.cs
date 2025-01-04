
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Patient: Base<int>
    {
        [Required, StringLength(100)]
        public string? MedicalHistory { get; set; } // Lịch sử bệnh án tổng quát.

        [StringLength(1000)]
        public string? Notes { get; set; } // Ghi chú thêm của bác sĩ hoặc hệ thống.

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Cuộc hẹn khám.

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }  
    }
}