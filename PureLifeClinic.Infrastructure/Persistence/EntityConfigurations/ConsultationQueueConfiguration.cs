
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureLifeClinic.Core.Entities.General.Queues;

namespace PureLifeClinic.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ConsultationQueueConfiguration : IEntityTypeConfiguration<ConsultationQueue>
    {
        public void Configure(EntityTypeBuilder<ConsultationQueue> builder)
        {
            builder.HasOne(cq => cq.Patient)
               .WithMany()
               .HasForeignKey(cq => cq.PatientId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(cq => cq.Counter)
                .WithMany()
                .HasForeignKey(cq => cq.CounterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(cq => cq.QueueNumber)
                .IsUnique();
        }
    }
}
