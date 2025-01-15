using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class MedicalReport : Base<int>
    {
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        [StringLength(1000)]
        public string Findings { get; set; }

        [StringLength(500)]
        public string? Recommendations { get; set; }

        [StringLength(500)]
        public string? Diagnosis { get; set; } 

        [StringLength(1000)]
        public List<PrescriptionDetail>? PrescriptionDetails { get; set; }

        [StringLength(200)]
        public string? DoctorNotes { get; set; } 

        public IEnumerable<MedicalFile>? MedicalFiles { get; set; } = new List<MedicalFile>();

        public int? InvoiceId { get; set; } 
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice? Invoice { get; set; }
    }
}
