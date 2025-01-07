
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

        //// new 
        //public PatientStatus Status { get; set; }
        //public List<string> Tags { get; set; }
        //public ICollection<MedicalHistory> MedicalHistory { get; set; }
        //public ICollection<PatientImaging> Imaging { get; set; }
        //public ICollection<LabResult> LabResults { get; set; }
        //public ICollection<MedicationInteraction> Interactions { get; set; }
        //public ICollection<Allergy> Allergies { get; set; }
    }
    public enum PatientStatus
    {
        UnderTreatment,
        Stable,
        Discharged,
        Referred
    }
}