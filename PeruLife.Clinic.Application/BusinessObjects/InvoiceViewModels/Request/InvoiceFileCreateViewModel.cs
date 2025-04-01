using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Response;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request
{
    public class InvoiceFileCreateViewModel
    {
        public string CLinicLogoUrl { get; set; }

        public int AppoinmentId { get; set; }

        public InvoiceClinicInfoViewModel ClinicInfo { get; set; }

        public InvoiceDoctorInfoViewModel DoctorInfo { get; set; }

        public InvoicePatientInfoViewModel PatientInfo { get; set; }

        public List<InvoiceMedicationViewModel> Medications { get; set; }

        public List<InvoiceUsedServiceViewModel> Services { get; set; }

        public InvoiceBreakdownViewModel InvoiceBreakdown { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string CreateBy { get; set; }

        public bool IsPaid { get; set; }
    }

}
