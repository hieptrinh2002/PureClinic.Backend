namespace PureLifeClinic.Core.Entities.Business
{
    public class GenerateInvoiceFileViewModel { }

    public class InvoiceFileCreateViewModel
    {
        public InvoiceClinicInfoViewModel ClinicInfo { get; set; }

        public InvoiceDoctorInfoViewModel DoctorInfo { get; set; }

        public InvoicePatientInfoViewModel PatientInfo { get; set; }

        public List<InvoiceMedicationViewModel> Medications { get; set; }

        public List<InvoiceUsedServiceViewModel> Services { get; set; }

        public InvoiceBreakdownViewModel InvoiceBreakdown { get; set; }

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

        public decimal Price { get; set; }
    }

    public class InvoiceUsedServiceViewModel
    {
        // Tên dịch vụ
        public string ServiceName { get; set; }

        // Giá dịch vụ
        public decimal Price { get; set; }

        // Số lượng sử dụng dịch vụ (nếu cần)
        public int Quantity { get; set; }
    }

    public class InvoiceBreakdownViewModel
    {
        // Tổng tiền thuốc
        public decimal MedicationTotal { get; set; }
        // Tổng tiền dịch vụ
        public decimal ServiceTotal { get; set; }
        // Số tiền giảm giá
        public decimal DiscountAmount { get; set; }
        // Thuế VAT (nếu có)
        public decimal TaxAmount { get; set; }
        // Tổng thanh toán cuối cùng
        public decimal GrandTotal { get; set; }
    }

    public class MedicalReportFileCreateViewModel
    {

    }
}
