using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Appointment : Base<int>
    {
        [Required]
        public DateTime AppointmentDate { get; set; } // Ngày giờ hẹn.

        [Required]
        public AppointmentReason Reason { get; set; } // Lý do khám (enum).

        [StringLength(500)]
        public string? OtherReason { get; set; } // Ghi chú lý do khác (nếu lý do không nằm trong enum).

        public int PatientId { get; set; } // FK tới bệnh nhân.
        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        public int DoctorId { get; set; } // FK tới bác sĩ.
        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; }

        public AppointmentStatus Status { get; set; } // Trạng thái cuộc hẹn (Pending, Completed, Canceled).

        public virtual ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>(); // Các báo cáo y tế trong cuộc hẹn.
    }

    public enum AppointmentStatus
    {
        Pending,
        Completed,
        Canceled
    }

    public enum AppointmentReason
    {
        Fever, // Sốt
        Headache, // Đau đầu
        Checkup, // Khám định kỳ
        Vaccination, // Tiêm phòng
        Other // Lý do khác
    }
}
