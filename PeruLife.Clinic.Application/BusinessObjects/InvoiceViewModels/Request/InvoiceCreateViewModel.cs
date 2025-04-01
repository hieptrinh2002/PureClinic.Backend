using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request
{
    public class InvoiceCreateViewModel
    {
        public int AppointmentId { get; set; }

        public double TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public bool IsPaid { get; set; }
    }
}
