using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using System.Reflection.Emit;

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
        public DbSet<HealthService> HealthServices { get; set; }
        public DbSet<AppointmentHealthService> AppointmentHealthServices { get; set; }
        public DbSet<MedicalReport> MedicalReports { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<WorkWeek> WorkWeeks { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<MedicineInteraction> MedicineInteractions { get; set; }
        public DbSet<MedicalFile> MedicalFiles { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationDbContextConfigurations.Configure(builder);

            builder.Entity<User>()
               .HasIndex(e => e.PhoneNumber)
               .IsUnique();

            builder.Entity<IdentityUserRole<int>>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict); // ❌ Không cascade khi xóa User

            builder.Entity<IdentityUserRole<int>>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Xóa Role thì xóa luôn UserRoles
        }
    }
}
