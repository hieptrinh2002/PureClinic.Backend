using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Entities.General.Queues;
using System.Security.Claims;
using System.Text;

namespace PureLifeClinic.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<ConsultationQueue> ConsultationQueues { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<ExaminationQueue> ExaminationQueues { get; set; }
        public DbSet<ActionOnResource> ActionOnResources { get; set; }
        public DbSet<Resource> Resources { get; set; }

        //public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<DoctorFeedback> DoctorFeedbacks { get; set; }
        public DbSet<ClinicFeedBack> ClinicFeedBacks { get; set; }
        public DbSet<HealthServiceFeedback> HealthServiceFeedbacks { get; set; }

        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }  

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added || entity.State == EntityState.Deleted || entity.State == EntityState.Modified)
                .ToList();

            foreach (var modifiedEntity in modifiedEntities)
            {
                var auditLog = new AuditLog
                {
                    Action = modifiedEntity.State.ToString(),
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    PerformedBy = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name),
                    Timestamp = DateTime.UtcNow,
                    Details = GetChanges(modifiedEntity),
                    IPAddress = _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString()
                };
                AuditLogs.Add(auditLog);
            };

            return base.SaveChangesAsync(cancellationToken);
        }

        private string GetChanges(EntityEntry modifiedEntity)
        {
            var changes = new StringBuilder();
            foreach( var property in modifiedEntity.OriginalValues.Properties)
            {
                var originVal = modifiedEntity.OriginalValues[property];
                var currentVal = modifiedEntity.CurrentValues[property];
                if(!Equals(originVal, currentVal))
                {
                   changes.AppendLine($"{property.Name}: From '{originVal}' to '{currentVal}'");
                } 
            }    
                
            return changes.ToString();  
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationDbContextConfigurations.Configure(builder);

            builder.Entity<User>()
               .HasIndex(e => e.PhoneNumber)
               .IsUnique();

            builder.ApplyConfigurationsFromAssembly(builder.GetType().Assembly);
        }
    }
}
