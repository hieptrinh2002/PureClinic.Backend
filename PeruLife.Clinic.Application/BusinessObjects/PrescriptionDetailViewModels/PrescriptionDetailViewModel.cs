using PureLifeClinic.Application.BusinessObjects.MedicineViewModels.Response;

namespace PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels
{
    public class PrescriptionDetailViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public MedicineViewModel Medicine { get; set; }

        public int MedicalReportId { get; set; }
    }
}
