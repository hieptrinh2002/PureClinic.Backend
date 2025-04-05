using System.ComponentModel;

namespace PureLifeClinic.Core.Enums
{
    public enum CounterType
    {
        [Description("Ticketing Counter")]
        Ticketing,              // Counter for issuing queue numbers

        [Description("Registration Counter")]
        Registration,           // Patient check-in / registration

        [Description("Consultation Counter")]
        Consultation,           // Where doctors see patients

        [Description("Payment Counter")]
        Payment,                // For payments and invoices

        [Description("Pharmacy Counter")]
        Pharmacy,               // For dispensing medicine

        [Description("Laboratory Counter")]
        Laboratory,             // For lab tests and sample collection

        [Description("Imaging Counter")]
        Imaging,                // For X-rays, MRIs, CT scans

        [Description("Vaccination Counter")]
        Vaccination,            // For immunization

        [Description("Triage Counter")]
        Triage,                 // For patient condition assessment

        [Description("Billing / Insurance Counter")]
        Billing,                // For billing, insurance processing

        [Description("Information / Help Desk")]
        Information             // General support and assistance
    }

}
