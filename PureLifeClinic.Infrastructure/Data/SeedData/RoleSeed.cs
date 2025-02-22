using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class RoleSeed
    {
        public static IEnumerable<Role> SeedRoleData()
        {
            return new List<Role>
            {
                new() {Code="ADMIN", Name = "Admin", NormalizedName="ADMIN", IsActive = true, EntryDate= DateTime.Now },
                new() {Code="EMPLOYEE", Name = "Employee", NormalizedName="EMPLOYEE", IsActive = true, EntryDate= DateTime.Now },
                new() {Code="PATIENT", Name = "Patient", NormalizedName= "PATIENT", IsActive = true, EntryDate= DateTime.Now },
                new() {Code="DOCTOR", Name = "Doctor", NormalizedName= "DOCTOR", IsActive = true, EntryDate= DateTime.Now },
            };
        }
    }
}
