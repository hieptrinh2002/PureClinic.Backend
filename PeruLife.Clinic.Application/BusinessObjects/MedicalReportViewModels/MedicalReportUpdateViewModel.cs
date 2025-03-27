using PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels;

namespace PureLifeClinic.Application.BusinessObjects.MedicalReportViewModels
{
    public class MedicalReportUpdateViewModel
    {
        public int Id { get; set; }

        public string Findings { get; set; }

        public string? Recommendations { get; set; }

        public string? Diagnosis { get; set; }

        public List<PrescriptionDetailUpdateViewModel>? PrescriptionDetails { get; set; }

        public string? DoctorNotes { get; set; }
    }
}
