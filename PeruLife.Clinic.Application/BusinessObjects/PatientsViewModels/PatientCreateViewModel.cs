using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Application.BusinessObjects.PatientsViewModels
{
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
        public ICollection<MedicineInteraction>? Interactions { get; set; }
        public ICollection<Allergy>? Allergies { get; set; }
    }
}
