using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Enums.Queues;

namespace PureLifeClinic.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ExaminationQueueConfiguration : IEntityTypeConfiguration<ExaminationQueue>
    {
        public void Configure(EntityTypeBuilder<ExaminationQueue> builder)
        {
            builder.ToTable("ExaminationQueues");

            builder.HasKey(eq => eq.Id);

            builder.HasOne(eq => eq.Patient)
                .WithMany()
                .HasForeignKey(eq => eq.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(eq => eq.Doctor)
                .WithMany()
                .HasForeignKey(eq => eq.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(eq => eq.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(eq => eq.Priority)
                .HasDefaultValue(Priority.Standard);

            builder.Property(eq => eq.Status)
                .HasDefaultValue(QueueStatus.Waiting);

            builder.HasIndex(eq => eq.QueueNumber)
                .IsUnique();
        }
    }
}
