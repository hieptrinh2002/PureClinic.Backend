using PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels;

namespace PureLifeClinic.Application.BusinessObjects.MedicalReportViewModels
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
}
