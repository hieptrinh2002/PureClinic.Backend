using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class PrescriptionDetail : Base<int>
    {
        public int Quantity { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }

        // Navigation properties
        public int MedicationId { get; set; }

        [ForeignKey(nameof(MedicationId))]
        public Medication Medication { get; set; }

        public int MedicalReportId { get; set; }

        [ForeignKey(nameof(MedicalReportId))]
        public MedicalReport MedicalReport { get; set; }
    }
}
