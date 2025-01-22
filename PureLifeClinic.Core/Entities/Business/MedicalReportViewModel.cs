using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class MedicalReportViewModel
    {
        public int Id { get; set; } 

        public DateTime? EntryDate { get; set; } 

        public int AppointmentId { get; set; }

        public DateTime? ReportDate { get; set; }

        public string? Findings { get; set; }

        public string? Recommendations { get; set; }

        public string? Diagnosis { get; set; }

        public List<PrescriptionDetailViewModel>? PrescriptionDetails { get; set; }

        public string? DoctorNotes { get; set; }

        public List<string> MedicalFiles { get; set; } = new List<string>();
    }

    public class MedicalReportCreateViewModel
    {
        public int AppointmentId { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        [StringLength(1000)]
        public string Findings { get; set; }

        [StringLength(500)]
        public string? Recommendations { get; set; }

        [StringLength(500)]
        public string? Diagnosis { get; set; }

        [StringLength(1000)]
        public List<PrescriptionDetailCreateViewModel>? PrescriptionDetails { get; set; } =  new List<PrescriptionDetailCreateViewModel>();

        [StringLength(200)]
        public string? DoctorNotes { get; set; }

        public IEnumerable<MedicalFileCreateViewModel>? MedicalFiles { get; set; } = new List<MedicalFileCreateViewModel>();

        public int? InvoiceId { get; set; }
    }

    public class MedicalReportUpdateViewModel
    {

    }
}
