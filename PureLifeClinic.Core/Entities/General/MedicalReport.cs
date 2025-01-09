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
        public string? Diagnosis { get; set; } // Chẩn đoán.

        [StringLength(1000)]
        public List<PrescriptionDetail>? PrescriptionDetails { get; set; }

        [StringLength(200)]
        public string? DoctorNotes { get; set; } // Ghi chú của bác sĩ.

        public IEnumerable<MedicalFile>? MedicalFiles { get; set; } = new List<MedicalFile>();  
    }

    public class PrescriptionDetail : Base<int>
    {
        public int Quantity { get; set; } // Số lượng thuốc
        public string Dosage { get; set; } // Liều dùng
        public string Instructions { get; set; } // Hướng dẫn sử dụng

        // Navigation properties

        public int MedicationId { get; set; }

        [ForeignKey(nameof(MedicationId))]
        public Medication Medication { get; set; }

        public int MedicalReportId { get; set; }

        [ForeignKey(nameof(MedicalReportId))]
        public MedicalReport MedicalReport { get; set; }
    }
}

