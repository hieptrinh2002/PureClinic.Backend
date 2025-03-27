using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.MedicalReportViewModels
{
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

        [StringLength(200)]
        public string? DoctorNotes { get; set; }
    }
}
