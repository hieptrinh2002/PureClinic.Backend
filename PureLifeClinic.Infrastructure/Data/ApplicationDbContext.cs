using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region DbSet Section
        public DbSet<Product> Products { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalReport> MedicalReports { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<WorkWeek> WorkWeeks { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<MedicineInteraction> MedicineInteractions { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationDbContextConfigurations.Configure(builder);

            builder.Entity<User>()
               .HasIndex(e => e.PhoneNumber)
               .IsUnique();
        }
    }
}
