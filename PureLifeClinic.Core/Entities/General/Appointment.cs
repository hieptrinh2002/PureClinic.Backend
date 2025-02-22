using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Appointment : Base<int>
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        public TimeSpan StartTime => AppointmentDate.TimeOfDay;

        public TimeSpan EndTime => StartTime.Add(TimeSpan.FromMinutes(Constants.AvgAppointmentTimeInMinute));

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
        public ICollection<AppointmentHealthService>? HealthServices { get; set; } = new List<AppointmentHealthService>();
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Completed,
        Canceled
    }
}
