using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class PatientCreateViewModel
    {
        [StringLength(1000)]
        public string? Notes { get; set; } = string.Empty;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public int UserId { get; set; }

        [Required, ForeignKey(nameof(UserId))]
        public required User User { get; set; }

        public int? PrimaryDoctorId { get; set; }

        [ForeignKey(nameof(PrimaryDoctorId))]
        public Doctor? PrimaryDoctor { get; set; }

        // new 
        public PatientStatus Status { get; set; } = PatientStatus.New;
        public ICollection<LabResult>? LabResults { get; set; }
        public ICollection<MedicationInteraction>? Interactions { get; set; }
        public ICollection<Allergy>? Allergies { get; set; }
    }
}
