using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
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

    public class InvoiceCreateViewModel
    {
        public int AppointmentId { get; set; }

        public double TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public bool IsPaid { get; set; }
    }
}
