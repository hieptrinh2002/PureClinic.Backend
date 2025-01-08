using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Appointment : Base<int>
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public AppointmentReason Reason { get; set; }

        [StringLength(500)]
        public string? OtherReason { get; set; }

        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>();
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
        ReExamination,// tái khám
        Other // Lý do khác
    }
}
