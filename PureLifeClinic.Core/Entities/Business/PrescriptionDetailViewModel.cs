using PureLifeClinic.Core.Entities.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.Business
{
    public class PrescriptionDetailViewModel
    {
        public int Quantity { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public MedicineViewModel Medicine { get; set; }

        public int MedicalReportId { get; set; }

    }

    public class PrescriptionDetailCreateViewModel
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Dosage cannot exceed 100 characters.")]
        public string Dosage { get; set; }

        [StringLength(500, ErrorMessage = "Instructions cannot exceed 500 characters.")]
        public string? Instructions { get; set; }
    }

    public class PrescriptionDetailUpdateViewModel
    {
    }
}
