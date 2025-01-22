using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Invoice : Base<int>
    {
        [Required]
        public double TotalAmount { get; set; }

        [StringLength(500)]
        public string? PaymentMethod { get; set; }

        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment Appointment { get; set; }

        public void CalculateTotalAmount()
        {
            double total = 0;

            if (Appointment?.MedicalReports != null)
            {
                foreach (var report in Appointment.MedicalReports)
                {
                    if (report?.PrescriptionDetails != null)
                    {
                        foreach (var prescription in report.PrescriptionDetails)
                        {
                            total += prescription.Quantity * prescription.Medicine.Price;
                        }
                    }
                }
            }

            double serviceFee = 50; // default fee for health check
            total += serviceFee;

            TotalAmount = total;
        }

        private double CalculateMedicineCost()
        {
            double medicineCost = 0;

            if (Appointment?.MedicalReports != null)
            {
                foreach (var report in Appointment.MedicalReports)
                {
                    if (report?.PrescriptionDetails != null)
                    {
                        foreach (var prescription in report.PrescriptionDetails)
                        {
                            medicineCost += prescription.Quantity * prescription.Medicine.Price;
                        }
                    }
                }
            }

            return medicineCost;
        }
    }
}
