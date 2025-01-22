using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class PrescriptionDetail : Base<int>
    {
        public int Quantity { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }

        // Navigation properties
        public int MedicineId { get; set; }

        [ForeignKey(nameof(MedicineId))]
        public Medicine Medicine { get; set; }

        public int MedicalReportId { get; set; }

        [ForeignKey(nameof(MedicalReportId))]
        public MedicalReport MedicalReport { get; set; }
    }
}
