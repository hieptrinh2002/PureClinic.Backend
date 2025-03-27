using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels
{
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
}
