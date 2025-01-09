using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class AppointmentViewModel
    {
        public DateTime? AppointmentDate { get; set; }

        public string? Reason { get; set; }

        public AppointmentStatus Status { get; set; } 

        public PatientViewModel? Patient { get; set; }

        public DoctorViewModel? Doctor { get; set; }
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

        [Required]
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string? Reason { get; set; }
        public string? ReferredPerson { get; set; }

        public int DoctorId { get; set; }
    }

    public class AppointmentUpdateStatusViewModel
    {
        [Required]
        public AppointmentStatus AppointmentDate { get; set; }
    }
}
