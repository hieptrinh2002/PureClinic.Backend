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

    public class PatientAppointmentViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        public DoctorViewModel Doctor { get; set; }
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
        public DateTime AppointmentDate { get; set; }

        public string? Reason { get; set; }

        public string? OtherReason { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }
    }

    public class InPersonAppointmentCreateViewModel
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = "Anual health check";
        public string? ReferredPerson { get; set; }

        public int DoctorId { get; set; }
    }

    public class AppointmentUpdateViewModel
    {
        public DateTime AppointmentDate { get; set; }

        public string? Reason { get; set; }

        public int? DoctorId { get; set; }

        public AppointmentStatus Status { get; set; }

        public string? ReferredPerson { get; set; }
    }

    public class AppointmentStatusUpdateViewModel
    {
        public AppointmentStatus Status { get; set; }
    }

    public class AppointmentSlotViewModel
    {
        public DateTime WeekDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
