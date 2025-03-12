namespace PureLifeClinic.Core.Common.Constants
{
    public static class PermissionConstants
    {
        public const int Deny = -1;

        public const string Product = nameof(Product);
        public const string MedicalReport = nameof(MedicalReport);
        public const string Doctor = nameof(Doctor);
        public const string Patient = nameof(Patient);
        public const string User = nameof(User);
        public const string Role = nameof(Role);
        public const string Permission = nameof(Permission);
        public const string Appointment = nameof(Appointment);
        public const string Invoice = nameof(Invoice);
        public const string HealthService = nameof(HealthService);  

        public static readonly List<string> Permissions = new List<string> {
            Product,
            MedicalReport,
            Doctor,
            Patient,
            User,
            Role,
            Permission,
            Appointment,
            Invoice,
            HealthService
        };
    }
}
