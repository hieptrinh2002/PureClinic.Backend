using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } 

        public AppointmentStatus Status { get; set; }

        public PatientViewModel? Patient { get; set; }

        public DoctorViewModel? Doctor { get; set; }
    }
    public class DoctorAppointmentViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public PatientViewModel Patient { get; set; }
    }

    public class AppointmentCreateViewModel
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string? Reason { get; set; }

        [StringLength(500)]
        public string? OtherReason { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }
    }

    public class InPersonAppointmentCreateViewModel
    {
        [Required, StringLength(maximumLength: 100)]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Reason { get; set; } = "Anual health check";
        public string? ReferredPerson { get; set; }

        public int DoctorId { get; set; }
    }

    public class AppointmentUpdateViewModel
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(255)]
        public string? Reason { get; set; }

        public int? DoctorId { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        public string? ReferredPerson { get; set; }
    }

    public class AppointmentStatusUpdateViewModel
    {
        [Required]
        public AppointmentStatus Status { get; set; }
    }
}
