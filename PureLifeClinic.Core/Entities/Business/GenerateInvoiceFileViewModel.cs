using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Entities.Business
{
    public class GenerateInvoiceFileViewModel { }

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

    public class InvoiceClinicInfoViewModel
    {
        public string ClinicName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class InvoiceDoctorInfoViewModel
    {
        public string DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string Specialization { get; set; }
    }

    public class InvoicePatientInfoViewModel
    {
        public string PatientId { get; set; }   

        public string PatientName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class InvoiceMedicationViewModel
    {
        public string MedicationName { get; set; }

        public string UsageInstructions { get; set; }

        public int Dosage { get; set; }

        public double Price { get; set; }
    }

    public class InvoiceUsedServiceViewModel
    {
        public string ServiceName { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }

    public class InvoiceBreakdownViewModel
    {
        public double MedicationTotal { get; set; }
        public double ServiceTotal { get; set; }
        public double DiscountAmount { get; set; }

        // VAT
        public double TaxAmount { get; set; }

        // total = (medicationTotal + serviceTotal) - discountAmount + taxAmount
        public double GrandTotal { get; set; }
    }

    public class MedicalReportFileCreateViewModel
    {

    }
}
