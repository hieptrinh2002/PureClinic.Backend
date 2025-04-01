using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Response
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int? EntryBy { get; set; }
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public string EntryDate { get; set; }
    }
}
