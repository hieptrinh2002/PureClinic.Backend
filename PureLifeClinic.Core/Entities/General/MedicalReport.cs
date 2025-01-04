using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class MedicalReport : Base<int>
    {
        public int AppointmentId { get; set; } // FK tới cuộc hẹn.
        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; }

        [Required]
        public DateTime ReportDate { get; set; } // Ngày lập báo cáo.

        [StringLength(1000)]
        public string Findings { get; set; } // Kết luận.

        [StringLength(500)]
        public string? Recommendations { get; set; } // Khuyến nghị điều trị.

        public string FilePath { get; set; } // Đường dẫn tệp báo cáo (nếu có).

        [StringLength(500)]
        public string? Diagnosis { get; set; } // Chẩn đoán.

        [StringLength(1000)]
        public string? Prescription { get; set; } // Đơn thuốc.

        [StringLength(200)]
        public string? DoctorNotes { get; set; } // Ghi chú của bác sĩ.
    }
}

