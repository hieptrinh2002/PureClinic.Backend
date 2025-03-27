namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File
{
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
}
