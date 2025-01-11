using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Appointment : Base<int>
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Reason { get; set; } = "Anual health check";

        public int PatientId { get; set; }

        public string? ReferredPerson {  get; set; } // ng giới thiệu

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public ICollection<MedicalReport>? MedicalReports { get; set; } = new List<MedicalReport>();
    }

    public enum AppointmentStatus
    {
        Pending,
        Completed,
        Canceled
    }
}
